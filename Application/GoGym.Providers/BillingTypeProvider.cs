using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public enum BillingTypeEnum
    {
        ManualPayment = 1,
        AutoPayment = 2
    }

    public class BillingTypeProvider : BaseProvider
    {
        public BillingTypeProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public void Add(string description, short autoPayDay, bool isActive)
        {
            BillingType bil = new BillingType();
            bil.Description = description;
            bil.AutoPayDay = autoPayDay;
            bil.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(bil, principal.Identity.Name);
            context.Add(bil);
            context.SaveChanges();
        }

        public void Update(int id, string description, short autoPayDay, bool isActive)
        {
            BillingType bil = context.BillingTypes.Single(row => row.ID == id);
            bil.Description = description;
            bil.AutoPayDay = autoPayDay;
            bil.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(bil, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] billingsID)
        {
            context.Delete(context.BillingTypes.Where(row => billingsID.Contains(row.ID)));
            context.SaveChanges();
        }

        public BillingType Get(int id)
        {
            return context.BillingTypes.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<BillingType> GetActiveBillingTypes(bool activeOnly = true)
        {
            var billingTypes = context.BillingTypes;
            if(activeOnly)
                billingTypes = billingTypes.Where(b => b.IsActive);
            return billingTypes.ToList();
        }

        public IEnumerable<BillingType> GetAll()
        {
            return context.BillingTypes.ToList();
        }
 
    }
}
