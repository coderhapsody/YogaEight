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
    public partial class MasterBranch : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
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
            WebFormHelper.ClearTextBox(txtName, txtAddress, txtCode, txtEmail, txtMerchantCode, txtPhone);

            txtName.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                BranchService.Delete(id);
                Refresh();
            }
            catch (Exception ex)
            {
                mvwForm.ActiveViewIndex = 0;
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
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
                        BranchService.Add(
                            txtCode.Text.ToUpper(),
                            txtName.Text,
                            txtAddress.Text,
                            txtPhone.Text,
                            txtEmail.Text,
                            txtMerchantCode.Text,
                            chkIsActive.Checked);
                        break;
                    default:
                        BranchService.Update(
                            RowID,
                            txtCode.Text.ToUpper(),
                            txtName.Text,
                            txtAddress.Text,
                            txtPhone.Text,
                            txtEmail.Text,
                            txtMerchantCode.Text,
                            chkIsActive.Checked);
                        break;
                }
                Refresh();
            }
            catch (Exception ex)
            {
                mvwForm.ActiveViewIndex = 0;
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
                Branch branch = BranchService.Get(id);
                txtCode.Text = branch.Code;
                txtName.Text = branch.Name;
                txtAddress.Text = branch.Address;
                txtEmail.Text = branch.Email;
                txtPhone.Text = branch.Phone;
                txtMerchantCode.Text = branch.MerchantCode;
                chkIsActive.Checked = branch.IsActive;
                txtName.Focus();
            }
        }
    }
}