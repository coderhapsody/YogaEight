using System;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.Reports
{
    public partial class ReportCustomerStatusHistory : BaseForm
    {
        [Inject]
        public CustomerStatusProvider CustomerStatusService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataBindingHelper.PopulateAllowedBranches(BranchService, ddlBranch, User.Identity.Name, false);
            ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);

            DataBindingHelper.PopulateCustomerStatus(CustomerStatusService, ddlStatus, false);
            ddlStatus.Items.Remove(ddlStatus.FindItemByText("OK"));
            if (!Page.IsPostBack)
            {
                calDateFrom.SelectedDate = DateTime.Today;
                calDateTo.SelectedDate = DateTime.Today;
            }

        }
    }
}