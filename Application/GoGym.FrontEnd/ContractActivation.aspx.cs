using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class ContractActivation : BaseForm
    {
        [Inject]
        public ContractProvider ContractService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
                ddlFindBranch.DataTextField = "Name";
                ddlFindBranch.DataValueField = "ID";
                ddlFindBranch.DataBind();

                calContractDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calContractDateTo.SelectedDate = DateTime.Today;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvwMaster.Rows)
            {
                string contractNo = row.Cells[1].Text;
                CheckBox chkActivate = row.FindControl("chkActivate") as CheckBox;
                if (chkActivate != null && !String.IsNullOrEmpty(contractNo))
                {
                    if (row.Enabled)
                    {
                        if(chkActivate.Checked)
                        {
                            ContractService.ActivateContract(contractNo);
                        }
                        else
                            ContractService.DeActivateContract(contractNo);
                    }
                }
                
            }
            
            gvwMaster.DataBind();
        }

        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var row = e.Row.DataItem as DataRowView;
                if (row != null)
                {
                    var chkActivate = e.Row.FindControl("chkActivate") as CheckBox;

                    if (chkActivate != null)
                    {
                        chkActivate.Checked = !(row["ActiveDate"] is DBNull);
                    }

                    e.Row.Enabled = Convert.ToString(row["Status"]) != "Closed";
                }
            }
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void sqldsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@ContractDateFrom"].Value = calContractDateFrom.SelectedDate.GetValueOrDefault();
            e.Command.Parameters["@ContractDateTo"].Value = calContractDateTo.SelectedDate.GetValueOrDefault();
            e.Command.Parameters["@CustomerName"].Value = txtCustomerFirstName.Text;
        }
    }
}