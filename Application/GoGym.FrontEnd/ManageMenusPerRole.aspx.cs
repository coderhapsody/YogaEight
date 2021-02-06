using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Menu = GoGym.Data.Menu;

namespace GoGym.FrontEnd
{
    public partial class ManageMenusPerRole : BaseForm
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMenus();
                btnSave.Visible = false;
            }
        }

        private void PopulateMenus()
        {
            var menus = SecurityService.GetAllMenusIgnoringRole(null);
            foreach (Menu menu in menus)
            {
                var menuItem = new TreeNode(menu.Title, menu.ID.ToString(CultureInfo.InvariantCulture));

                PopulateChildMenu(menuItem, menu.ID);

                tvwMenus.Nodes.Add(menuItem);
                menuItem.Expanded = false;
            }

        }

        private void PopulateChildMenu(TreeNode parentMenu, int menuID)
        {
            IEnumerable<Menu> childMenus = SecurityService.GetAllMenusIgnoringRole(menuID);
            foreach (Menu childMenu in childMenus)
            {
                var menuItem = new TreeNode(childMenu.Title, childMenu.ID.ToString(CultureInfo.InvariantCulture));
                parentMenu.ChildNodes.Add(menuItem);

                PopulateChildMenu(menuItem, childMenu.ID);
            }
        }

        protected void tvwMenus_SelectedNodeChanged(object sender, EventArgs e)
        {
            int menuID = Convert.ToInt32(tvwMenus.SelectedValue);
            RowID = menuID;

            lblMenuName.Text = "Current selected menu : " + tvwMenus.SelectedNode.Text;

            cblRoles.DataSource = SecurityService.GetAllRoles();
            cblRoles.DataValueField = "ID";
            cblRoles.DataTextField = "Name";
            cblRoles.DataBind();

            btnSave.Visible = true;

            IEnumerable<Role> roles = SecurityService.GetRolesForMenu(menuID);
            foreach (var role in roles)
                cblRoles.Items.FindByText(role.Name).Selected = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] selectedRoles =
                    cblRoles.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => item.Value).ToArray();
                int[] roles = SecurityService.ResolveRoles(selectedRoles);
                SecurityService.UpdateRoleMenu(RowID, roles);

                ScriptManager.RegisterStartupScript(this, GetType(), "_save", "alert('Menu settings saved')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_save", String.Format("alert('{0}')", ex.Message), true);
                LogService.ErrorException(GetType().FullName, ex);               
            }
        }
    }
}