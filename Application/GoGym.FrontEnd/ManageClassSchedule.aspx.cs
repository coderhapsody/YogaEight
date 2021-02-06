using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ManageClassSchedule : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public ClassProvider ClassService { get; set; }
        [Inject]
        public InstructorProvider InstructorService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead1);

                DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, User.Identity.Name, false);
                DynamicControlBinding.BindDropDown(ddlMonth, CommonHelper.GetMonthNames(), "Value", "Key", false);
                DynamicControlBinding.BindDropDown(ddlDayOfWeek, CommonHelper.GetDayNames(), "Value", "Key", false);
                DynamicControlBinding.BindDropDown(ddlInstructor, InstructorService.GetActiveInstructors(), "Name", "ID", false);
                for (int year = DateTime.Today.Year - 1; year <= DateTime.Today.Year; year++)
                    ddlYear.Items.Add(new DropDownListItem(year.ToString(), year.ToString()));

                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlDayOfWeek.SelectedValue = DateTime.Today.DayOfWeek.ToString();

                ddlBranch.Enabled = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClassService.AddSchedule(
                    Convert.ToInt32(ddlDayOfWeek.SelectedValue),
                    Convert.ToInt32(ddlBranch.SelectedValue),
                    Convert.ToInt32(ddlYear.SelectedValue),
                    Convert.ToInt32(ddlMonth.SelectedValue),
                    Convert.ToInt32(ddlClass.SelectedValue),
                    ddlLevel.SelectedValue,
                    Convert.ToInt32(ddlClassRoom.SelectedValue),
                    ddlTimeStart.SelectedItem.Text.Split('-')[0],
                    ddlTimeStart.SelectedItem.Text.Split('-')[1],
                    Convert.ToInt32(ddlInstructor.SelectedValue));
                gvwDetail.DataBind();
                ddlTimeStart.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "_error", "alert('" + ex.Message + "')", true);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
        protected void btnSelectOtherPeriod_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwRead1);
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            lblBranch.Text = ddlBranch.SelectedItem.Text;
            lblPeriod.Text = ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
            ddlDayOfWeek.SelectedValue = Convert.ToString((int)DateTime.Today.DayOfWeek == 0 ? 7 : (int)DateTime.Today.DayOfWeek);
            DynamicControlBinding.BindDropDown(ddlClass, ClassService.GetAllActiveClasses().ToList(), "Name", "ID", false);
            DynamicControlBinding.BindDropDown(ddlClassRoom, ClassService.GetActiveClassRooms(Convert.ToInt32(ddlBranch.SelectedValue)).ToList(), "Name", "ID", false);
            ddlTimeStart.DataSource = ClassService.GetTimeSlots(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlDayOfWeek.SelectedValue));
            ddlTimeStart.DataBind();
            gvwDetail.DataBind();
            Form.DefaultButton = btnSave.UniqueID;
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@Year"].Value = ddlYear.SelectedValue;
            e.Command.Parameters["@Month"].Value = ddlMonth.SelectedValue;
            e.Command.Parameters["@DayOfWeek"].Value = ddlDayOfWeek.SelectedValue;
        }
        protected void btnRefreshDayOfWeek_Click(object sender, EventArgs e)
        {
            gvwDetail.DataBind();
        }
        protected void gvwDetail_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DynamicControlBinding.HideGridViewRowId(0, e);
        }
        protected void gvwDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                if (ClassService.VerifyScheduleDeletion(id))
                {
                    ClassService.DeleteSchedule(id);
                    gvwDetail.DataBind();
                }
                else
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblStatusDelete,
                        "Cannot delete schedule because it has attendees, please clear all of the attendees first to delete this schedule.",
                        LabelStyleNames.ErrorMessage);
                }
            }
            else if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                mvwForm.ActiveViewIndex = 2;
                DynamicControlBinding.BindDropDown(ddlInstructor0, InstructorService.GetActiveInstructors(), "Name", "ID", false);
                DynamicControlBinding.BindDropDown(ddlClass0, ClassService.GetAllActiveClasses().ToList(), "Name", "ID", false);
                DynamicControlBinding.BindDropDown(ddlClassRoom0, ClassService.GetActiveClassRooms(Convert.ToInt32(ddlBranch.SelectedValue)).ToList(), "Name", "ID", false);
                ddlTimeStart0.DataSource = ClassService.GetTimeSlots(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlDayOfWeek.SelectedValue));
                ddlTimeStart0.DataBind();

                ClassScheduleDetail schedule = ClassService.GetSchedule(id);
                if (schedule != null)
                {
                    lblBranch0.Text = ddlBranch.SelectedItem.Text;
                    lblPeriod0.Text = ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
                    lblDayOfWeek0.Text = ddlDayOfWeek.SelectedItem.Text;
                    ddlClass0.SelectedValue = schedule.ClassID.ToString();
                    ddlClassRoom0.SelectedValue = schedule.ClassRoomID.ToString();
                    ddlInstructor0.SelectedValue = schedule.InstructorID.ToString();
                    ddlTimeStart0.FindItemByText(schedule.TimeStart + "-" + schedule.TimeEnd).Selected = true;
                    ddlLevel0.SelectedValue = schedule.Level;
                    ViewState["ScheduleID"] = id;
                }
                Form.DefaultButton = btnUpdate.UniqueID;

            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            var fi = new FileInfo(fupFile.FileName);
            if (fi.Extension.ToUpper() != ".XLS")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "_error", "alert('Not excel file ');", true);
                return;
            }
            fupFile.SaveAs(Server.MapPath("~/Temp/") + fupFile.FileName);
            try
            {
                ClassService.UploadFromExcel(
                    Convert.ToInt32(ddlBranch.SelectedValue),
                    Convert.ToInt32(ddlYear.SelectedValue),
                    Convert.ToInt32(ddlMonth.SelectedValue),
                    Server.MapPath("~/Temp/") + fupFile.FileName);
                ClientScript.RegisterStartupScript(this.GetType(), "_error", "alert('Schedules uploaded successfully');", true);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblUploadError, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
        protected void ddlDayOfWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTimeStart.DataSource = ClassService.GetTimeSlots(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlDayOfWeek.SelectedValue));
            ddlTimeStart.DataBind();
        }
        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {

            mvwForm.ActiveViewIndex = 1;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(ViewState["ScheduleID"]);
                ClassService.UpdateSchedule(id,
                    Convert.ToInt32(ddlClass0.SelectedValue),
                    Convert.ToInt32(ddlClassRoom0.SelectedValue),
                    Convert.ToInt32(ddlInstructor0.SelectedValue),
                    ddlLevel0.SelectedValue);
                mvwForm.ActiveViewIndex = 1;
                gvwDetail.DataBind();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatusUpdate,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnCopyFromLastMonth_Click(object sender, EventArgs e)
        {
            var currentPeriod = new DateTime(
                Convert.ToInt32(ddlYear.SelectedValue),
                Convert.ToInt32(ddlMonth.SelectedValue),
                1);

            try
            {
                ClassService.CopyScheduleFromLastMonth(
                    Convert.ToInt32(ddlBranch.SelectedValue),
                    currentPeriod.Year,
                    currentPeriod.Month);

                gvwDetail.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", String.Format("alert('{0}');", ex.Message), true);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
    }
}