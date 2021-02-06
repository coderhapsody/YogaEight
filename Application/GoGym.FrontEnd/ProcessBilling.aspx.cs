using System;
using System.Collections.Generic;
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
    public partial class ProcessBilling : BaseForm
    {
        [Inject]
        public PackageProvider PackageService { get; set; }
        [Inject]
        public CustomerStatusProvider CustomerStatusService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public BillingProvider BillingService { get; set; }
        [Inject]
        public BillingTypeProvider BillingTypeService { get; set; }        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cblMembershipType.DataSource = PackageService.GetAll();
                cblMembershipType.DataTextField = "Name";
                cblMembershipType.DataValueField = "ID";
                cblMembershipType.DataBind();

                cblCustomerStatus.DataSource = CustomerStatusService.GetAll();
                cblCustomerStatus.DataTextField = "Description";
                cblCustomerStatus.DataValueField = "ID";
                cblCustomerStatus.DataBind();

                DynamicControlBinding.BindDropDown(ddlBillingType, BillingTypeService.GetActiveBillingTypes().Where(bt => bt.ID > 1), "Description", "ID", false);
                DynamicControlBinding.BindDropDown(ddlBranch, BranchService.GetActiveBranches(User.Identity.Name), "Name", "ID", false);

                ddlBranch.Enabled = false;

                calFindDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calFindDateTo.SelectedDate = DateTime.Today;
                cblMembershipType.Items.Cast<ListItem>().ToList().ForEach(item => item.Selected = true);
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwBilling.DataSource = BillingService.GetBillingData(
                Convert.ToInt32(ddlBranch.SelectedValue),
                Convert.ToInt32(ddlBillingType.SelectedValue),
                cblCustomerStatus.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Convert.ToInt32(item.Value)).ToArray(),
                cblMembershipType.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Convert.ToInt32(item.Value)).ToArray(),
                calFindDateFrom.SelectedDate.GetValueOrDefault(DateTime.Today),
                calFindDateTo.SelectedDate.GetValueOrDefault(DateTime.Today)).OrderBy(data => data.CustomerBarcode);
            gvwBilling.DataBind();
            foreach (GridViewRow gridRow in gvwBilling.Rows)
            {
                (gridRow.Cells[gridRow.Cells.Count - 1].Controls[1] as CheckBox).Checked = true;
            }
        }
        protected void btnProcessAll_Click(object sender, EventArgs e)
        {
            if (BillingService.IsMerchantCodeValid(Convert.ToInt32(ddlBranch.SelectedValue)))
            {
                string[] contractNumbers = gvwBilling.Rows.Cast<GridViewRow>()
                                                .Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked)
                                                .Select(row => row.Cells[4].Text).ToArray();
                BillingService.DeleteBillingHistoryByProcessDate(DateTime.Today);
                try
                {
                    string billingFile = BillingService.ProcessBilling(
                        Convert.ToInt32(ddlBranch.SelectedValue),
                        Convert.ToInt32(ddlBillingType.SelectedValue),
                        contractNumbers,
                        DateTime.Today,
                        User.Identity.Name,
                        calFindDateFrom.SelectedDate.GetValueOrDefault(DateTime.Today),
                        calFindDateTo.SelectedDate.GetValueOrDefault(DateTime.Today),
                        Server.MapPath("~/billing/"));
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "billing",
                        String.Format("alert('Billing file created {0}');", billingFile),
                        true);
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "billing", String.Format("alert('Invalid Merchant Code, please go to master Branch menu to set it up.');"), true);
            }
        }
    }
}