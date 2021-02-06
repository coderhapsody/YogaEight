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
    public partial class MasterInstructor : BaseForm
    {
        [Inject]
        public InstructorProvider InstructorService { get; set; }

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
            txtCellPhone.Text = txtHomePhone.Text = txtEmail.Text = String.Empty;
            txtBarcode.Focus();
            calDate.SelectedDate = DateTime.Today;            
            chkIsActive.Checked = true;
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            foreach (var _id in id)
                InstructorService.DeleteInstructor(_id);
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
                        InstructorService.AddInstructor(
                            txtBarcode.Text,
                            txtName.Text,
                            calDate.SelectedDate.GetValueOrDefault(),
                            ddlStatus.SelectedValue,
                            txtEmail.Text,
                            txtHomePhone.Text,
                            txtCellPhone.Text,
                            chkIsActive.Checked);
                        break;
                    default:
                        InstructorService.UpdateInstructor(
                            RowID,
                            txtBarcode.Text,
                            txtName.Text,
                            calDate.SelectedDate.GetValueOrDefault(),
                            ddlStatus.SelectedValue,
                            txtEmail.Text,
                            txtHomePhone.Text,
                            txtCellPhone.Text,
                            chkIsActive.Checked);
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
                Instructor inst = InstructorService.GetInstructor(id);
                txtBarcode.Text = inst.Barcode;
                txtName.Text = inst.Name;
                txtHomePhone.Text = inst.HomePhone;
                txtCellPhone.Text = inst.CellPhone;
                txtEmail.Text = inst.Email;
                ddlStatus.SelectedValue = Convert.ToString(inst.Status);
                chkIsActive.Checked = inst.IsActive;
                txtName.Focus();
            }
        }
        protected void sqldsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }
    }
}