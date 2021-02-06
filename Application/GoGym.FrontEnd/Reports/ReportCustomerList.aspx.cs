using System;
using System.Globalization;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd.Reports
{
    public partial class ReportCustomerList : BaseForm
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataBindingHelper.PopulateAllowedBranches(BranchService, ddlBranch, User.Identity.Name, false);
                ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);

                ddlMonth.DataSource = CommonHelper.GetMonthNames();
                ddlMonth.DataTextField = "Value";
                ddlMonth.DataValueField = "Key";
                ddlMonth.DataBind();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString(CultureInfo.InvariantCulture);

                int minYear, maxYear;
                try
                {
                    CustomerService.GetMinMaxCustomerJoinYear(out minYear, out maxYear);
                    maxYear = DateTime.Today.Year;
                }
                catch
                {
                    minYear = DateTime.Today.Year;
                    maxYear = minYear;
                }

                for (int year = minYear; year <= maxYear; year++)
                    ddlYear.Items.Add(new DropDownListItem(year.ToString(CultureInfo.InvariantCulture), year.ToString(CultureInfo.InvariantCulture)));
                ddlYear.FindItemByText(DateTime.Today.Year.ToString(CultureInfo.InvariantCulture)).Selected = true;

            }
        }
    }
}