using System;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class CheckIn : System.Web.UI.Page
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
                    String.Format("showPromptPopUp('PromptCustomer.aspx?', '{0}', 550, 900);", txtBarcode.ClientID);
            }
        }
    }
}