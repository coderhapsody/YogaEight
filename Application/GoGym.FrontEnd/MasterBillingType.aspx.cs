using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.Constants;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterBillingType : BaseForm
    {
        [Inject]
        public BillingTypeProvider BillingTypeService { get; set; }

        [Inject]
        public CustomerProvider CustomerService { get; set; }


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
            txtDescription.Text = String.Empty;
            txtDescription.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                if (!id.Contains(1))
                    BillingTypeService.Delete(id);
                else
                    WebFormHelper.SetLabelTextWithCssClass(lblMessage, "<b>Manual Payment</b> cannot be deleted", LabelStyleNames.ErrorMessage);
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
                        BillingTypeService.Add(
                            txtDescription.Text,
                            Convert.ToInt16(txtAutoPayDay.Value),
                            chkIsActive.Checked);
                        break;
                    default:
                        if (chkIsActive.Checked == false && CustomerService.GetCustomersByBillingType(RowID).Any())
                        {
                            mvwForm.ActiveViewIndex = 0;
                            WebFormHelper.SetLabelTextWithCssClass(lblMessage, "Cannot set this billing type to inactive since there are customers use this billing type", LabelStyleNames.ErrorMessage);
                            return;
                        }
                        BillingTypeService.Update(
                            RowID,
                            txtDescription.Text,
                            Convert.ToInt16(txtAutoPayDay.Value),
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
                BillingType billingType = BillingTypeService.Get(id);
                txtDescription.Text = billingType.Description;
                txtAutoPayDay.Text = RowID > BillingTypeConstants.MANUAL_PAYMENT
                    ? Convert.ToString(billingType.AutoPayDay)
                    : "N/A";
                chkIsActive.Checked = billingType.IsActive;
                txtDescription.Focus();
                btnSave.Enabled = RowID > BillingTypeConstants.MANUAL_PAYMENT;
            }
        }

        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                var row = e.Row.DataItem as DataRowView;
                if (row != null)
                {
                    if (Convert.ToInt32(row["ID"]) == BillingTypeConstants.MANUAL_PAYMENT)
                    {
                        ((CheckBox) e.Row.FindControl("chkDelete")).Visible = false;
                        ((ImageButton) e.Row.FindControl("imbEdit")).Visible = false;
                    }
                }
            }
        }
    }
}