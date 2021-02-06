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
    public class BankProvider : BaseProvider
    {
        public BankProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public void Add(
            string name,
            bool isActive)
        {
            var bank = new Bank();
            bank.Name = name;
            bank.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(bank, principal.Identity.Name);
            context.Add(bank);
            context.SaveChanges();
        }


        public void Update(
            int id,
            string name,
            bool isActive)
        {
            var bank = context.Banks.Single(row => row.ID == id);
            bank.Name = name;
            bank.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(bank, principal.Identity.Name);
            context.SaveChanges();
        }


        public void Delete(int[] banksID)
        {
            context.Delete(context.Banks.Where(row => banksID.Contains(row.ID)));
            context.SaveChanges();
        }

        public string GetBankName(int id)
        {
            return context.Banks.Any(row => row.ID == id) ?
                context.Banks.Single(row => row.ID == id).Name : String.Empty;
        }

        public Bank Get(int id)
        {
            return context.Banks.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Bank> GetActiveBanks(bool activeOnly = true)
        {
            var query = context.Banks;
            if(activeOnly)
                query = query.Where(bank => bank.IsActive);

            return query.ToList();
        }
    }
}
