using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.FrontEnd.UserControls;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class ApprovalPurchaseOrder : BaseForm
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        [Inject]
        public BranchProvider BranchService { get; set; }

        [Inject]
        public ItemProvider ItemService { get; set; }

        [Inject]
        public SupplierProvider SupplierService { get; set; }

        [Inject]
        public PurchaseOrderProvider PurchaseOrderService { get; set; }

        public List<PurchaseOrderDetailViewModel> Detail
        {
            get
            {
                if (ViewState["Detail"] == null)
                    ViewState["Detail"] = new List<PurchaseOrderDetailViewModel>();
                return ViewState["Detail"] as List<PurchaseOrderDetailViewModel>;
            }
            set { ViewState["Detail"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                RadHelper.SetUpGrid(grdMaster);
                FillDropDown();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdMaster.DataBind();
        }

        protected void btnProcessApprove_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderService.ApprovedPurchaseOrder(txtDocumentNo.Text,
                   EmployeeService.Get(User.Identity.Name).ID, txtApproveReason.Text);
                LoadData();
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, String.Format("Purchase Order {0} has been marked as APPROVED", txtDocumentNo.Text), LabelStyleNames.AlternateMessage);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnProcessVoid_Click(object sender, EventArgs e)
        {
            try
            {
                if (PurchaseOrderService.IsReceive(RowID))
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, "Purchase order already have receiving transaction", LabelStyleNames.ErrorMessage);
                    return;
                }
                PurchaseOrderService.NotApprovedPurchaseOrder(txtDocumentNo.Text, txtVoidReason.Text);
                LoadData();
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, String.Format("Purchase Order {0} has been marked as not approved", txtDocumentNo.Text), LabelStyleNames.AlternateMessage);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void grdMaster_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                mvwForm.SetActiveView(viwAddEdit);
                RowID = Convert.ToInt32(e.CommandArgument);
                LoadData();
            }
        }

        private void LoadData()
        {
            var poHeader = PurchaseOrderService.GetPurchaseOrder(RowID);
            Detail = PurchaseOrderService.GetPurchaseOrderDetail(RowID).ToList();
            txtDocumentNo.Text = poHeader.DocumentNo;
            lblBranch.Text = poHeader.Branch.Name;
            ViewState["BranchID"] = poHeader.BranchID;
            lblSupplierName.Text = SupplierService.GetSupplier(poHeader.SupplierID).Name;
            lblDate.Text = poHeader.DocumentDate.ToString("dd/MM/yyyy");
            lblExpectedDate.Text = Convert.ToDateTime(poHeader.ExpectedDate).ToString("dd/MM/yyyy");
            txtNotes.Text = poHeader.Notes;
            SupplierInformation.LoadSupplierInformation(poHeader.SupplierID);
            lblTerms.Text = poHeader.Terms;
            lblDiscValue.Text = poHeader.DiscountValue.ToString("#0.00");
            lblPONo.Text = String.Format("{0} - {1}", poHeader.DocumentNo, PurchaseOrderService.TranslateStatus(Convert.ToString(poHeader.Status)));
            btnApprove.Enabled = true;
            RefreshDetail();


            if (poHeader.VoidWhen.HasValue || poHeader.ApprovedByEmployeeID.HasValue)
            {
                btnApprove.Enabled = false;
                btnVoid.Enabled = false;
            }
        }

        private void RefreshDetail()
        {
            grdDetail.DataSource = Detail;
            grdDetail.DataBind();

            grdDetail.Columns[0].Visible = false;

            decimal totalBeforeTax = Detail.Sum(
                line => line.Quantity * line.UnitPrice - (line.Quantity * line.UnitPrice * line.DiscountRate / 100));

            decimal totalAfterTax =
                Detail.Sum(
                    line =>
                        (line.Quantity * line.UnitPrice - (line.Quantity * line.UnitPrice * line.DiscountRate / 100)) *
                        (line.IsTaxed ? 1.1M : 1M));

            decimal totalTax = totalAfterTax - totalBeforeTax;

            lblTotalBeforeTax.Text = totalBeforeTax.ToString("c");
            lblTotalAfterTax.Text = totalAfterTax.ToString("c");
            lblTaxAmouont.Text = totalTax.ToString("c");

        }

        private void FillDropDown()
        {
            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = Convert.ToInt32(ddlFindBranch.SelectedValue);
            e.Command.Parameters["@DateFrom"].Value = dtpFindDateFrom.SelectedDate ?? new DateTime(1980, 1, 1);
            e.Command.Parameters["@DateTo"].Value = dtpFindDateTo.SelectedDate ?? new DateTime(2099, 12, 31);
            e.Command.Parameters["@DocumentNo"].Value = txtFindDocumentNo.Text;
        }

        protected void btnVoid_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }
    }
}