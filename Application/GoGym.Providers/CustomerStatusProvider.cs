using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class CustomerStatusProvider : BaseProvider
    {
        public CustomerStatusProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public void Add(string description, string color, string backgroundColor)
        {            
            var obj = new CustomerStatus();
            obj.Description = description;
            obj.Color = color + "|" + backgroundColor;
            EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
            context.Add(obj);
            context.SaveChanges();
        }

        public void Update(int id, string description, string color, string backgroundColor)
        {
            var obj = context.CustomerStatus.Single(row => row.ID == id);
            obj.Description = description;
            obj.Color = color + "|" + backgroundColor;
            EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.CustomerStatus.Where(row => id.Contains(row.ID)));
            context.SaveChanges();
        }

        public CustomerStatus Get(int id)
        {
            return context.CustomerStatus.SingleOrDefault(row => row.ID == id);
        }

        public string GetStatusColor(string status)
        {
            try
            {
                var customerStatus = context.CustomerStatus.SingleOrDefault(row => row.Description == status);                
                if (customerStatus != null)
                    return customerStatus.Color;                
            }
            catch
            { }

            return "#ff0000|#ffffff";
        }

        public IEnumerable<CustomerStatus> GetAll()
        {
            return context.CustomerStatus.ToList();
        }

        public IEnumerable<CustomerStatusHistory> GetStatusHistory(string customerBarcode)
        {
            return context.CustomerStatusHistories.Where(cust => cust.Customer.Barcode == customerBarcode).ToList();
        }

        public CustomerStatusHistory GetLatestStatus(string customerBarcode)
        {
            var customer = context.Customers.FirstOrDefault(cust => cust.Barcode == customerBarcode);

            return context.CustomerStatusHistories.Where(csh => csh.CustomerID == customer.ID
                                                                && !csh.ChangeStatusDocument.VoidDate.HasValue).ToList()
                          .Where(
                              cust =>
                                  cust.StartDate <= DateTime.Today &&
                                  cust.EndDate.GetValueOrDefault(new DateTime(2099, 12, 31)) >= DateTime.Today)
                          .OrderByDescending(cs => cs.StartDate).ThenByDescending(cs => cs.ChangedWhen)                          
                          .FirstOrDefault();

        }

        public void AddStatusHistory(int customerStatusID, string customerCode, DateTime startDate)
        {
            AddStatusHistory(customerStatusID, customerCode, startDate, (DateTime?)null);
        }

        public void AddStatusHistory(int customerStatusID, string customerCode, DateTime startDate, DateTime? endDate)
        {
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            var custStatus = context.CustomerStatus.SingleOrDefault(c => c.ID == customerStatusID);
            if (cust != null && custStatus != null)
                AddStatusHistory(custStatus, cust, startDate, endDate);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, string customerCode, DateTime startDate)
        {
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            if (cust != null)
                AddStatusHistory(customerStatus, cust, startDate, (DateTime?)null);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, string customerCode, DateTime startDate, DateTime? endDate)
        {
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            if (cust != null)
                AddStatusHistory(customerStatus, cust, startDate, endDate);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, Customer customer, DateTime startDate)
        {
            AddStatusHistory(customerStatus, customer, startDate, (DateTime?)null);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, Customer customer, DateTime startDate, DateTime? endDate)
        {
            var csh = new CustomerStatusHistory();
            csh.CustomerStatusID = customerStatus.ID;
            csh.CustomerID = customer.ID;
            csh.StartDate = startDate;
            csh.EndDate = endDate;
            EntityHelper.SetAuditFieldForInsert(csh, principal.Identity.Name);
            context.Add(csh);
            context.SaveChanges();
        }



        public string GetStatusBackgroundColor(string p)
        {
            throw new NotImplementedException();
        }
    }
}
