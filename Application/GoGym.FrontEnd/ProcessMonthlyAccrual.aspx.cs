using System;
using System.Linq;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ProcessMonthlyAccrual : BaseForm
    {
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDown();
                ddlYear.FindItemByText(DateTime.Today.Year.ToString()).Selected = true;
                ddlMonth.FindItemByValue(DateTime.Today.Month.ToString()).Selected = true;
                ddlBranch.Enabled = false;
            }
        }

        private void FillDropDown()
        {
            DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, User.Identity.Name);
            chkExcludeFirstAccrual.Checked = true;

            WebFormHelper.BindDropDown(ddlMonth, CommonHelper.GetMonthNames(), "value", "key");
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

            ddlYear.Items.Clear();
            ddlYear.Items.Add(new DropDownListItem(Convert.ToString(DateTime.Today.Year - 1)));
            ddlYear.Items.Add(new DropDownListItem(Convert.ToString(DateTime.Today.Year)));
            ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.HideGridViewRowId(e, 1);

        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@AccrualMonth"].Value = ddlMonth.SelectedValue;
            e.Command.Parameters["@AccrualYear"].Value = ddlYear.SelectedItem.Text;
            e.Command.Parameters["@ExcludeFirstAccrual"].Value = chkExcludeFirstAccrual.Checked;
            e.Command.Parameters["@ExcludeFinishAccrual"].Value = chkExcludeFinishAccrual.Checked;
        }

        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hypAccrualHistory = e.Row.FindControl("hypAccrualHistory") as HyperLink;
                if (hypAccrualHistory != null)
                {
                    int invoiceID = Convert.ToInt32(e.Row.Cells[1].Text);
                    hypAccrualHistory.Attributes.Add("onclick", String.Format("showPromptPopUp('ViewAccrualHistory.aspx?InvoiceID={0}', null, 600, 1200)", invoiceID));
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                int[] invoiceIDs = gvwMaster.Rows.Cast<GridViewRow>()
                                            .Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked)
                                            .Select(row => Convert.ToInt32(row.Cells[0].Text)).ToArray();
                int totalInvoice = InvoiceService.ProcessAccrualInvoices(Convert.ToInt32(ddlBranch.SelectedValue),
                    invoiceIDs,
                    DateTime.Today);
                gvwMaster.DataBind();

                WebFormHelper.SetLabelTextWithCssClass(lblStatus,
                    String.Format("{0} invoice(s) have been processed successfully.", totalInvoice),
                    LabelStyleNames.InfoMessage);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
    }
}