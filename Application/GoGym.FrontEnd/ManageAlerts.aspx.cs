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
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ManageAlerts : BaseForm
    {
        [Inject]
        public AlertProvider AlertService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlFilter.SelectedIndex = 0;
                mvwForm.SetActiveView(viwRead);
                RadHelper.SetUpGrid(RadGrid1);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEdit");
            if (Page.IsValid)
            {
                try
                {
                    AlertService.AddOrUpdateAlert(
                        RowID,
                        txtSubject.Text,
                        txtDescription.Text,
                        CalendarPopup1.SelectedDate.GetValueOrDefault(DateTime.Today),
                        chkInfinite.Checked
                            ? (DateTime?) null
                            : CalendarPopup2.SelectedDate.GetValueOrDefault(DateTime.Today),
                        chkActive.Checked,
                        RadColorPicker1.SelectedColor.ToArgb());                    

                    ReloadCurrentPage();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);  
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            txtSubject.Text = String.Empty;
            txtDescription.Text = String.Empty;
            CalendarPopup1.SelectedDate = DateTime.Today;
            CalendarPopup2.SelectedDate = DateTime.Today;
            chkActive.Checked = true;
            chkInfinite.Checked = false;
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            int[] arrayOfID = RadHelper.GetRowIdForDeletion(RadGrid1);

            try
            {
                Array.ForEach(arrayOfID, id => AlertService.DeleteAlert(id)); 
                ReloadCurrentPage();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);                
            }            
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditRow"))
            {
                RowID = Convert.ToInt32(e.CommandArgument);
                mvwForm.SetActiveView(viwAddEdit);

                var entity = AlertService.GetAlert(RowID);
                txtSubject.Text = entity.Subject;
                txtDescription.Text = entity.Description;
                CalendarPopup1.SelectedDate = entity.StartDate;
                chkInfinite.Checked = !entity.EndDate.HasValue;
                CalendarPopup2.SelectedDate = entity.EndDate.HasValue ? entity.EndDate : null;
                chkActive.Checked = entity.Active;
                RadColorPicker1.SelectedColor = Color.FromArgb(entity.BackColor.GetValueOrDefault(-1));
                txtDescription.Focus();
            }
        }

        protected void RadGrid1_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            RadGrid1.CurrentPageIndex = e.NewPageIndex;
            RadGrid1.DataBind();
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@PageIndex"].Value = RadGrid1.CurrentPageIndex + 1;
            e.Command.Parameters["@PageSize"].Value = RadGrid1.PageSize;
            e.Command.Parameters["@RecordCount"].Value = 0;
            e.Command.Parameters["@ShowOnlyActiveAlerts"].Value = ddlFilter.SelectedValue;
        }

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected DateTime? SetStartDate(GridItem item)
        {
            return item.OwnerTableView.GetColumn("StartDate").CurrentFilterValue == string.Empty ? new DateTime?() : DateTime.Parse(item.OwnerTableView.GetColumn("StartDate").CurrentFilterValue);
        }

        protected DateTime? SetEndDate(GridItem item)
        {
            return item.OwnerTableView.GetColumn("EndDate").CurrentFilterValue == string.Empty ? new DateTime?() : DateTime.Parse(item.OwnerTableView.GetColumn("EndDate").CurrentFilterValue);
        }
    }
}