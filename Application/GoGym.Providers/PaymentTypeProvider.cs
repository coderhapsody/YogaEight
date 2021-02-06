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
    public class PaymentTypeProvider : BaseProvider
    {
        public PaymentTypeProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void Add(
                    string description,
                    bool isActive)
        {
            PaymentType pay = new PaymentType();
            pay.Description = description;
            pay.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(pay, principal.Identity.Name);
            context.Add(pay);
            context.SaveChanges();
        }

        public void Update(
            int id,
            string description,
            bool isActive)
        {
            PaymentType pay = context.PaymentTypes.Single(row => row.ID == id);
            pay.Description = description;
            pay.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(pay, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] paymentTypesID)
        {
            context.Delete(context.PaymentTypes.Where(row => paymentTypesID.Contains(row.ID)));
            context.SaveChanges();
        }

        public string GetDescription(int paymentTypeID)
        {
            return context.PaymentTypes.Any(row => row.ID == paymentTypeID) ?
                context.PaymentTypes.Single(row => row.ID == paymentTypeID).Description : String.Empty;
        }

        public PaymentType Get(int paymentTypeID)
        {
            return context.PaymentTypes.SingleOrDefault(row => row.ID == paymentTypeID);
        }

        public IEnumerable<PaymentType> GetAll()
        {
            return context.PaymentTypes.Where(row => row.IsActive).ToList();
        }
    }
}
