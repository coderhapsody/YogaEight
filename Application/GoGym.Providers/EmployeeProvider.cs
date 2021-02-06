using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class EmployeeProvider : BaseProvider
    {
        private SecurityProvider securityProvider;
        public EmployeeProvider(FitnessEntities context, IPrincipal principal, SecurityProvider securityProvider)
            : base(context, principal)
        {
            this.securityProvider = securityProvider;
        }

        public bool CanApproveDocument(string userName)
        {
            return context.Employees.Single(emp => emp.UserName == userName).CanApproveDocument;
        }

        public bool CanReprintInvoice(string userName)
        {
            return context.Employees.Single(emp => emp.UserName == userName).CanReprint;
        }

        public void Add(string userName, string barcode, string password, string firstName, int homeBranchID, int roleID, string email, bool isActive, bool canApproveDocument)
        {
            Employee emp = new Employee();
            emp.UserName = userName;
            emp.Barcode = barcode;
            emp.FirstName = firstName;
            emp.HomeBranchID = homeBranchID;
            emp.RoleID = roleID;
            emp.Email = email;
            emp.IsActive = isActive;
            emp.CanApproveDocument = canApproveDocument;
            EntityHelper.SetAuditFieldForInsert(emp, principal.Identity.Name);
            context.Add(emp);

            var userAtBranch = new UserAtBranch() { BranchID = homeBranchID, UserName = userName };
            context.Add(userAtBranch);

            context.SaveChanges();

            securityProvider.ChangePassword(userName, password);
        }

        public void Update(string userName, string barcode, string firstName, int homeBranchID, int roleID, string email, bool isActive)
        {
            Employee emp = context.Employees.SingleOrDefault(user => user.UserName == userName);
            if (emp != null)
            {
                emp.UserName = userName;
                emp.Barcode = barcode;
                emp.FirstName = firstName;
                emp.HomeBranchID = homeBranchID;
                emp.Email = email;
                emp.RoleID = roleID;
                emp.IsActive = isActive;
                EntityHelper.SetAuditFieldForUpdate(emp, principal.Identity.Name);

                if (!context.UserAtBranches.Any(user => user.UserName == userName && user.BranchID == homeBranchID))
                {
                    var userAtBranch = new UserAtBranch() {BranchID = homeBranchID, UserName = userName};
                    context.Add(userAtBranch);
                }

                context.SaveChanges();
            }
        }

        public void Update(int id, string barcode, string userName, int homeBranchID, string firstName, string lastName, string phone, string email, bool deletePhoto, string photoFileName, bool isActive, bool canApproveDocument, bool canEditActiveContract, bool canReprint)
        {
            Employee emp = context.Employees.Single(row => row.ID == id);
            emp.Barcode = barcode;
            emp.UserName = userName;
            emp.FirstName = firstName;
            emp.LastName = lastName;
            emp.HomeBranchID = homeBranchID;
            emp.Photo = deletePhoto ? null : photoFileName;
            emp.Phone = phone;
            emp.Email = email;
            emp.IsActive = isActive;
            emp.CanApproveDocument = canApproveDocument;
            emp.CanEditActiveContract = canEditActiveContract;
            emp.CanReprint = canReprint;
            EntityHelper.SetAuditFieldForUpdate(emp, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(string userName)
        {
            Employee emp = context.Employees.SingleOrDefault(row => row.UserName == userName);
            if (emp != null)
            {
                context.Delete(emp);

                var userAtBranches = context.UserAtBranches.Where(user => user.UserName == userName).ToList();
                context.Delete(userAtBranches);

                context.SaveChanges();
            }
        }

        public Employee Get(string userName)
        {
            return context.Employees.SingleOrDefault(row => row.UserName == userName);
        }

        public Employee Get(int employeeID)
        {
            return context.Employees.SingleOrDefault(row => row.ID == employeeID);
        }

        public string GetName(string userName)
        {
            Employee emp = Get(userName);
            return emp == null ? String.Empty : String.Format("{0} {1}", emp.FirstName, emp.LastName);
        }

        public IEnumerable<Employee> GetSales(int branchID)
        {
            int salesRoleID =
                Convert.ToInt32(ConfigurationSingletonProvider.Instance[ConfigurationKeys.Roles.SalesRoleID]);

            var query = from emp in context.Employees
                        join role in context.Roles on emp.RoleID equals role.ID                        
                        where role.ID == salesRoleID && emp.IsActive && emp.HomeBranchID == branchID
                        select emp;
            return query.ToList();
        }

        public IEnumerable<Employee> GetTrainers(int branchID)
        {
            int trainerRoleID =
                Convert.ToInt32(ConfigurationSingletonProvider.Instance[ConfigurationKeys.Roles.TrainerRoleID]);

            var query = from emp in context.Employees
                        join role in context.Roles on emp.RoleID equals role.ID
                        where role.ID == trainerRoleID && emp.IsActive && emp.HomeBranchID == branchID
                        select emp;

            return query.ToList();
        }

        public IEnumerable<Employee> GetAll(int branchID)
        {
            var query = from emp in context.Employees                        
                        where emp.IsActive && emp.HomeBranchID == branchID
                        select emp;
            return query.ToList();
        }

        public void AddSalesPoint(int contractID, int employeeID, decimal pointAmount, string notes)
        {
            var point = new SalesPoint();
            point.ContractID = contractID;
            point.EmployeeID = employeeID;
            point.PointAmount = pointAmount;
            point.Notes = notes;
            point.CreatedWhen = DateTime.Now;
            point.CreatedWho = principal.Identity.Name;
            context.Add(point);
            context.SaveChanges();
        }

        public void DeleteSalesPoint(int contractID, int employeeID)
        {
            var salesPoint = context.SalesPoints
                .SingleOrDefault(sp => sp.ContractID == contractID 
                                    && sp.EmployeeID == employeeID);
            if (salesPoint != null)
            {
                context.Delete(salesPoint);
                context.SaveChanges();
            }
        }

        public decimal GetTotalPoints(int contractID)
        {
            var totalPoints = 0M;

            var totalPointsQuery = context.SalesPoints.Where(contract => contract.ContractID == contractID);

            if(totalPointsQuery.Any())
                totalPoints = totalPointsQuery.Sum(sp => sp.PointAmount);
            
            return totalPoints;
        }

        public int GetHomeBranchID()
        {
            return GetHomeBranchID(principal.Identity.Name);
        }

        public int GetHomeBranchID(string userName)
        {
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == userName);
            if(employee != null)
                return employee.HomeBranchID;
            return 0;
        }

        public void ChangeHomeBranch(int branchID)
        {
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == principal.Identity.Name);
            if(employee != null)
            {
                employee.HomeBranchID = branchID;
                context.SaveChanges();
            }
        }
    }
}
