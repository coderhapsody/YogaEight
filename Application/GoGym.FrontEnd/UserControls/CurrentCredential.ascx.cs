using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.UserControls
{
    public partial class CurrentCredential : System.Web.UI.UserControl
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        [Inject]
        public EmployeeProvider EmployeeService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var emp = EmployeeService.Get(Page.User.Identity.Name);
            
            //lblCurrentUserName.Text = String.Format("{0} [{1}] @ {2}",
            //    Page.User.Identity.Name,
            //    SecurityService.GetRoleName(Page.User.Identity.Name),
            //    emp.Branch.Name);

            lblCurrentUserName.Text = String.Format("{0} [{1}]",
                Page.User.Identity.Name,
                SecurityService.GetRoleName(Page.User.Identity.Name));
        }
    }
}