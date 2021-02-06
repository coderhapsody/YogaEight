using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class ReceivingProvider : BaseProvider
    {
        private readonly AutoNumberProvider autoNumberProvider;

        public ReceivingProvider(FitnessEntities context,
                                 IPrincipal principal,
                                 AutoNumberProvider autoNumberProvider) : base(context, principal)
        {
            this.autoNumberProvider = autoNumberProvider;            
        }

        public void VoidReceiving(int receivingID, string reason)
        {
            var receiveHeader = context.ReceivingHeaders.SingleOrDefault(x => x.ID == receivingID);

            if (receiveHeader == null) return;

            receiveHeader.Status = 'V';
            receiveHeader.VoidDate = DateTime.Today;
            receiveHeader.VoidReason = reason;

            var order = receiveHeader.PurchaseOrderHeader;
            if (
                context.ReceivingHeaders.Any(
                    rcv => rcv.PurchaseOrderID == order.ID && rcv.ID != receivingID && !rcv.VoidDate.HasValue))
            {
                order.StatusReceiving = 'P';
                order.IsFullReceived = false;
            }
            else
            {
                order.StatusReceiving = 'O';
                order.IsFullReceived = false;
            }

            context.SaveChanges();
        }

        public string AddOrUpdateReceiving(int id, int poID, DateTime date, string goodIssueNo,
            string freightInfo, int warehouseID, string notes, List<ReceivingDetailViewModel> detail)
        {

            var receivingDetails = context.ReceivingHeaders
                                          .Where(rcv => rcv.PurchaseOrderID == poID && rcv.ID != id && !rcv.VoidDate.HasValue)
                                          .SelectMany(x => x.ReceivingDetails)
                                          .ToList();
            var order = context.PurchaseOrderHeaders.Single(po => po.ID == poID && !po.VoidWhen.HasValue);
            var orderDetails = order.PurchaseOrderDetails.ToList();

            var receiveHeader = id == 0 ? new ReceivingHeader() : context.ReceivingHeaders.Single(rh => rh.ID == id);

            if (id == 0)
                receiveHeader.DocumentNo = autoNumberProvider.Generate(order.BranchID, "RC", date.Month, date.Year);

            receiveHeader.PurchaseOrderID = poID;
            receiveHeader.Date = date;
            receiveHeader.GoodIssueNo = goodIssueNo;
            receiveHeader.FreightInfo = freightInfo;
            receiveHeader.WarehouseID = warehouseID;
            receiveHeader.Notes = notes;
            receiveHeader.Status = 'O';
            EntityHelper.SetAuditField(id, receiveHeader, CurrentUserName);


            context.Delete(receiveHeader.ReceivingDetails.ToList());

            bool isFullReceived = true;

            foreach (var newReceivingDetail in detail)
            {
                var totalQtyOrder = orderDetails.Where(po => po.ItemID == newReceivingDetail.ItemID).Sum(po => po.Quantity);
                var totalQtyRcv = receivingDetails.Where(rcv => rcv.ItemID == newReceivingDetail.ItemID).Sum(rcv => rcv.QtyReceived);
                
                if (totalQtyOrder - totalQtyRcv - newReceivingDetail.QtyReceived < 0)
                    throw new Exception(String.Format("Qty for item {0} exceeds Qty in Purchase Order. Item:{1}, QtyPO:{2}, QtyRcv:{3}", newReceivingDetail.ItemCode, newReceivingDetail.ItemCode, Convert.ToInt32(totalQtyOrder), totalQtyRcv + newReceivingDetail.QtyReceived));
                
                if (totalQtyOrder - totalQtyRcv - newReceivingDetail.QtyReceived > 0)
                    isFullReceived = false;

                var receiveDetail = new ReceivingDetail
                {
                    ItemID = newReceivingDetail.ItemID,
                    UnitName = newReceivingDetail.UnitName,
                    QtyReceived = newReceivingDetail.QtyReceived,
                    Notes = newReceivingDetail.Notes,
                    ReceivingHeader = receiveHeader
                };
                receiveHeader.ReceivingDetails.Add(receiveDetail);
            }

            if (id == 0)
            {
                context.Add(receiveHeader);
                autoNumberProvider.Increment("RC", order.BranchID, date.Year);
            }

            order.IsFullReceived = isFullReceived;
            order.StatusReceiving = isFullReceived ? 'F' : 'P';



            context.SaveChanges();

            return receiveHeader.DocumentNo;
        }

        public ReceivingHeader GetReceiving(int id)
        {
            return context.ReceivingHeaders.Single(po => po.ID == id);
        }

        public IEnumerable<ReceivingDetailViewModel> GetreceiveDetail(int receivingID)
        {
            var query = (from rh in context.ReceivingHeaders
                         join rd in context.ReceivingDetails on rh.ID equals rd.ReceivingID
                         where rh.ID == receivingID
                         select new ReceivingDetailViewModel
                         {
                             ID = rd.ID,
                             ItemID = rd.ItemID,
                             ItemCode = rd.Item.Barcode,
                             ItemName = rd.Item.Description,
                             QtyReceived = rd.QtyReceived,
                             PurchaseOrderID = rh.PurchaseOrderID,
                             UnitName = rd.UnitName,
                             Notes = rd.Notes
                         }).ToList();


            foreach (ReceivingDetailViewModel item in query)
            {
                item.QtyPO = context.PurchaseOrderDetails.Where(x => x.PurchaseOrderID == item.PurchaseOrderID && x.ItemID == item.ItemID).Sum(x => x.Quantity);
            }

            return query;
        }

        public string TranslateStatus(string status)
        {
            switch (status)
            {
                case "O":
                    return "Open";
                case "C":
                    return "Closed";
                case "V":
                    return "Void";
                case "F":
                    return "Full Received";
            }

            return String.Empty;
        }



        public IEnumerable<PurchaseOrderHeader> GetPurchaseOrderForReceiving()
        {
            return context.PurchaseOrderHeaders.Where(po => po.Status == 'O' && !po.IsFullReceived).ToList();
        }

        public bool CheckItemIsFullReceive(int recevingID, int poID, List<ReceivingDetailViewModel> detail)
        {

            var query = context.ReceivingHeaders
                               .Where(x => x.PurchaseOrderID == poID && x.ID != recevingID)
                               .SelectMany(x => x.ReceivingDetails);

            var receiveDetails = new List<ReceivingDetailViewModel>();
            foreach (var item in query)
                receiveDetails.Add(new ReceivingDetailViewModel { ItemID = item.ItemID, QtyReceived = item.QtyReceived });

            foreach (var lineDetail in detail)
                receiveDetails.Add(lineDetail);

            var sumReceiveUnit = detail.GroupBy(x => x.ItemID).Select(y => new { item = y.Key, Qty = receiveDetails.Where(x => x.ItemID == y.Key).Sum(x => x.QtyReceived) }).ToList();

            var sumOrderUnit = context.PurchaseOrderHeaders.Where(x => x.ID == poID).SelectMany(x => x.PurchaseOrderDetails).
                                                            GroupBy(x => x.ItemID).Select(z => new { item = z.Key, Qty = z.Sum(p => p.Quantity) });

            bool isNotValid = false;

            if (sumReceiveUnit.Count() > sumOrderUnit.Count())
            {
                isNotValid = true;
                return isNotValid;
            }

            foreach (var itemReceive in sumReceiveUnit)
            {
                var isItemExists = sumOrderUnit.Where(x => x.item == itemReceive.item);
                if (isItemExists.Count() == 1)
                {
                    var itemPO = isItemExists.SingleOrDefault();
                    if (itemPO.Qty < itemReceive.Qty)
                    {
                        isNotValid = true;
                        return isNotValid;
                    }
                }
                else
                {
                    isNotValid = true;
                    return isNotValid;
                }
            }

            return isNotValid;
        }



        public List<ReceivingDetailViewModel> GetUnReceivingOrder(int poID, int receivingID)
        {
            var totalCurrentReceiving = context.ReceivingHeaders.Where(x => x.PurchaseOrderID == poID && x.Status == 'O' && x.ID != receivingID).
                                               SelectMany(x => x.ReceivingDetails).GroupBy(x => x.ItemID).Select(y => new { item = y.Key, qty = y.Sum(p => p.QtyReceived) });

            var poItems = context.PurchaseOrderHeaders.Where(x => x.ID == poID).SelectMany(x => x.PurchaseOrderDetails).
                                                           GroupBy(x => x.ItemID).Select(z => new { item = z.Key, qty = z.Sum(p => p.Quantity) });

            var fullReceivings = new List<ReceivingDetailViewModel>();

            foreach (var orderItem in poItems)
            {
                var masterItem = context.Items.SingleOrDefault(x => x.ID == orderItem.item);
                var isItemExists = totalCurrentReceiving.Where(x => x.item == orderItem.item).ToList();

                if (isItemExists.Count() == 1)
                {
                    var itemOrder = isItemExists.SingleOrDefault();
                    if (itemOrder != null)
                    {
                        var qty = orderItem.qty - itemOrder.qty;
                        
                        fullReceivings.Add(new ReceivingDetailViewModel
                                           {
                                               ItemID = orderItem.item,
                                               ItemCode = masterItem.Barcode,
                                               ItemName = masterItem.Description,
                                               UnitName = masterItem.UnitName1,
                                               QtyPO = orderItem.qty,
                                               QtyReceived = qty,
                                               ReceivingID = receivingID
                                           });
                    }
                }
                else
                {
                    fullReceivings.Add(new ReceivingDetailViewModel
                    {
                        ItemID = orderItem.item,
                        ItemCode = masterItem.Barcode,
                        ItemName = masterItem.Description,
                        UnitName = masterItem.UnitName1,
                        QtyPO = orderItem.qty,
                        QtyReceived = orderItem.qty,
                        ReceivingID = receivingID
                    });
                }
            }

            return fullReceivings;
        }
    }

}
