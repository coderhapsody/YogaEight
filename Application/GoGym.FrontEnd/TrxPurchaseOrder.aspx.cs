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
    public partial class TrxPurchaseOrder : BaseForm
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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                RadHelper.SetUpGrid(grdMaster);
                FillDropDown();
            }
        }

        private void FillDropDown()
        {
            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();

            cboSupplier.DataSource = SupplierService.GetSuppliers();
            cboSupplier.DataTextField = "Name";
            cboSupplier.DataValueField = "ID";
            cboSupplier.DataBind();

            ddlTerms.DataSource = PurchaseOrderService.GetTerms();
            ddlTerms.DataValueField = "Name";
            ddlTerms.DataTextField = "Name";
            ddlTerms.DataBind();

            cboSupplier.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(String.Empty));
        }

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

        protected void lnbDelete_Click(object sender, EventArgs e)
        {

        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            ViewState["BranchID"] = Convert.ToInt32(ddlFindBranch.SelectedValue);
            RefreshDetail();
            dtpDate.SelectedDate = DateTime.Today;
            dtpExpectedDate.SelectedDate = DateTime.Today.AddDays(7);
            lblBranch.Text = ddlFindBranch.SelectedItem.Text;
            txtDocumentNo.Text = "(Auto)";
            btnVoid.Enabled = false;
            btnPrint.Enabled = false;
            cboSupplier.Focus();
            //txt
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdMaster.DataBind();
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
            ddlTerms.SelectedValue = poHeader.Terms;
            cboSupplier.SelectedValue = poHeader.SupplierID.ToString();
            dtpDate.SelectedDate = poHeader.DocumentDate;
            dtpExpectedDate.SelectedDate = poHeader.ExpectedDate;
            txtNotes.Text = poHeader.Notes;
            SupplierInformation.LoadSupplierInformation(poHeader.SupplierID);
            lblPONo.Text = String.Format("{0} - {1}", poHeader.DocumentNo, PurchaseOrderService.TranslateStatus(Convert.ToString(poHeader.Status)));
            btnVoid.Enabled = true;
            btnPrint.Enabled = true;
            RefreshDetail();

            if (poHeader.VoidWhen.HasValue || poHeader.Status == 'N' || poHeader.Status == 'A')
            {
                btnSave.Enabled = false;
                btnVoid.Enabled = false;
            }

            if (poHeader.Status != 'A')
            {
                btnPrint.Enabled = false;
            }

            switch (poHeader.Status)
            {
                case 'A':
                    lblPOStatus.Text = "Approved";
                    break;
                case 'N':
                    lblPOStatus.Text = "Not Approved";
                    break;
                case 'V':
                    lblPOStatus.Text = "Void";
                    break;
                default:
                    lblPOStatus.Text = "Open";
                    break;
            }
            //btnPrint.Attributes.Add("onclick",
            //    String.Format("showSimplePopUp('ReportPreview.aspx?ReportName={0}&DocumentNo={1}'); return false;",
            //        "SlipPurchaseOrder",
            //        poHeader.DocumentNo));
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = Convert.ToInt32(ddlFindBranch.SelectedValue);
            e.Command.Parameters["@DateFrom"].Value = dtpFindDateFrom.SelectedDate ?? new DateTime(1980, 1, 1);
            e.Command.Parameters["@DateTo"].Value = dtpFindDateTo.SelectedDate ?? new DateTime(2099, 12, 31);
            e.Command.Parameters["@DocumentNo"].Value = txtFindDocumentNo.Text;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEdit");
            if (Page.IsValid)
            {
                try
                {
                    if (Detail.Count == 0)
                    {
                        WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, "Cannot save empty detail", LabelStyleNames.ErrorMessage);
                        return;
                    }

                    if (PurchaseOrderService.IsReceive(RowID))
                    {
                        WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, "Cannot editing Purchase order already have receiving transaction", LabelStyleNames.ErrorMessage);
                        return;
                    }

                    var documentNo = PurchaseOrderService.AddOrUpdatePurchaseOrder(
                        RowID,
                        Convert.ToInt32(ViewState["BranchID"]),
                        dtpDate.SelectedDate.GetValueOrDefault(),
                        dtpExpectedDate.SelectedDate.GetValueOrDefault(),
                        Convert.ToInt32(cboSupplier.SelectedValue),
                        txtNotes.Text,
                        Convert.ToDecimal(ntbDiscValue.Value),
                        ddlTerms.SelectedItem.Text,
                        Detail);

                    btnPrint.Enabled = true;

                    ReloadCurrentPage();

                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                }
            }
        }

        protected void cboSupplier_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            cboSupplier.DataSource = SupplierService.GetSuppliers(e.Text);
            cboSupplier.DataTextField = "Name";
            cboSupplier.DataValueField = "ID";
            cboSupplier.DataBind();
        }

        protected void cboItem_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            cboItem.DataSource = ItemService.GetMaterialItems(e.Text);
            cboItem.DataTextField = "Description";
            cboItem.DataValueField = "ID";
            cboItem.DataBind();
        }

        protected void cboItem_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                var units = ItemService.GetItemUnits(Convert.ToInt32(e.Value));
                ddlUnit.DataSource = units;
                ddlUnit.DataBind();

                GetDefaultPrice();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, "Invalid Item: " + ex.Message, LabelStyleNames.ErrorMessage);
            }
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            Page.Validate("AddDetail");
            if (Page.IsValid)
            {
                if (Convert.ToInt32(ntbQty.Value) == 0)
                    ntbQty.Value = 1;

                try
                {
                    var detailLine = new PurchaseOrderDetailViewModel();
                    var item = ItemService.Get(Convert.ToInt32(cboItem.SelectedValue));

                    detailLine.ItemID = item.ID;
                    detailLine.ItemCode = item.Barcode;
                    detailLine.ItemName = item.Description;
                    detailLine.UnitName = ddlUnit.SelectedItem.Text;
                    detailLine.Quantity = Convert.ToInt32(ntbQty.Value);
                    detailLine.UnitPrice = Convert.ToDecimal(ntbUnitPrice.Value);
                    detailLine.IsTaxed = true;
                    detailLine.DiscountRate = Convert.ToDecimal(ntbDisc.Value);
                    detailLine.Total = (detailLine.Quantity * detailLine.UnitPrice) -
                                       (detailLine.Quantity * detailLine.UnitPrice * detailLine.DiscountRate / 100);

                    if (detailLine.IsTaxed)
                        detailLine.Total += detailLine.Total * 0.1M;

                    detailLine.ID = Detail.Any() ? Detail.Max(detail => detail.ID) + 1 : 1;
                    Detail.Add(detailLine);

                    RefreshDetail();

                    cboItem.Text = "";
                    ntbQty.Value = 1;
                    ddlUnit.SelectedIndex = -1;
                    ntbUnitPrice.Value = 0;
                    ntbDisc.Value = 0;

                    cboItem.Focus();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                }

            }
        }

        protected void ddlUnit_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            GetDefaultPrice();
        }

        private void GetDefaultPrice()
        {
            if (!String.IsNullOrEmpty(cboItem.SelectedValue) && !String.IsNullOrEmpty(cboItem.SelectedValue) &&
                !String.IsNullOrEmpty(ddlUnit.SelectedText))
            {
                try
                {
                    var defaultPrice = 0M; /*ItemService.GetDefaultPrice(
                        Convert.ToInt32(cboSupplier.SelectedValue),
                        Convert.ToInt32(cboItem.SelectedValue),
                        ddlUnit.SelectedText);*/
                    ntbUnitPrice.Value = Convert.ToDouble(defaultPrice);

                }
                catch
                {
                    ntbUnitPrice.Value = 0;
                }

                if (ntbQty.Value == 0)
                    ntbQty.Value = 1;
                ntbQty.Focus();
            }
        }

        protected void grdDetail_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteDetail")
            {
                int detailID = Convert.ToInt32(e.CommandArgument);
                var detailLine = Detail.FirstOrDefault(detail => detail.ID == detailID);
                if (detailLine != null)
                    Detail.Remove(detailLine);
                RefreshDetail();
            }
        }

        protected void cboSupplier_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int supplierID = 0;
            if (Int32.TryParse(e.Value, out supplierID))
            {
                SupplierInformation.LoadSupplierInformation(supplierID);
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
                PurchaseOrderService.VoidPurchaseOrder(txtDocumentNo.Text, txtVoidReason.Text);
                LoadData();
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, String.Format("Purchase Order {0} has been marked as void", txtDocumentNo.Text), LabelStyleNames.AlternateMessage);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }
    }
}