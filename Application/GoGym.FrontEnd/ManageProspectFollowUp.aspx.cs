using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ManageProspectFollowUp : BaseForm
    {
        [Inject]
        public ProspectProvider ProspectService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            RowID = Convert.ToInt32(Request.QueryString["ProspectID"]);
            if (!Page.IsPostBack)
            {
                FillDropDown();
                RadHelper.SetUpGrid(grdMaster);
                dtpDate.SelectedDate = DateTime.Today;
                dtpDate.Enabled = false;

                var prospect = ProspectService.GetProspect(RowID);
                lblProspectName.Text = String.Format("{0} {1}", prospect.FirstName, prospect.LastName);
            }
        }

        private void FillDropDown()
        {
            var followUpVias = ProspectService.GetFollowUpVias();
            ddlFollowUpVia.Items.Clear();
            ddlFollowUpVia.Items.Add(new DropDownListItem(String.Empty));
            foreach (var followUpVia in followUpVias)
                ddlFollowUpVia.Items.Add(new DropDownListItem(followUpVia, followUpVia));

            var outcomes = ProspectService.GetFollowUpOutcomes();
            ddlOutcome.Items.Clear();
            ddlOutcome.Items.Add(new DropDownListItem(String.Empty));
            foreach (var outcome in outcomes)
                ddlOutcome.Items.Add(new DropDownListItem(outcome, outcome));    
            
            
        }

        protected void btnAddFollowUp_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                try
                {
                    ProspectService.AddOrUpdateFollowUp(
                        0,
                        Convert.ToInt32(Request.QueryString["ProspectID"]),
                        dtpDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                        ddlFollowUpVia.SelectedValue,
                        txtResult.Text,
                        ddlOutcome.SelectedValue);

                    grdMaster.DataBind();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }

        protected void grdMaster_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                try
                {
                    int followUpID = Convert.ToInt32(e.CommandArgument);
                    ProspectService.DeleteFollowUp(followUpID);
                    grdMaster.DataBind();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
    }
}