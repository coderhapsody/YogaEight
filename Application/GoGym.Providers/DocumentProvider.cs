using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.Constants;

namespace GoGym.Providers
{
    public class DocumentProvider : BaseProvider
    {
        private AutoNumberProvider autoNumberProvider;

        public DocumentProvider(FitnessEntities context, IPrincipal principal, AutoNumberProvider autoNumberProvider) : base(context, principal)
        {
            this.autoNumberProvider = autoNumberProvider;
        }

        public IEnumerable<DocumentType> GetAllDocumentTypes()
        {
            return context.DocumentTypes.ToList();
        }

        public DocumentType GetDocumentType(int id)
        {
            return context.DocumentTypes.SingleOrDefault(doc => doc.ID == id);
        }

        public ChangeStatusDocument GetChangeStatusDocument(int id)
        {
            return context.ChangeStatusDocuments.SingleOrDefault(doc => doc.ID == id);
        }

        public bool CanChangeStatus(string customerCode)
        {
            var query = from cust in context.Customers
                        join contract in context.Contracts on cust.ID equals contract.CustomerID
                        join invoice in context.InvoiceHeaders on contract.ID equals invoice.ContractID
                        where !invoice.VoidDate.HasValue
                           && cust.Barcode == customerCode
                           && invoice.InvoiceType == InvoiceConstants.FRESH_MEMBER_INVOICE
                        select cust;
            return query.Any();
        }

        public void Add(
            int branchID,
            DateTime date,
            DateTime startDate,
            DateTime? endDate,
            string customerCode,
            int documentTypeID,
            string notes)
        {
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            Employee emp = context.Employees.SingleOrDefault(e => e.UserName == principal.Identity.Name);
            if (cust != null && emp != null)
            {
                ChangeStatusDocument doc = new ChangeStatusDocument();
                doc.BranchID = branchID;
                doc.DocumentNo = autoNumberProvider.Generate(branchID, "CS", date.Month, date.Year);
                doc.Date = date;
                doc.StartDate = startDate;
                doc.EndDate = endDate;
                doc.CustomerID = cust.ID;
                doc.EmployeeID = emp.ID;
                doc.DocumentTypeID = documentTypeID;
                doc.Notes = notes;
                doc.VoidDate = null;
                EntityHelper.SetAuditFieldForInsert(doc, principal.Identity.Name);
                context.Add(doc);

                autoNumberProvider.Increment("CS", doc.BranchID, doc.Date.Year);
                context.SaveChanges();
            }
        }

        public void Update(
            int id,
            DateTime date,
            DateTime startDate,
            DateTime? endDate,
            string customerCode,
            int documentTypeID,
            string notes)
        {
            ChangeStatusDocument doc = context.ChangeStatusDocuments.Single(d => d.ID == id);
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            Employee emp = context.Employees.SingleOrDefault(e => e.UserName == principal.Identity.Name);
            if (cust != null && doc != null && emp != null)
            {
                doc.Date = date;
                doc.StartDate = startDate;
                doc.EndDate = endDate;
                doc.CustomerID = cust.ID;
                doc.EmployeeID = emp.ID;
                doc.DocumentTypeID = documentTypeID;
                doc.Notes = notes;
                doc.VoidDate = null;
                EntityHelper.SetAuditFieldForUpdate(doc, principal.Identity.Name);



                context.SaveChanges();
            }
        }

        public void Approve(int id)
        {
            ChangeStatusDocument doc = context.ChangeStatusDocuments.Single(d => d.ID == id);
            Employee emp = context.Employees.SingleOrDefault(e => e.ID == doc.EmployeeID);
            if (emp != null && doc != null)
            {
                doc.ApprovedDate = DateTime.Now;
                doc.ApprovedByEmployeeID = emp.ID;
                EntityHelper.SetAuditFieldForUpdate(doc, principal.Identity.Name);

                CustomerStatusHistory csh = new CustomerStatusHistory();
                csh.StartDate = doc.StartDate;
                csh.EndDate = doc.EndDate;
                csh.Notes = doc.Notes;
                csh.CustomerID = doc.CustomerID;
                csh.Date = DateTime.Today;
                csh.ChangeStatusDocument = doc;
                EntityHelper.SetAuditFieldForInsert(csh, principal.Identity.Name);

                if (doc.DocumentType.ChangeCustomerStatusIDTo.HasValue)
                    csh.CustomerStatusID = doc.DocumentType.ChangeCustomerStatusIDTo.Value;

                //if (doc.DocumentType.IsLastState)
                //{
                //    foreach (Contract activeContract in doc.Customer.Contracts.Where(con => con.Status == ContractStatus.PAID))
                //        activeContract.Status = ContractStatus.CLOSED;
                //}

                context.Add(csh);

                context.SaveChanges();
            }
        }

        public void Void(int id)
        {
            var doc = context.ChangeStatusDocuments.Single(d => d.ID == id);
            doc.VoidDate = DateTime.Now;
            EntityHelper.SetAuditFieldForUpdate(doc, principal.Identity.Name);
            context.SaveChanges();
        }
    }
}
