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
    public partial class MasterClassRoom : BaseForm
    {
        [Inject]
        public ClassProvider ClassService { get; set; }

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
            txtName.Text = String.Empty;
            txtCode.Text = String.Empty;
            chkIsActive.Checked = true;
            Form.DefaultButton = btnSave.UniqueID;
            RowID = 0;
            txtCode.Focus();            
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                ClassService.DeleteClassRoom(id);
                Refresh();
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
                    switch (RowID)
                    {
                        case 0:
                            ClassService.AddRoom(
                                txtCode.Text,
                                txtName.Text,
                                chkIsActive.Checked);
                            break;
                        default:
                            ClassService.UpdateRoom(
                                RowID,
                                txtCode.Text,
                                txtName.Text,
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
        }

        private void Refresh()
        {
            ReloadCurrentPage();
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                ClassRoom room = ClassService.GetRoom(id);
                txtCode.Text = room.Code;
                txtName.Text = room.Name;
                chkIsActive.Checked = room.IsActive;
                txtName.Focus();
            }
        }
    }
}