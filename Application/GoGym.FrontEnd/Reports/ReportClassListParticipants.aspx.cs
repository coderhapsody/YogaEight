using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.Reports
{
    public partial class ReportClassListParticipants : BaseForm
    {
        [Inject]
        public ClassProvider ClassService { get; set; }
        [Inject]
        public InstructorProvider InstructorService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public CustomerProvider CustomerService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var branches = BranchService.GetActiveBranches(User.Identity.Name);
                PopulateBranches(branches);
                calDate.SelectedDate = DateTime.Today;
            }
        }

        private void PopulateBranches(IEnumerable<Branch> branches)
        {
            ddlBranch.DataSource = branches;
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);

            calDate.SelectedDate = DateTime.Today;
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwSchedule.DataBind();
        }

        protected void sdsSchedule_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (Page.IsPostBack)
            {
                e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
                e.Command.Parameters["@Date"].Value = calDate.SelectedDate.GetValueOrDefault(DateTime.Today);
            }
        }
        protected void gvwSchedule_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DynamicControlBinding.HideGridViewRowId(0, e);
        }
        protected void gvwSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hypViewReport = e.Row.FindControl("hypViewReport") as HyperLink;
                hypViewReport.Attributes.Add("onclick",
                    "showSimplePopUp('PrintPreview.aspx?RDL=PaidClassListParticipant&ClassRunningID=" + e.Row.Cells[0].Text + "');");
            }
        }
    }
}