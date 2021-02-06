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
using GoGym.Utilities.Extensions;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterDocumentType : BaseForm
    {
        [Inject]
        public DocumentTypeProvider DocumentTypeService { get; set; }
        [Inject]
        public CustomerStatusProvider CustomerStatusService{ get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
                DataBindingHelper.PopulateCustomerStatus(CustomerStatusService, ddlCustomerStatus, true);
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
            RowID = 0;
            txtDescription.Text = String.Empty;
            txtDescription.Focus();
            ddlCustomerStatus.SelectedIndex = 0;
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            DocumentTypeService.Delete(id);
            Refresh();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                switch (RowID)
                {
                    case 0:
                        DocumentTypeService.Add(
                            txtDescription.Text,
                            chkIsLastState.Checked,
                            ddlCustomerStatus.SelectedValue.ToDefaultNumber<int>());
                        break;
                    default:
                        DocumentTypeService.Update(
                            RowID,
                            txtDescription.Text,
                            chkIsLastState.Checked,
                            ddlCustomerStatus.SelectedValue.ToDefaultNumber<int>());
                        break;
                }
                Refresh();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        private void Refresh()
        {
            mvwForm.SetActiveView(viwRead);
            gvwMaster.DataBind();
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                DocumentType docType = DocumentTypeService.Get(id);
                txtDescription.Text = docType.Description;
                chkIsLastState.Checked = docType.IsLastState;
                ddlCustomerStatus.SelectedValue = !docType.ChangeCustomerStatusIDTo.HasValue ? String.Empty : docType.ChangeCustomerStatusIDTo.Value.ToString();
                txtDescription.Focus();
            }
        }
    }
}