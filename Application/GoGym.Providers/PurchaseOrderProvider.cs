using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;
using Telerik.OpenAccess;

namespace GoGym.Providers
{
    public class PurchaseOrderProvider : BaseProvider
    {
        private AutoNumberProvider autoNumberProvider;
        private EmployeeProvider employeeProvider;
        private ItemProvider itemProvider;
        public PurchaseOrderProvider(FitnessEntities context, IPrincipal principal, AutoNumberProvider autoNumberProvider, EmployeeProvider employeeProvider, ItemProvider itemProvider) : base(context, principal)
        {
            this.autoNumberProvider = autoNumberProvider;
            this.employeeProvider = employeeProvider;
            this.itemProvider = itemProvider;
        }

        public void NotApprovedPurchaseOrder(string documentNo, string reason)
        {
            var poHeader = context.PurchaseOrderHeaders.SingleOrDefault(po => po.DocumentNo == documentNo);

            if (poHeader == null) return;

            poHeader.Status = 'N'; //Not Approve
            EntityHelper.SetAuditFieldForUpdate(poHeader, CurrentUserName);
            //poHeader.VoidDate = DateTime.Today;
            //poHeader.VoidReason = reason;

            context.SaveChanges();
        }

        public void VoidPurchaseOrder(string documentNo, string reason)
        {
            var poHeader = context.PurchaseOrderHeaders.SingleOrDefault(po => po.DocumentNo == documentNo);

            if (poHeader == null) return;

            poHeader.Status = 'V';
            poHeader.VoidWhen = DateTime.Today;
            poHeader.VoidReason = reason;

            context.SaveChanges();
        }

        public void ApprovedPurchaseOrder(string documentNo, int employeeID, string approveReason)
        {
            var poHeader = context.PurchaseOrderHeaders.SingleOrDefault(po => po.DocumentNo == documentNo);

            if (poHeader == null) return;

            poHeader.Status = 'A';
            poHeader.ApprovedByEmployeeID = employeeID;
            poHeader.ApprovedWhen = DateTime.Today;
            poHeader.ApprovedReason = approveReason;

            context.SaveChanges();
        }

        public string AddOrUpdatePurchaseOrder(int id, int branchID, DateTime date, DateTime expectedDate, int supplierID, string notes,
            decimal discValue, string terms,
            List<PurchaseOrderDetailViewModel> detail)
        {
            var poHeader = id == 0
                ? new PurchaseOrderHeader()
                : context.PurchaseOrderHeaders.Single(po => po.ID == id);

            if (id == 0)
                poHeader.DocumentNo = autoNumberProvider.Generate(branchID, "PO", date.Month, date.Year);

            poHeader.BranchID = branchID;
            poHeader.DocumentDate = date;
            poHeader.ExpectedDate = expectedDate;
            poHeader.SupplierID = supplierID;
            poHeader.Notes = notes;
            poHeader.DiscountValue = discValue;
            poHeader.Terms = terms;
            poHeader.EmployeeID = employeeProvider.Get(CurrentUserName).ID;
            poHeader.Status = 'O';
            poHeader.StatusReceiving = 'O';
            EntityHelper.SetAuditField(id, poHeader, CurrentUserName);

            context.Delete(poHeader.PurchaseOrderDetails.ToList());            
            
            foreach (var detailLine in detail)
            {
                PurchaseOrderDetailViewModel line = detailLine;
                //var itemProvider = new ItemProvider(context, principal);
                decimal ratio = itemProvider.GetItemUnitRatio(detailLine.ItemID, detailLine.UnitName);

                var poDetail = new PurchaseOrderDetail
                {
                    ItemID = detailLine.ItemID,
                    UnitPrice = detailLine.UnitPrice,
                    Quantity = detailLine.Quantity,
                    IsTaxed = detailLine.IsTaxed,
                    DiscountRate = detailLine.DiscountRate,
                    UnitName = detailLine.UnitName,
                    UnitRatio = ratio,
                    Notes = String.Empty
                };

                poHeader.PurchaseOrderDetails.Add(poDetail);
            }

            if (id == 0)
            {
                context.Add(poHeader);
                autoNumberProvider.Increment("PO", branchID, date.Year);
            }
            

            context.SaveChanges();

            return poHeader.DocumentNo;
        }

        public PurchaseOrderHeader GetPurchaseOrder(int id)
        {
            return context.PurchaseOrderHeaders.Single(po => po.ID == id);
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
            }

            return String.Empty;
        }

        public IEnumerable<PurchaseOrderDetailViewModel> GetPurchaseOrderDetail(string documentNo)
        {
            var purchaseOrder = context.PurchaseOrderHeaders.SingleOrDefault(po => po.DocumentNo == documentNo);
            return GetPurchaseOrderDetail(purchaseOrder.ID);
        }

        public IEnumerable<PurchaseOrderDetailViewModel> GetPurchaseOrderDetail(int id)
        {
            var query = from poh in context.PurchaseOrderHeaders
                        join pod in context.PurchaseOrderDetails.Include(po => po.Item) on poh.ID equals pod.PurchaseOrderID
                        where poh.ID == id
                        select new PurchaseOrderDetailViewModel
                        {
                            ID = pod.ID,
                            DiscountRate = pod.DiscountRate,
                            DiscountValue = pod.DiscountValue,
                            IsTaxed = pod.IsTaxed,
                            ItemCode = pod.Item.Barcode,
                            ItemName = pod.Item.Description,
                            UnitName = pod.UnitName,
                            ItemID = pod.ItemID,
                            Quantity = pod.Quantity,
                            UnitPrice = pod.UnitPrice,
                            Total = ((pod.Quantity * pod.UnitPrice) -
                                     (pod.Quantity * pod.UnitPrice * pod.DiscountRate / 100)) * (pod.IsTaxed ? 1.1M : 1M)
                        };
            return query.ToList();
        }

        public decimal GetTotalDetail(IEnumerable<PurchaseOrderDetail> detail)
        {
            return detail.Sum(
                    d =>
                        (d.Quantity * d.UnitPrice - (d.Quantity * d.UnitPrice * d.DiscountRate / 100)) *
                        (d.IsTaxed ? 1.1M : 1M));
        }

        public bool IsReceive(int id)
        {
            return context.ReceivingHeaders.Any(x => x.PurchaseOrderID == id && (x.Status == 'O' || x.Status == 'F'));
        }

        #region Terms

        public IEnumerable<Term> GetTerms()
        {
            return context.Terms.Where(term => term.IsActive).ToList();
        }

        public Term GetTerm(int id)
        {
            var term = context.Terms.SingleOrDefault(r => r.ID == id);
            return term;
        }

        public Term GetTerm(string name)
        {
            return context.Terms.SingleOrDefault(term => term.Name == name);
        }

        public void DeleteTerms(int[] arrayOfID)
        {
            context.Delete(context.Terms.Where(id => arrayOfID.Contains(id.ID)).ToList());
            context.SaveChanges();
        }

        public void AddOrUpdateTerm(int id, string name, bool isActive)
        {
            var term = id == 0 ? new Term() : context.Terms.SingleOrDefault(r => r.ID == id);
            if (term != null)
            {
                term.Name = name;
                term.IsActive = isActive;
                EntityHelper.SetAuditField(id, term, CurrentUserName);
            }

            if (id == 0)
                context.Add(term);

            context.SaveChanges();
        }

        public bool CanDeleteTerms(int[] arrayOfID)
        {
            // TODO: add logic to validate whether a role can be safely deleted or not
            return true;
        }

        #endregion


    }
}
