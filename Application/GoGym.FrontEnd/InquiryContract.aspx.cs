using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.Configuration;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class InquiryContract : BaseForm
    {
        CheckInConfiguration checkInConfiguration = new CheckInConfiguration();

        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public BillingTypeProvider BillingTypeService { get; set; }
        [Inject]
        public PackageProvider PackageService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateDropDown();

                calFindDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calFindDateTo.SelectedDate = DateTime.Today;
            }
        }

        private void PopulateDropDown()
        {
            DataBindingHelper.PopulateAllowedBranches(BranchService, ddlFindBranch, User.Identity.Name, false);
            ddlFindBranch.SelectedValue = Convert.ToString(HomeBranchID);

            DataBindingHelper.PopulateBillingTypes(BillingTypeService, ddlFindBillingType, true);
            ddlFindBillingType.Items[0].Value = "0";

            DataBindingHelper.PopulatePackages(PackageService, ddlFindPackage, true);
            ddlFindPackage.Items[0].Value = "0";

            ddlFindStatus.Items.Add(new DropDownListItem(String.Empty));
            ddlFindStatus.Items.Add(new DropDownListItem("Pending", ContractStatus.UNPAID));
            ddlFindStatus.Items.Add(new DropDownListItem("Active", ContractStatus.PAID));
            ddlFindStatus.Items.Add(new DropDownListItem("Closed", ContractStatus.CLOSED));
            ddlFindStatus.Items.Add(new DropDownListItem("Void", ContractStatus.VOID));

            ddlFindStatus.SelectedIndex = 0;
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@ContractNo"].Value = txtFindContractNo.Text;
            e.Command.Parameters["@DateFrom"].Value = calFindDateFrom.SelectedDate.GetValueOrDefault(DateTime.Today).ToString("yyyy-MM-dd");
            e.Command.Parameters["@DateTo"].Value = calFindDateTo.SelectedDate.GetValueOrDefault(DateTime.Today).ToString("yyyy-MM-dd");
            e.Command.Parameters["@CustomerCode"].Value = txtFindBarcode.Text;
            e.Command.Parameters["@PackageID"].Value = ddlFindPackage.SelectedValue;
            e.Command.Parameters["@BillingTypeID"].Value = ddlFindBillingType.SelectedValue;
            e.Command.Parameters["@Status"].Value = ddlFindStatus.SelectedValue;
        }
        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string contractNo = e.Row.Cells[0].Text;
                string reportAgreementForm = checkInConfiguration.ReportAgreementForm;
                var hypPrint = e.Row.FindControl("hypPrint") as HyperLink;
                var printPreviewUrl = ResolveUrl("~/Reports/PrintPreview.aspx");
                if (hypPrint != null)
                    hypPrint.Attributes.Add("onclick", String.Format("showSimplePopUp('{0}?RDL=" + reportAgreementForm + "&ContractNo={1}')", printPreviewUrl, contractNo));

            }
        }
    }
}