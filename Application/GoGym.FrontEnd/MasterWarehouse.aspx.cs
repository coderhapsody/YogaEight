using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterWarehouse : BaseForm
    {
        [Inject]
        public WarehouseProvider WarehouseService { get; set; }

        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
                DataBindingHelper.PopulateActiveBranches(BranchService, ddlFindBranch, false);
            }
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, true);
            txtName.Text = String.Empty;
            txtCode.Text = String.Empty;
            chkIsActive.Checked = true;
            RowID = 0;
            ddlBranch.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                Array.ForEach(id, wh => WarehouseService.DeleteWarehouse(wh));
                ReloadCurrentPage();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
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
                    WarehouseService.AddOrUpdate(RowID,
                        Convert.ToInt32(ddlBranch.SelectedValue),
                        txtCode.Text,
                        txtName.Text,
                        chkIsActive.Checked);
                    ReloadCurrentPage();
                }
                catch (Exception ex)
                {
                    mvwForm.ActiveViewIndex = 0;
                    WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                var warehouse = WarehouseService.GetWarehouse(id);
                DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, false);
                ddlBranch.SelectedValue = Convert.ToString(warehouse.BranchID);
                txtCode.Text = warehouse.Code;
                txtName.Text = warehouse.Name;
                chkIsActive.Checked = warehouse.IsActive;
                txtName.Focus();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();            
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        }
    }
}