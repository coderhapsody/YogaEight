using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class TrxInventoryAdjustment : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        [Inject]
        public ItemProvider ItemService { get; set; }

        [Inject]
        public InventoryMutationProvider InventoryMutationService { get; set; }

        public List<InventoryMutationDetailViewModel> Details
        {
            get
            {
                return ViewState["Details"] == null
                    ? new List<InventoryMutationDetailViewModel>()
                    : ViewState["Details"] as List<InventoryMutationDetailViewModel>;
            }
            set { ViewState["Details"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);

                FillDropDown();
            }
        }

        private void FillDropDown()
        {
            DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, User.Identity.Name);
            ddlDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ddlDateTo.SelectedDate = DateTime.Today;

        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@FromDate"].Value = ddlDateFrom.SelectedDate;
            e.Command.Parameters["@ToDate"].Value = ddlDateTo.SelectedDate;
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            Details = new List<InventoryMutationDetailViewModel>();
            PopulateDropDown();
            RefreshDetail();
        }

        private void PopulateDropDown()
        {
            WebFormHelper.BindDropDown(ddlItem, ItemService.GetMaterialItems(), "Description", "ID", true);            
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            Page.Validate("AddDetail");
            if(Page.IsValid)
            {
                var detail = new InventoryMutationDetailViewModel();
                var item = ItemService.Get(Convert.ToInt32(ddlItem.SelectedValue));
                detail.ItemID = item.ID;
                detail.ItemBarcode = item.Barcode;
                detail.ItemDescription = item.Description;
                detail.UnitName = ddlUnit.SelectedValue;
                detail.Qty = Convert.ToInt32(ntbQty.Value.GetValueOrDefault());
                detail.Notes = txtNotesDetail.Text;
                detail.ID = Details.Any() ? Details.Max(d => d.ID) + 1 : 1;
                Details.Add(detail);

                RefreshDetail();
            }
        }

        private void RefreshDetail()
        {
            grdDetail.DataSource = Details;
            grdDetail.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEdit");
            if(Page.IsValid)
            {
                try
                {
                    InventoryMutationHeader header = RowID == 0
                        ? new InventoryMutationHeader()
                        : InventoryMutationService.GetInventoryMutation(RowID);

                    InventoryMutationService.AddOrUpdateInventoryMutation(header, Details);

                    ReloadCurrentPage();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                }
            }
        }
    }
}