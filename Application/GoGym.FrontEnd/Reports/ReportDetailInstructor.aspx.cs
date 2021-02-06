using System;
using System.Globalization;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Telerik.Web.UI;

namespace GoGym.FrontEnd.Reports
{
    public partial class ReportDetailInstructor : BaseForm
    {
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataBindingHelper.PopulateAllowedBranches(BranchService, ddlBranch, User.Identity.Name, false);
            ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);

            ddlMonth.DataSource = CommonHelper.GetMonthNames();
            ddlMonth.DataTextField = "Value";
            ddlMonth.DataValueField = "Key";
            ddlMonth.DataBind();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString(CultureInfo.InvariantCulture);

            for (int year = DateTime.Today.Year - 3; year <= DateTime.Today.Year; year++)
                ddlYear.Items.Add(new DropDownListItem(year.ToString(CultureInfo.InvariantCulture)));
            ddlYear.FindItemByText(DateTime.Today.Year.ToString(CultureInfo.InvariantCulture)).Selected = true;

        }
    }
}