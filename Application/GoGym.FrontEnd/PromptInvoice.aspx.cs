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
    public partial class PromptInvoice : System.Web.UI.Page
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["_Prompt.CallbackWidget"] = Request.QueryString["callback"];
            if (!this.IsPostBack)
            {
                FillDropDown();
                calDateFrom.SelectedDate = DateTime.Today;
                calDateTo.SelectedDate = DateTime.Today;
            }
        }

        private void FillDropDown()
        {
            ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Enabled = ddlBranch.Items.Count > 0;
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
                        String.Format("javascript:window.opener.$find('{0}').set_value('{1}'); window.close(); return false;", ViewState["_Prompt.CallbackWidget"].ToString(), e.Row.Cells[1].Text));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwPrompt.DataBind();
        }
        protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@InvoiceType"].Value = ddlInvoiceType.SelectedValue;
            e.Command.Parameters["@DateFrom"].Value = calDateFrom.SelectedDate.GetValueOrDefault(DateTime.Today);
            e.Command.Parameters["@DateTo"].Value = calDateTo.SelectedDate.GetValueOrDefault(DateTime.Today);
        }
    }
}