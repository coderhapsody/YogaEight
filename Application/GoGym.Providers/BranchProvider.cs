using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class BranchProvider : BaseProvider
    {
        public BranchProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public string GetBranchName(int branchID)
        {
            string result = String.Empty;
            Branch branch = context.Branches.SingleOrDefault(br => br.ID == branchID);
            result = branch == null ? String.Empty : branch.Name;
            return result;
        }

        //public IEnumerable<Branch> GetAll()
        //{
        //    return context.Branches.ToList();
        //}

        public IEnumerable<Branch> GetActiveBranches()
        {
            return context.Branches.Where(br => br.IsActive).ToList();
        }

        public IEnumerable<Branch> GetActiveBranches(string userName)
        {
            var query = context.Employees.FirstOrDefault(emp => emp.UserName == userName);
            if(query != null)
            {
                yield return query.Branch;
            }
            //var query = from branch in context.Branches
            //            join userAtBranch in context.UserAtBranches on branch equals userAtBranch.Branch
            //            where userAtBranch.UserName == userName
            //            select branch;
            //return query.ToList();
        }

        public IEnumerable<Branch> GetAllowedBranches(string userName)
        {
            var query = from branch in context.Branches
                        join userAtBranch in context.UserAtBranches on branch equals userAtBranch.Branch
                        where userAtBranch.UserName == userName && branch.IsActive
                        select branch;
            return query.ToList();
        } 

        public IEnumerable<string> GetBranchName(IEnumerable<int> branchesID)
        {
            return context.Branches.Where(row => branchesID.Contains(row.ID)).Select(row => row.Name);
        }

        public void Delete(int[] branchesID)
        {
            foreach(var branchID in branchesID)
            {
                var branch = context.Branches.SingleOrDefault(br => br.ID == branchID);
                if(branch != null)
                {
                    if(context.InvoiceHeaders.Any(inv => inv.BranchID == branchID))
                        throw new Exception(String.Format("Branch {0} has been used in invoice.", branch.Name));

                    if (context.Contracts.Any(con => con.BranchID == branchID))
                        throw new Exception(String.Format("Branch {0} has been used in contracts.", branch.Name));

                    if (context.Customers.Any(cust => cust.HomeBranchID == branchID))
                        throw new Exception(String.Format("Branch {0} has been used in customers.", branch.Name));

                    if (context.UserAtBranches.Any(userAtBranch => userAtBranch.BranchID == branchID))
                        throw new Exception(String.Format("Branch {0} has been used in user branches.", branch.Name));
                }
            }
            context.Delete(context.Branches.Where(row => branchesID.Contains(row.ID)));
            context.SaveChanges();
        }

        public void Add(
            string code,
            string name,
            string address,
            string phone,
            string email,
            string merchantCode,
            bool isActive)
        {
            Branch br = new Branch();
            br.Code = code;
            br.Name = name;
            br.Address = address;
            br.Phone = phone;
            br.Email = email;
            br.MerchantCode = merchantCode;
            br.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(br, principal.Identity.Name);
            context.Add(br);
            context.SaveChanges();
        }

        public void Update(
            int id,
            string code,
            string name,
            string address,
            string phone,
            string email,
            string merchantCode,
            bool isActive)
        {
            Branch br = context.Branches.SingleOrDefault(row => row.ID == id);
            br.Code = code;
            br.Name = name;
            br.Address = address;
            br.Phone = phone;
            br.Email = email;
            br.MerchantCode = merchantCode;
            br.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(br, principal.Identity.Name);
            context.SaveChanges();
        }

        public Branch Get(int branchID)
        {
            return context.Branches.SingleOrDefault(row => row.ID == branchID);
        }
    }
}
