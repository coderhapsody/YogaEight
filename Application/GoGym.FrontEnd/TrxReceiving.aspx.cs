using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class TrxReceiving : BaseForm
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

        [Inject]
        public ReceivingProvider ReceiveService { get; set; }

        [Inject]
        public WarehouseProvider WarehouseService { get; set; }

        public List<ReceivingDetailViewModel> Detail
        {
            get
            {
                if (ViewState["Detail"] == null)
                    ViewState["Detail"] = new List<ReceivingDetailViewModel>();
                return ViewState["Detail"] as List<ReceivingDetailViewModel>;
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

        private void FillDropDown()
        {
            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();            
        }

        private void LoadData()
        {
            var receiveHeader = ReceiveService.GetReceiving(RowID);
            
            hypLookUpPO.Attributes["onclick"] =
                String.Format("showPromptPopUp('PromptPurchaseOrder.aspx?Code=cphMainContent_hidPOID&Name=ctl00_cphMainContent_txtDocumentPO&BranchID={0}', null, 550, 900);", receiveHeader.PurchaseOrderHeader.BranchID);

            ddlWarehouse.Items.Clear();
            ddlWarehouse.DataSource = WarehouseService.GetWarehouses(Convert.ToInt32(ddlFindBranch.SelectedValue));
            ddlWarehouse.DataTextField = "Name";
            ddlWarehouse.DataValueField = "ID";
            ddlWarehouse.DataBind();

            lblBranch.Text = receiveHeader.PurchaseOrderHeader.Branch.Name;            

            Detail = ReceiveService.GetreceiveDetail(RowID).ToList();
            receivingDate.SelectedDate = receiveHeader.Date;
            txtDocumentNo.Text = receiveHeader.DocumentNo;
            txtDocumentNo.ReadOnly = true;

            txtDocumentPO.Text = receiveHeader.PurchaseOrderHeader.DocumentNo;
            hidPOID.Value = Convert.ToString(receiveHeader.PurchaseOrderHeader.ID);
            //ddlPurchaseOrder.SelectedValue = Convert.ToString(receiveHeader.PurchaseOrderID);
            txtDocumentPO.ReadOnly = true;
            txtGoodIssueNo.Text = receiveHeader.GoodIssueNo;
            txtFreightInfo.Text = receiveHeader.FreightInfo;
            txtNotes.Text = receiveHeader.Notes;
            ddlWarehouse.SelectedValue = Convert.ToString(receiveHeader.WarehouseID);
            btnVoid.Enabled = true;
            btnPrint.Enabled = true;
            RefreshDetail();

            if (receiveHeader.VoidDate.HasValue)
            {
                btnSave.Enabled = false;
                btnPrint.Enabled = false;
                btnVoid.Enabled = false;
            }
        }


        private void RefreshDetail()
        {
            grdDetail.DataSource = Detail;
            grdDetail.DataBind();

            grdDetail.Columns[0].Visible = false;
            grdDetail.Columns[1].Visible = false;
        }


        #region Control Event
        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            ViewState["BranchID"] = Convert.ToInt32(ddlFindBranch.SelectedValue);
            RefreshDetail();
            txtDocumentNo.Text = "(Auto)";
            receivingDate.SelectedDate = DateTime.Today;
            lblBranch.Text = ddlFindBranch.SelectedItem.Text;
            btnVoid.Enabled = false;
            btnPrint.Enabled = false;

            ddlWarehouse.Items.Clear();
            ddlWarehouse.DataSource = WarehouseService.GetWarehouses(Convert.ToInt32(ddlFindBranch.SelectedValue));
            ddlWarehouse.DataTextField = "Name";
            ddlWarehouse.DataValueField = "ID";
            ddlWarehouse.DataBind();

            hypLookUpPO.Attributes["onclick"] =
                String.Format("showPromptPopUp('PromptPurchaseOrder.aspx?Code=cphMainContent_hidPOID&Name=ctl00_cphMainContent_txtDocumentPO&BranchID={0}', null, 550, 900);", ddlFindBranch.SelectedValue);
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {

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
                        WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit,
                            "Cannot save empty detail",
                            LabelStyleNames.ErrorMessage);
                        return;
                    }

                    foreach (GridItem row in grdDetail.Items)
                    {
                        int itemID = Convert.ToInt32(((Label)row.FindControl("lblQtyPO")).Text);
                        ReceivingDetailViewModel detail = Detail.Single(x => x.ItemID == itemID);
                        detail.QtyReceived = Convert.ToInt32(((RadNumericTextBox)row.FindControl("txtQtyReceived")).Value);
                        detail.Notes = ((RadTextBox)row.FindControl("txtNotes")).Text;
                    }

                    string result = ReceiveService.AddOrUpdateReceiving(
                        RowID,
                       Convert.ToInt32(hidPOID.Value),
                        receivingDate.SelectedDate.GetValueOrDefault(),
                        txtGoodIssueNo.Text,
                        txtFreightInfo.Text,
                        Convert.ToInt32(ddlWarehouse.SelectedValue),
                        txtNotes.Text,
                        Detail);

                    btnPrint.Enabled = true;

                    ReloadCurrentPage();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }

        protected void btnProcessVoid_Click(object sender, EventArgs e)
        {
            try
            {
                ReceiveService.VoidReceiving(RowID, txtVoidReason.Text);
                LoadData();
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, String.Format("Purchase Order {0} has been marked as void", string.Empty), LabelStyleNames.AlternateMessage);

                btnSave.Enabled = false;
                btnVoid.Enabled = false;
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdMaster.DataBind();
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = Convert.ToInt32(ddlFindBranch.SelectedValue);
            e.Command.Parameters["@DateFrom"].Value = dtpFindDateFrom.SelectedDate ?? new DateTime(1980, 1, 1);
            e.Command.Parameters["@DateTo"].Value = dtpFindDateTo.SelectedDate ?? new DateTime(2099, 12, 31);
            e.Command.Parameters["@DocumentNo"].Value = txtFindDocumentNo.Text;
        }

        #endregion

        protected void radBtnRefreshPO_Click(object sender, EventArgs e)
        {
            Detail = ReceiveService.GetUnReceivingOrder(Convert.ToInt32(hidPOID.Value), RowID);
            RefreshDetail();
        }
    }
}