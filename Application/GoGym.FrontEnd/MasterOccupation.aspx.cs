using System;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterOccupation : BaseForm
    {
        [Inject]
        public OccupationProvider OccupationService { get; set; }

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
            OccupationService.Delete(id);
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
                        OccupationService.Add(
                            txtDescription.Text);
                        break;
                    default:
                        OccupationService.Update(
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
                var itemType = OccupationService.Get(id);
                txtDescription.Text = itemType.Description;
                txtDescription.Focus();
            }
        }

        protected void odsOccupation_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = OccupationService;
        }
    }
}