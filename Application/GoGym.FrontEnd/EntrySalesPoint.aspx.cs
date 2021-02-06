using System;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class EntrySalesPoint : BaseForm
    {
        [Inject]
        public ContractProvider ContractService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WebFormHelper.SetGridViewPageSize(gvwSales);
                int contractID = Convert.ToInt32(Request.QueryString["ContractID"]);
                LoadContractInfo(contractID);
                txtPointAmount.Value = 0;
                CalculateTotalPoints();
            }
        }

        private void LoadContractInfo(int contractID)
        {
            var contract = ContractService.Get(contractID);
            var customer = contract.Customer;
            var package = contract.PackageHeader;
            lblContractNo.Text = contract.ContractNo;
            lblContractDate.Text = contract.Date.ToString("dddd, dd MMMM yyyy");
            lblCustomer.Text = String.Format("{0} - {1} {2}", customer.Barcode, customer.FirstName, customer.LastName);
            lblPackage.Text = package.Name;

            WebFormHelper.BindDropDown(ddlSales, EmployeeService.GetAll(contract.BranchID), "FirstName", "ID", true);

            btnAddSales.Enabled = !contract.VoidDate.HasValue;
        }

        protected void gvwSales_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void btnAddSales_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                try
                {
                    int contractID = Convert.ToInt32(Request.QueryString["ContractID"]);
                    int employeeID = Convert.ToInt32(ddlSales.SelectedValue);
                    EmployeeService.AddSalesPoint(contractID,
                        employeeID,
                        Convert.ToDecimal(txtPointAmount.Value),
                        txtNotes.Text);

                    ddlSales.SelectedIndex = 0;
                    txtPointAmount.Value = 0;
                    txtNotes.Text = String.Empty;
                    
                    gvwSales.DataBind();

                    CalculateTotalPoints();
                }
                catch (Exception ex)
                {                    
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }

        private void CalculateTotalPoints()
        {
            int contractID = Convert.ToInt32(Request.QueryString["ContractID"]);
            lblTotalPoints.Text = EmployeeService.GetTotalPoints(contractID).ToString("##0.00");
        }

        protected void gvwSales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int contractID = Convert.ToInt32(Request.QueryString["ContractID"]);
                int employeeID = Convert.ToInt32(e.CommandArgument);

                try
                {
                    EmployeeService.DeleteSalesPoint(contractID, employeeID);
                    gvwSales.DataBind();

                    CalculateTotalPoints();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
    }
}