using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Utilities;

namespace GoGym.Providers
{
    public class SecurityProvider : BaseProvider
    {
        public SecurityProvider(FitnessEntities context, IPrincipal principal)
            : base(context, principal)
        {
        }

        public bool ValidateLogin(string userName, string password, string ipAddress = null, string workstationName = null, bool checkLoginHistory = false)
        {
            var valid = false;
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == userName && emp.IsActive);
            if (employee != null)
            {
                var clearPassword = RijndaelHelper.Decrypt(employee.Password, cryptographyKey);
                valid = clearPassword == password;
            }

            if (checkLoginHistory)
            {
                if (Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.Login.History]))
                {
                    var logInHistory = new LogInHistory();
                    logInHistory.LogInWhen = DateTime.Now;
                    logInHistory.UserName = userName;
                    logInHistory.CanLogIn = valid;
                    logInHistory.IPAddress = ipAddress;
                    logInHistory.WorkstationName = workstationName;
                    context.Add(logInHistory);
                    context.SaveChanges();
                }
            }

            return valid;
        }

        public void UpdateRoleMenu(int menuID, int[] roles)
        {
            var selectedMenu = context.Menus.Single(menu => menu.ID == menuID);
            selectedMenu.Roles.Clear();

            foreach (int roleID in roles)
            {
                var selectedRole = context.Roles.SingleOrDefault(role => role.ID == roleID);
                selectedMenu.Roles.Add(selectedRole);
            }
            context.SaveChanges();
        }

        public Menu GetMenu(int id)
        {
            return context.Menus.SingleOrDefault(m => m.ID == id);
        }

        public IEnumerable<Menu> GetAllMenus()
        {
            return context.Menus.Where(m => m.IsActive);
        }

        public IEnumerable<Menu> GetAllMenus(int? parentMenuID)
        {
            var employee = context.Employees.Single(emp => emp.UserName == principal.Identity.Name);

            if (parentMenuID.HasValue)
                return from menu in context.Menus
                       from role in menu.Roles
                       where menu.IsActive && menu.ParentMenuID.Value == parentMenuID.Value && role.ID == employee.RoleID
                       orderby menu.Seq
                       select menu;

            return from menu in context.Menus
                   from role in menu.Roles
                   where menu.IsActive && !menu.ParentMenuID.HasValue && role.ID == employee.RoleID
                   orderby menu.Seq
                   select menu;
        }

        public IEnumerable<Menu> GetAllMenusIgnoringRole(int? parentMenuID)
        {
            if (parentMenuID.HasValue)
                return from menu in context.Menus
                       where menu.IsActive && menu.ParentMenuID.Value == parentMenuID.Value
                       orderby menu.Seq
                       select menu;

            return from menu in context.Menus
                   where menu.IsActive && !menu.ParentMenuID.HasValue
                   orderby menu.Seq
                   select menu;
        }

        public IEnumerable<Role> GetRolesForMenu(int menuID)
        {
            var query = from menu in context.Menus
                        from role in menu.Roles
                        where menu.ID == menuID
                        select role;
            return query.ToList();
        }

        public string GetRoleName(string userName)
        {
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == userName);
            return employee != null ? employee.Role.Name : String.Empty;
        }

        public int[] ResolveRoles(string[] roleNames)
        {            
            return Array.ConvertAll(roleNames, Convert.ToInt32);
            //var list = new List<int>();
            //foreach (var roleName in roleNames)
            //{
            //    var currentRole = context.Roles.SingleOrDefault(role => role.Name == roleName);
            //    if (currentRole != null)
            //    {
            //        list.Add(currentRole.ID);
            //    }
            //}
            //return list.ToArray();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return context.Roles.ToList();
        }

        public bool CanDeleteRoles(int[] arrayOfID)
        {
            return true;
        }

        public Role GetRole(int id)
        {
            return context.Roles.SingleOrDefault(role => role.ID == id);
        }

        public void AddOrUpdateRole(int roleID, string roleName, bool isActive)
        {
            var role = roleID == 0 ? new Role() : context.Roles.Single(rl => rl.ID == roleID);

            role.Name = roleName;
            if (roleID == 0)
            {
                EntityHelper.SetAuditFieldForInsert(role, principal.Identity.Name);
                context.Add(role);
            }
            else
                EntityHelper.SetAuditFieldForUpdate(role, principal.Identity.Name);

            role.IsActive = isActive;
            context.SaveChanges();
        }

        public void DeleteRoles(int[] arrayOfID)
        {
            context.Delete(
                context.Roles.Where(role => arrayOfID.Contains(role.ID)));
            context.SaveChanges();
        }

        public void ChangePassword(string userName, string newPassword)
        {
            var employee = context.Employees.FirstOrDefault(emp => emp.UserName == userName);
            if (employee != null)
            {
                employee.Password = RijndaelHelper.Encrypt(newPassword, cryptographyKey);
                EntityHelper.SetAuditFieldForUpdate(employee, principal.Identity.Name);

                context.SaveChanges();
            }

        }

        public string ResetPassword(string userName)
        {
            ChangePassword(userName, "mustchange");
            return "mustchange";
        }
    }
}
