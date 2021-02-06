using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class ItemAccountProvider : BaseProvider
    {
        public ItemAccountProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void DisableAccountCascade(string accountNo, bool isActive)
        {
            ItemAccount account = Get(accountNo);
            account.IsActive = isActive;
            List<ItemAccount> accounts = GetChildAccount(accountNo).ToList();
            if (!accounts.Any())
                return;
            foreach (var item in accounts)
                DisableAccountCascade(item.AccountNo, isActive);
            context.SaveChanges();
        }

        public void Add(string accountNo, string description, int? parentID, bool isActive)
        {
            ItemAccount obj = new ItemAccount();
            obj.AccountNo = accountNo;
            obj.Description = description;
            obj.ParentID = parentID;
            obj.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
            context.Add(obj);
            context.SaveChanges();
        }

        public void Update(int id, string accountNo, string description, int? parentID, bool isActive)
        {
            ItemAccount obj = context.ItemAccounts.Single(row => row.ID == id);
            obj.AccountNo = accountNo;
            obj.Description = description;
            obj.ParentID = parentID;
            obj.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.ItemAccounts.Where(row => id.Contains(row.ID)));
            context.SaveChanges();
        }

        public ItemAccount Get(int id)
        {
            return context.ItemAccounts.SingleOrDefault(row => row.ID == id);
        }

        public ItemAccount Get(string accountNo)
        {
            return context.ItemAccounts.SingleOrDefault(row => row.AccountNo == accountNo);
        }

        public IEnumerable<ItemAccount> GetRootAccount()
        {
            return context.ItemAccounts.Where(row => !row.ParentID.HasValue).ToList();
        }

        public IEnumerable<ItemAccount> GetAll()
        {
            return context.ItemAccounts.ToList();
        }

        public IEnumerable<ItemAccount> GetChildAccount(int parentID)
        {
            return context.ItemAccounts.Where(row => row.ParentID == parentID).ToList();
        }

        public IEnumerable<ItemAccount> GetChildAccount(string accountNo)
        {
            return
                context.ItemAccounts.Where(row => row.ItemAccount1 != null && row.ItemAccount1.AccountNo == accountNo)
                       .ToList();
        }

        public IEnumerable<ItemAccount> GetValuedAccounts()
        {
            foreach (var account in context.ItemAccounts)
            {
                if (context.ItemAccounts.Count(row => row.ParentID.Value == account.ID) == 0)
                    yield return account;
            }
        }

        public Stack<ItemAccount> GetAccountHierarchy(string accountNo)
        {
            Stack<ItemAccount> stack = new Stack<ItemAccount>();
            return _GetAccountHierarchy(stack, accountNo);
        }

        private Stack<ItemAccount> _GetAccountHierarchy(Stack<ItemAccount> stack, string accountNo)
        {
            ItemAccount account = context.ItemAccounts.Single(row => row.AccountNo == accountNo);
            if (account.ParentID.HasValue)
            {
                stack.Push(account);
                _GetAccountHierarchy(stack, Get(account.ParentID.Value).AccountNo);
            }
            else
                stack.Push(account);
            return stack;
        }
    }
}
