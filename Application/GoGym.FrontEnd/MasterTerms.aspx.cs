using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class MasterTerms : BaseForm
    {
        [Inject]
        public PurchaseOrderProvider PurchaseOrderService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                RadHelper.SetUpGrid(RadGrid1);
            }
        }   

        protected void gvwMaster_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void lnbAddNew_OnClick(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            txtName.Text = String.Empty;
            chkIsActive.Checked = true;
            txtName.Focus();
        }

        protected void lnbDelete_OnClick(object sender, EventArgs e)
        {
            int[] arrayOfID = RadHelper.GetRowIdForDeletion(RadGrid1);
            if (PurchaseOrderService.CanDeleteTerms(arrayOfID))
            {
                PurchaseOrderService.DeleteTerms(arrayOfID);
                btnCancel_OnClick(sender, e);
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            Page.Validate(WebFormHelper.AddEditValidationGroup);

            if (!Page.IsValid) return;

            try
            {
                PurchaseOrderService.AddOrUpdateTerm(RowID, txtName.Text, chkIsActive.Checked);
                btnCancel_OnClick(sender, e);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void RadGrid1_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                mvwForm.SetActiveView(viwAddEdit);
                RowID = Convert.ToInt32(e.CommandArgument);
                var term = PurchaseOrderService.GetTerm(RowID);
                txtName.Text = term.Name;
                chkIsActive.Checked = term.IsActive;
                txtName.Focus();
            }
        }
    }
}