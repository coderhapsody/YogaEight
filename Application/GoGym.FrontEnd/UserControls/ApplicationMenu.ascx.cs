using System;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd.UserControls
{
    public partial class ApplicationMenu : System.Web.UI.UserControl
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            mnuMenu.Items.Clear();
            LoadMenus();
        }


        public void LoadMenus()
        {
            var menus = SecurityService.GetAllMenus(null);
            foreach (var menu in menus)
            {
                var menuItem = new RadMenuItem(menu.Title, ResolveUrl(menu.NavigationTo));
                menuItem.Value = Convert.ToString(menu.ID);

                PopulateChildMenu(menuItem, menu.ID);

                mnuMenu.Items.Add(menuItem);
            }

        }

        private void PopulateChildMenu(RadMenuItem parentMenu, int menuID)
        {
            var childMenus = SecurityService.GetAllMenus(menuID);
            foreach (var childMenu in childMenus)
            {
                var menuItem = new RadMenuItem(childMenu.Title, ResolveUrl(childMenu.NavigationTo));
                menuItem.Value = Convert.ToString(childMenu.ID);
                parentMenu.Items.Add(menuItem);

                PopulateChildMenu(menuItem, childMenu.ID);
            }
        }
    }
}