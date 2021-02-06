using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class InquiryBillings : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public BillingProvider BillingService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlBranch.DataSource = BranchService.GetAllowedBranches(User.Identity.Name);
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();
                ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);

                ddlYear.DataSource = BillingService.GetYears();
                ddlYear.DataBind();
                ddlYear.SelectedText = DateTime.Today.Year.ToString(CultureInfo.InvariantCulture);
            }
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (Page.IsPostBack)
            {
                e.Command.Parameters["@BranchID"].Value = Convert.ToInt32(ddlBranch.SelectedValue);
                e.Command.Parameters["@ProcessYear"].Value = Convert.ToInt32(ddlYear.SelectedText);
            }
        }
        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string batchNo = e.Row.Cells[1].Text;
                HyperLink hypBillingDetail = e.Row.FindControl("hypBillingDetail") as HyperLink;
                if (hypBillingDetail != null)
                {
                    hypBillingDetail.Attributes.Add("onclick", String.Format("showPromptPopUp('ViewBillingDetailHistory.aspx?BatchNo={0}', null, 600, 900)", batchNo));
                }
            }
        }
    }
}