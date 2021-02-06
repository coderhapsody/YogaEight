using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class InventoryMutationProvider : BaseProvider
    {
        public InventoryMutationProvider(FitnessEntities context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public void AddOrUpdateInventoryMutation(
            InventoryMutationHeader header,
            IEnumerable<InventoryMutationDetailViewModel> detail)
        {
            context.Delete(header.InventoryMutationDetails);

            foreach(var inventoryMutationDetail in detail)
            {
                header.InventoryMutationDetails.Add(new InventoryMutationDetail
                {
                    InventoryMutationHeader = header,
                    ItemID = inventoryMutationDetail.ItemID,
                    Notes = inventoryMutationDetail.Notes,
                    Qty = inventoryMutationDetail.Qty,
                    UnitName = inventoryMutationDetail.UnitName,
                });  
            }

            if(header.ID == 0)
            {
                context.Add(header);                
            }

            EntityHelper.SetAuditField(header.ID, header, principal.Identity.Name);

            context.SaveChanges();
        }

        public bool IsVoid(int inventoryMutationHeaderId)
        {
            var invMutation = GetInventoryMutation(inventoryMutationHeaderId);
            if(invMutation != null)
            {
                return invMutation.VoidDate.HasValue;
            }

            return false;
        }

        public bool IsPosted(int inventoryMutationHeaderId)
        {
            var invMutation = GetInventoryMutation(inventoryMutationHeaderId);
            if (invMutation != null)
            {
                return invMutation.PostedWhen.HasValue;
            }

            return false;
        }

        public void VoidInventoryMutation(int inventoryMutationHeaderId, string reason)
        {
            var header = context.InventoryMutationHeaders.SingleOrDefault(inv => inv.ID == inventoryMutationHeaderId);
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == principal.Identity.Name);
            if(header != null && employee != null)
            {
                //foreach(var inventoryMutationDetail in header.InventoryMutationDetails)
                //{
                //    var stockHistory = new StockHistory();
                //    stockHistory.ItemID = inventoryMutationDetail.ItemID;
                //    stockHistory.RefNo = header.DocumentNo;
                //    stockHistory.RefType = "IM";
                //    stockHistory.DetailID = inventoryMutationDetail.ID;
                //    stockHistory.Qty = inventoryMutationDetail.Qty*(header.InOut == "I" ? -1 : 1);
                //    stockHistory.Date = header.ChangedWhen.Date;
                //    stockHistory.PostDate = DateTime.Today;
                //    context.StockHistories.Add(stockHistory);

                //    if (header.FromBranchID.HasValue)
                //    {
                //        var stockBranchFrom = context.Stocks.SingleOrDefault(
                //            stock => stock.BranchID == header.FromBranchID &&
                //                     stock.ItemID == inventoryMutationDetail.ItemID);
                //        if (stockBranchFrom == null)
                //        {
                //            stockBranchFrom = new Stock();
                //            context.Stocks.Add(stockBranchFrom);
                //        }
                //        stockBranchFrom.QtyOnHand += inventoryMutationDetail.Qty;
                //        stockBranchFrom.LastPostWhen = DateTime.Now;
                //    }

                //    if (header.ToBranchID.HasValue)
                //    {
                //        var stockBranchTo = context.Stocks.SingleOrDefault(
                //            stock => stock.BranchID == header.ToBranchID &&
                //                     stock.ItemID == inventoryMutationDetail.ItemID);
                //        if (stockBranchTo == null)
                //        {
                //            stockBranchTo = new Stock();
                //            context.Stocks.Add(stockBranchTo);
                //        }
                //        stockBranchTo.QtyOnHand -= inventoryMutationDetail.Qty;
                //        stockBranchTo.LastPostWhen = DateTime.Now;
                //    }
                //}

                header.VoidDate = DateTime.Now;
                header.VoidByEmployeeID = employee.ID;
                header.VoidReason = reason;
            }
            context.SaveChanges();
        }

        public void PostInventoryMutation(int inventoryMutationHeaderId)
        {
            var header = context.InventoryMutationHeaders.SingleOrDefault(inv => inv.ID == inventoryMutationHeaderId);
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == principal.Identity.Name);
            if(header != null && employee != null)
            {
                foreach(var inventoryMutationDetail in header.InventoryMutationDetails)
                {
                    var stockHistory = new StockHistory();
                    stockHistory.ItemID = inventoryMutationDetail.ItemID;
                    stockHistory.RefNo = header.DocumentNo;
                    stockHistory.RefType = "IM";
                    stockHistory.DetailID = inventoryMutationDetail.ID;
                    stockHistory.Qty = inventoryMutationDetail.Qty*(header.InOut == 'I' ? 1 : -1);
                    stockHistory.Date = header.ChangedWhen.Date;
                    stockHistory.PostDate = DateTime.Today;
                    context.Add(stockHistory);

                    if(header.FromBranchID.HasValue)
                    {
                        var stockBranchFrom = context.Stocks.SingleOrDefault(
                            stock => stock.BranchID == header.FromBranchID &&
                                     stock.ItemID == inventoryMutationDetail.ItemID);
                        if(stockBranchFrom == null)
                        {
                            stockBranchFrom = new Stock();
                            context.Add(stockBranchFrom);
                        }
                        stockBranchFrom.QtyOnHand -= inventoryMutationDetail.Qty;
                        stockBranchFrom.LastPostWhen = DateTime.Now;
                    }

                    if(header.ToBranchID.HasValue)
                    {
                        var stockBranchTo = context.Stocks.SingleOrDefault(
                            stock => stock.BranchID == header.ToBranchID &&
                                     stock.ItemID == inventoryMutationDetail.ItemID);
                        if(stockBranchTo == null)
                        {
                            stockBranchTo = new Stock();
                            context.Add(stockBranchTo);
                        }
                        stockBranchTo.QtyOnHand += inventoryMutationDetail.Qty;
                        stockBranchTo.LastPostWhen = DateTime.Now;
                    }
                    
                }

                header.PostedWhen = DateTime.Now;
                header.PostedByEmployeeID = employee.ID;
            }            
            context.SaveChanges();
        }

        public InventoryMutationHeader GetInventoryMutation(int inventoryMutationId)
        {
            return context.InventoryMutationHeaders.SingleOrDefault(inv => inv.ID == inventoryMutationId);
        }        
    }
}
