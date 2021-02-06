using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.UserControls
{
    public partial class ChangeHomeBranch : System.Web.UI.UserControl
    {
        [Inject]
        public EmployeeProvider EmployeeService { get; set; }

        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlChangeHomeBranch.DataSource = BranchService.GetAllowedBranches(Page.User.Identity.Name);
            ddlChangeHomeBranch.DataTextField = "Name";
            ddlChangeHomeBranch.DataValueField = "ID";
            ddlChangeHomeBranch.DataBind();
            ddlChangeHomeBranch.SelectedValue = Convert.ToString(EmployeeService.GetHomeBranchID());
        }

        protected void ddlChangeHomeBranch_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            EmployeeService.ChangeHomeBranch(Convert.ToInt32(e.Value));
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}