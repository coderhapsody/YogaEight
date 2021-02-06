using System;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class MasterCustomerStatus : BaseForm
    {
        [Inject]
        public CustomerStatusProvider CustomerStatusService { get; set; }


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
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            CustomerStatusService.Delete(id);
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
                        CustomerStatusService.Add(
                            txtDescription.Text,
                            ColorTranslator.ToHtml(colColor.SelectedColor),
                            ColorTranslator.ToHtml(colBgColor.SelectedColor));
                        break;
                    default:
                        CustomerStatusService.Update(
                            RowID,
                            txtDescription.Text,
                            ColorTranslator.ToHtml(colColor.SelectedColor),
                            ColorTranslator.ToHtml(colBgColor.SelectedColor));
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
                var customerStatus = CustomerStatusService.Get(id);
                txtDescription.Text = customerStatus.Description;
                if (!String.IsNullOrEmpty(customerStatus.Color))
                {
                    try
                    {

                        colColor.SelectedColor = ColorTranslator.FromHtml(customerStatus.Color.Split('|')[0]);
                        colBgColor.SelectedColor = ColorTranslator.FromHtml(customerStatus.Color.Split('|')[1]);
                    }
                    catch (Exception ex)
                    {
                        LogService.ErrorException(GetType().FullName, ex);
                    }
                }
                txtDescription.Focus();
            }
        }
    }
}