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
    public partial class ProcessUnpaidBilling : System.Web.UI.Page
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public BillingProvider BillingService { get; set; }
        [Inject]
        public CustomerStatusProvider CustomerStatusService { get; set; }        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                cblCustomerStatus.DataSource = CustomerStatusService.GetAll();
                cblCustomerStatus.DataTextField = "Description";
                cblCustomerStatus.DataValueField = "ID";
                cblCustomerStatus.DataBind();

                DynamicControlBinding.BindDropDown(ddlBranch, BranchService.GetActiveBranches(User.Identity.Name), "Name", "ID", false);
                ddlBranch.Enabled = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (this.IsPostBack)
            {
                string statusWithComma = String.Join(",", cblCustomerStatus.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Convert.ToInt32(item.Value)).ToArray());
                e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
                e.Command.Parameters["@FromDate"].Value = calFromDate.SelectedDate;
                e.Command.Parameters["@ToDate"].Value = calToDate.SelectedDate;
                e.Command.Parameters["@StatusWithComma"].Value = statusWithComma;
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                string[] invoices = gvwMaster.Rows.Cast<GridViewRow>().Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked).Select(row => row.Cells[0].Text).ToArray();
                string fileName = BillingService.GenerateBillingUnpaidInvoice(
                    Convert.ToInt32(ddlBranch.SelectedValue),
                    invoices,
                    Server.MapPath("~/billing/"));
                litResult.Text = String.Format("Billing result file <b><a target='_new' href='{0}'>{1}</a></b>",
                    this.ResolveClientUrl("~/billing/" + fileName),
                    fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "_alert",
                    "alert('" + ex.Message + "');",
                    true);
            }
        }
        protected void btnProcessResult_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Server.MapPath("~/Temp/") + fupResultFile.FileName;
                fupResultFile.SaveAs(fileName);
                int count = BillingService.ProcessBillingUnpaidInvoice(fileName);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "_alert",
                    String.Format("alert('{0} accepted invoice(s) are successfully processed');", count),
                    true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "_alert",
                    "alert('" + ex.Message + "');",
                    true);
            }
        }
    }
}