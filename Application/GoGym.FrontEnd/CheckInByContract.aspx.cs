using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class CheckInByContract : System.Web.UI.Page
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();

                hypLookUpCustomer.Attributes["onclick"] =
                    String.Format("showPromptPopUp('PromptContractCheckIn.aspx?', '{0}', 550, 900);", txtBarcode.ClientID);
            }
        }
    }
}