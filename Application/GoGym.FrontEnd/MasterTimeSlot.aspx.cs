using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterTimeSlot : BaseForm
    {
        [Inject]
        public ClassProvider ClassService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.ActiveViewIndex = 0;
                DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, User.Identity.Name, false);
                DynamicControlBinding.BindDropDown(ddlDayOfWeek, CommonHelper.GetDayNames(), "Value", "Key", false);
                ddlDayOfWeek.SelectedValue = (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek).ToString(CultureInfo.InvariantCulture);

                ddlBranch.Enabled = false;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            mvwForm.ActiveViewIndex = 1;
            gvwData.DataBind();
        }

        protected void gvwData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DynamicControlBinding.HideGridViewRowId(0, e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClassService.AddTimeSlot(
                    Convert.ToInt32(ddlBranch.SelectedValue),
                    Convert.ToInt32(ddlDayOfWeek.SelectedValue),
                    txtTimeStart.TextWithLiterals + "-" + txtTimeEnd.TextWithLiterals);
                gvwData.DataBind();
                txtTimeStart.Text = String.Empty;
                txtTimeEnd.Text = String.Empty;
                txtTimeStart.Focus();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwData);
            ClassService.DeleteTimeSlot(id);
            gvwData.DataBind();
        }
        protected void sdsTimeSlot_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@DayOfWeek"].Value = ddlDayOfWeek.SelectedValue;
        }
    }
}