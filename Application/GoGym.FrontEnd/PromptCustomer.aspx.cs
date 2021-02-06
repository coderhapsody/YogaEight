using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class PromptCustomer : System.Web.UI.Page
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["_Prompt.CallbackWidget"] = Request.QueryString["callback"];
            if (!this.IsPostBack)
            {
                DataBindingHelper.PopulateActiveBranches(BranchService, ddlFindBranch, User.Identity.Name, false);
            }
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
                var hypSelect = e.Row.FindControl("hypSelect") as HyperLink;
                if (hypSelect != null)
                    hypSelect.Attributes.Add(
                        "onclick",
                        String.Format("javascript:window.opener.$find('{0}').set_value('{1}'); window.close(); window.opener.$find('{0}').focus(); return false;", ViewState["_Prompt.CallbackWidget"], e.Row.Cells[1].Text));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwPrompt.DataBind();
        }
        protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@Barcode"].Value = txtFindCustomerCode.Text;
            e.Command.Parameters["@Name"].Value = txtFindCustomerName.Text;
            e.Command.Parameters["@ParentName"].Value = txtFindParentName.Text;
            e.Command.Parameters["@PhoneNo"].Value = txtFindPhoneNo.Text;
        }
    
    }
}