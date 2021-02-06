using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class CreditCardTypeProvider : BaseProvider
    {
        public CreditCardTypeProvider(FitnessEntities context, IPrincipal principal) : base (context, principal)
        {            
        }

        public void Add(string description)
        {
            var obj = new CreditCardType();
            obj.Description = description;
            EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
            context.Add(obj);
            context.SaveChanges();
        }


        public void Update(int id, string description)
        {
            CreditCardType obj = context.CreditCardTypes.Single(row => row.ID == id);
            obj.Description = description;
            EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.CreditCardTypes.Where(row => id.Contains(row.ID)));
            context.SaveChanges();
        }

        public CreditCardType Get(int id)
        {
            return context.CreditCardTypes.SingleOrDefault(row => row.ID == id);
        }


        public IEnumerable<CreditCardType> GetAll()
        {
            return context.CreditCardTypes.ToList();
        }
    }
}
