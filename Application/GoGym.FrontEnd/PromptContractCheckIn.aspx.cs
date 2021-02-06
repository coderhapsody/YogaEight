using System;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class PromptContractCheckIn : System.Web.UI.Page
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public UserProvider UserService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["_Prompt.CallbackWidget"] = Request.QueryString["callback"];
            if (!this.IsPostBack)
            {
                FillDropDown();
            }
        }

        private void FillDropDown()
        {
            ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();

            ddlBranch.SelectedValue = Request.QueryString["BranchID"];
            //UserService.GetCurrentActiveBranches(User.Identity.Name).ToList()[0].ID.ToString();
        }

        protected void gvwPrompt_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void gvwPrompt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hypSelect = e.Row.FindControl("hypSelect") as HyperLink;
                if (hypSelect != null)
                    hypSelect.Attributes.Add(
                        "onclick",
                        String.Format("javascript:window.opener.$find('{0}').set_value('{1}'); window.opener.document.getElementById('cphMainContent_btnDummy').click(); window.close(); return false;", ViewState["_Prompt.CallbackWidget"], e.Row.Cells[2].Text));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwPrompt.DataBind();
        }
        protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@ContractNo"].Value = txtContractNo.Text;
            e.Command.Parameters["@CustomerName"].Value = txtCustomerName.Text;
        }
    }
}