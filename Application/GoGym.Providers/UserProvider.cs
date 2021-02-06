using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using Telerik.OpenAccess;

namespace GoGym.Providers
{
    public class UserProvider : BaseProvider
    {
        public UserProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public IEnumerable<Employee> GetUsers()
        {
            var query = context.Employees.Where(emp => emp.IsActive).ToList();
            return query;
        }

        //public IEnumerable<Branch> GetCurrentUserBranchID(string userName)
        //{
        //    IQueryable<Branch> query = context.UserAtBranches.Where(user => user.UserName == userName).Select(row => row.Branch).Concat(context.Branches).Distinct();
        //    return query.ToList();
        //}

        //public IEnumerable<Branch> GetCurrentActiveBranches(string userName)
        //{
        //    IQueryable<Branch> query = context.UserAtBranches.Where(user => user.UserName == userName).Select(row => row.Branch);
        //    return query.ToList();
        //}

        //public IEnumerable<Branch> GetSelectedBranches(string userName)
        //{
        //    return context.UserAtBranches.Where(user => user.UserName == userName).Select(row => row.Branch).ToList();
        //}

        public void UpdateUserAtBranch(string userName, IEnumerable<int> branchesID)
        {
            var query = context.UserAtBranches.Where(row => row.UserName == userName).ToList();            
            context.Delete(query);
            context.SaveChanges();

            foreach (int id in branchesID)
                context.Add(
                    new UserAtBranch()
                    {
                        BranchID = id,
                        UserName = userName
                    });

            context.SaveChanges();

            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == userName);
            if(!context.UserAtBranches.Any(user => user.UserName == userName && user.BranchID == employee.Branch.ID))
            {
                context.Add(new UserAtBranch
                {
                    BranchID = employee.HomeBranchID,
                    UserName = userName
                });

                context.SaveChanges();
            }
        }

        public void AddSuccessLogInHistory(string userName)
        {
            LogInHistory log = new LogInHistory();
            log.UserName = userName;
            log.LogInWhen = DateTime.Now;
            log.CanLogIn = true;
            context.Add(log);
            context.SaveChanges();
        }

        public void AddFailedLogInHistory(string userName)
        {
            LogInHistory log = new LogInHistory();
            log.UserName = userName;
            log.LogInWhen = DateTime.Now;
            log.CanLogIn = false;
            context.Add(log);
            context.SaveChanges();
        }
    }
}
