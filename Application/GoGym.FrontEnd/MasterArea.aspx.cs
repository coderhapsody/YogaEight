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
    public partial class MasterArea : BaseForm
    {
        [Inject]
        public AreaProvider AreaService { get; set; }        

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
            WebFormHelper.ClearTextBox(txtDescription);
            txtDescription.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                AreaService.Delete(id);
                Refresh();
            }
            catch (Exception ex)
            {
                mvwForm.ActiveViewIndex = 0;
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
            if (Page.IsValid)
            {
                try
                {
                    switch (RowID)
                    {
                        case 0:
                            AreaService.Add(
                                txtDescription.Text);
                            break;
                        default:
                            AreaService.Update(
                                RowID,
                                txtDescription.Text);
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
                Area area = AreaService.Get(id);
                txtDescription.Text = area.Description;
                txtDescription.Focus();
            }
        }
    }
}