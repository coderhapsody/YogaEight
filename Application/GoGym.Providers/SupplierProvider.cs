using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class SupplierProvider : BaseProvider
    {
        public SupplierProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdateSupplier(int id,
                                        string name,
                                        string address,
                                        string npwp,
                                        string email,
                                        string phone1,
                                        string phone2,
                                        bool taxable)
        {
            var supp = id == 0 ? new Supplier() : context.Suppliers.Single(supplier => supplier.ID == id);
            supp.Name = name;
            supp.Address = address;
            supp.NPWP = npwp;
            supp.Email = email;
            supp.Phone1 = phone1;
            supp.Phone2 = phone2;
            supp.Taxable = taxable;
            EntityHelper.SetAuditField(id, supp, CurrentUserName);

            if (id == 0)
                context.Add(supp);            
            context.SaveChanges();
        }

        public Supplier GetSupplier(int id)
        {
            return context.Suppliers.SingleOrDefault(supp => supp.ID == id);
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            return context.Suppliers.ToList();
        }

        public IEnumerable<Supplier> GetSuppliers(string name)
        {
            return context.Suppliers.Where(supp => supp.Name.Contains(name)).ToList();
        }


        public bool CanDeleteSupplier(int[] arrayOfID)
        {
            return true;
        }

        public void DeleteSupplier(int[] arrayOfID)
        {
            context.Delete(context.Suppliers.Where(supp => arrayOfID.Contains(supp.ID)).ToList());
            context.SaveChanges();
        }
    }
}
