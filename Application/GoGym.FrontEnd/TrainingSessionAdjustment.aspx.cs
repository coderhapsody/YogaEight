using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class TrainingSessionAdjustment : BaseForm
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }

        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();

                btnProcess.Enabled = gvwSession.Rows.Count > 0;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var trainingSessionInfo = CustomerService.QueryTrainingSession(txtBarcode.Text, DateTime.Today);            
            ViewState["CustomerHomeBranchID"] = trainingSessionInfo.CustomerHomeBranchID;
            lblCustomerInfo.Text = String.Format("{0} - {1}",
                trainingSessionInfo.CustomerBarcode,
                trainingSessionInfo.CustomerName);
            gvwSession.DataSource = trainingSessionInfo.Invoices;
            gvwSession.DataBind();

            btnProcess.Enabled = gvwSession.Rows.Count > 0;
            btnSearch.Enabled = !(gvwSession.Rows.Count > 0);
            txtBarcode.Enabled = !(gvwSession.Rows.Count > 0);
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            var result = new StringBuilder();
            try
            {
                if (ValidateAdjustmentProcess())
                {
                    var customer = CustomerService.Get(txtBarcode.Text);
                    var branchID = Convert.ToInt32(hidBranchID.Value);                    

                    foreach (GridViewRow row in gvwSession.Rows)
                    {
                        var quotaID = Convert.ToInt32(row.Cells[0].Text);
                        var ddlType = row.FindControl("ddlType") as RadDropDownList;
                        var txtQtySession = row.FindControl("txtQtySession") as RadNumericTextBox;
                        var txtNotes = row.FindControl("txtNotes") as RadTextBox;
                        var ddlTrainer = row.FindControl("ddlTrainer") as RadDropDownList;                        
                        var itemID = Convert.ToInt32(row.Cells[1].Text);
                        var itemBarcode = row.Cells[2].Text;
                        var itemDescription = row.Cells[3].Text;

                        if (ddlType != null && txtQtySession != null && ddlTrainer != null && txtNotes != null)
                        {
                            int qty = Convert.ToInt32(txtQtySession.Value.GetValueOrDefault());
                            if (qty > 0)
                            {
                                try
                                {
                                    if (ddlType.SelectedValue == "Sub")
                                    {
                                        CustomerService.ProcessCutTrainingSession(
                                            quotaID,
                                            branchID,
                                            Convert.ToInt32(ddlTrainer.SelectedValue),
                                            customer.Barcode,
                                            Convert.ToInt16(txtQtySession.Value.GetValueOrDefault()),
                                            txtNotes.Text);

                                        result.AppendLine(
                                            String.Format(
                                                "Cutting session for {0} has been done for {1} session(s) <br/>",
                                                itemBarcode,
                                                Convert.ToInt32(txtQtySession.Value.GetValueOrDefault())));
                                    }
                                    else if (ddlType.SelectedValue == "Add")
                                    {
                                        CustomerService.AdjustTrainingSession(
                                            quotaID,
                                            branchID,
                                            Convert.ToInt32(ddlTrainer.SelectedValue),
                                            customer.Barcode,
                                            DateTime.Today,
                                            Convert.ToInt16(txtQtySession.Value.GetValueOrDefault()),
                                            txtNotes.Text);

                                        result.AppendLine(
                                            String.Format(
                                                "Adjustment session for {0} has been done for {1} session(s) <br/>",
                                                itemBarcode,
                                                Convert.ToInt32(txtQtySession.Value.GetValueOrDefault())));
                                    }


                                }
                                catch (Exception ex)
                                {
                                    result.AppendLine("Cutting session for {0} failed. <br/>");
                                    LogService.ErrorException(GetType().FullName, ex);
                                }
                            }
                        }
                    }

                    WebFormHelper.SetLabelTextWithCssClass(lblResult, result.ToString(), LabelStyleNames.InfoMessage);
                }
            }
            catch (Exception ex)
            {
                LogService.ErrorException(GetType().FullName, ex);                
            }
        }

        private bool ValidateAdjustmentProcess()
        {
            return true;
            //throw new NotImplementedException();
        }

        protected void gvwSession_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ddlTrainer = (RadDropDownList) e.Row.FindControl("ddlTrainer");
                var customerHomeBranchID = Convert.ToInt32(ViewState["CustomerHomeBranchID"]);                
                ddlTrainer.DataSource = EmployeeService.GetTrainers(customerHomeBranchID);
                ddlTrainer.DataTextField = "FirstName";
                ddlTrainer.DataValueField = "ID";
                ddlTrainer.DataBind();
            }

            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.HideGridViewRowId(e, 1);
        }

        protected void gvwSession_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hypQuotaDetail = (HyperLink) e.Row.FindControl("hypQuotaDetail");
                hypQuotaDetail.Attributes.Add(
                    "onclick",
                    String.Format("window.showModalDialog('TrainingSessionDetail.aspx?QuotaID={0}', null, 'dialogHeight=550px;dialogWidth=900px;resizable=no;');",
                        e.Row.Cells[0].Text));
            }
        }
    }
}