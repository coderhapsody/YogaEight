using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.Providers;
using log4net.Repository.Hierarchy;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class Default : BaseForm
    {
        [Inject]
        public FitnessEntities Entities { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public AlertProvider AlertService { get; set; }

        [Inject]
        public CustomerProvider CustomerService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblBrowser.Text = String.Format("{0} - {1} {2}", Request.Browser.Type, Request.Browser.Version, Request.Browser.Platform);
            using (var entities = new FitnessEntities())
            {
                using (var connection = entities.Connection)
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    lblDatabaseServer.Text = String.Format("{0} on {1} ({2})",
                        entities.Connection.Database,
                        entities.Connection.DataSource,
                        entities.Connection.ServerVersion);
                }
            }
            bulBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            bulBranch.DataTextField = "Name";
            bulBranch.DataBind();

            bulAllowedBranch.DataSource = BranchService.GetAllowedBranches(User.Identity.Name);
            bulAllowedBranch.DataTextField = "Name";
            bulAllowedBranch.DataBind();

            hypAlerts.Visible = !String.IsNullOrEmpty(Request["FromAlert"]);
            
            if (!Page.IsPostBack)
            {
                RadScheduler1.SelectedDate = DateTime.Today;
                BindData();
            }
        }

        protected void RadScheduler1_AppointmentDataBound(object sender, Telerik.Web.UI.SchedulerEventArgs e)
        {
            var alert = e.Appointment.DataItem as Alert;
            if (alert != null)
            {
                e.Appointment.BackColor = alert.BackColor != null
                    ? Color.FromArgb(alert.BackColor.GetValueOrDefault())
                    : Color.White;
                e.Appointment.Description = alert.Description;
            }
        }

        private void BindData()
        {
            RadScheduler1.DataSource = AlertService.GetAlerts();
        }
    }
}