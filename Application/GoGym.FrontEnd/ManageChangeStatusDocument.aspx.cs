using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ManageChangeStatusDocument : BaseForm
    {
        [Inject]
        public DocumentProvider DocumentService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public UserProvider UserService { get; set; }
        [Inject]
        public CustomerProvider CustomerService { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                calFindFromDate.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calFindToDate.SelectedDate = DateTime.Today;

                ddlDocumentType.DataSource = DocumentService.GetAllDocumentTypes();
                ddlDocumentType.DataTextField = "Description";
                ddlDocumentType.DataValueField = "ID";
                ddlDocumentType.DataBind();
                ddlDocumentType.SelectedIndex = 0;

                ddlFindDocumentType.DataSource = DocumentService.GetAllDocumentTypes();
                ddlFindDocumentType.DataTextField = "Description";
                ddlFindDocumentType.DataValueField = "ID";
                ddlFindDocumentType.DataBind();
                ddlFindDocumentType.Items.Insert(0, new DropDownListItem(String.Empty, "0"));

                ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
                ddlFindBranch.DataTextField = "Name";
                ddlFindBranch.DataValueField = "ID";
                ddlFindBranch.DataBind();
                ddlFindBranch.SelectedIndex = 0;
                ddlFindBranch.Enabled = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEdit");
            if (Page.IsValid)
            {
                int documentTypeID = Convert.ToInt32(ddlDocumentType.SelectedValue);
                try
                {
                    if (DocumentService.CanChangeStatus(txtCustomerCode.Text))
                    {

                        switch (RowID)
                        {
                            case 0:
                                DocumentService.Add(
                                    Convert.ToInt32(ddlFindBranch.SelectedValue),
                                    DateTime.Today,
                                    calStartDate.SelectedDate.GetValueOrDefault(),
                                    chkEndDate.Checked ? calEndDate.SelectedDate : (DateTime?)null,
                                    txtCustomerCode.Text,
                                    documentTypeID,
                                    txtNotes.Text);
                                break;
                            default:
                                DocumentService.Update(
                                    RowID,
                                    DateTime.Today,
                                    calStartDate.SelectedDate.GetValueOrDefault(),
                                    chkEndDate.Checked ? calEndDate.SelectedDate : (DateTime?)null,
                                    txtCustomerCode.Text,
                                    documentTypeID,
                                    txtNotes.Text);
                                break;
                        }

                        mvwForm.ActiveViewIndex = 0;
                        gvwMaster.DataBind();
                    }
                    else
                    {
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblStatus,
                            "Cannot change customer status for any unpaid contract",
                            LabelStyleNames.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblStatus,
                        ex.Message,
                        LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvwForm.ActiveViewIndex = 0;
            gvwMaster.DataBind();
        }
        protected void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = DocumentService.GetChangeStatusDocument(RowID);
                if (doc != null)
                {
                    DocumentService.Void(RowID);
                    ClientScript.RegisterStartupScript(
                        GetType(),
                        "_notification",
                        String.Format("alert('Document No. {0} has been marked as VOID');", doc.DocumentNo),
                        true);
                }
                btnSave.Enabled = false;
                btnApprove.Enabled = false;
                btnVoid.Enabled = false;
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatus,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
        protected void lnbDelete_Click(object sender, EventArgs e)
        {

        }
        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.ActiveViewIndex = 1;
            RowID = 0;
            lblBranch.Text = ddlFindBranch.SelectedItem.Text;
            WebFormHelper.ClearTextBox(txtCustomerCode, txtNotes);
            ddlDocumentType.Enabled = true;
            ddlDocumentType.SelectedIndex = 0;
            calStartDate.SelectedDate = DateTime.Today;
            calEndDate.SelectedDate = DateTime.Today;
            btnApprove.Enabled = false;
            btnVoid.Enabled = false;
            lblApprovalStatus.Text = "Not Approved";
            lblDocumentNo.Text = "(Generated by System)";

            if (String.IsNullOrEmpty(hypPromptCustomer.Attributes["onclick"]))
            {
                hypPromptCustomer.Attributes["onclick"] =
                    String.Format("showPromptPopUp('PromptCustomer.aspx?BranchID={0}', '{1}', 550, 900);",
                        ddlFindBranch.SelectedValue,
                        txtCustomerCode.ClientID);
            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = DocumentService.GetChangeStatusDocument(RowID);
                if (doc != null)
                {
                    DocumentService.Approve(RowID);
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "_notification",
                        String.Format("alert('Document No. {0} has been marked as APPROVED');", doc.DocumentNo),
                        true);
                    lblApprovalStatus.Text = "Approved";
                }
                mvwForm.ActiveViewIndex = 0;
                gvwMaster.DataBind();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatus,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
            }
        }
        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditRow"))
            {
                mvwForm.ActiveViewIndex = 1;
                RowID = Convert.ToInt32(e.CommandArgument);
                ddlDocumentType.Enabled = false;
                var doc = DocumentService.GetChangeStatusDocument(RowID);
                lblBranch.Text = doc.Branch.Name;
                txtCustomerCode.Text = doc.Customer.Barcode;
                txtNotes.Text = doc.Notes;
                ddlDocumentType.SelectedValue = doc.DocumentTypeID.ToString();
                calStartDate.SelectedDate = doc.StartDate;
                chkEndDate.Checked = doc.EndDate.HasValue;
                if (doc.EndDate.HasValue)
                    calEndDate.SelectedDate = doc.EndDate.Value;
                else
                    calEndDate.Clear();
                lblDocumentNo.Text = String.Format("{0} - {1}", doc.DocumentNo, doc.Date.ToLongDateString());

                if (doc.VoidDate.HasValue)
                    lblApprovalStatus.Text = "Void";
                else if (doc.ApprovedDate.HasValue)
                    lblApprovalStatus.Text = "Approved";
                else
                    lblApprovalStatus.Text = "Not Approved";

                btnVoid.Enabled = !doc.ApprovedDate.HasValue && !doc.VoidDate.HasValue;
                btnApprove.Enabled = EmployeeService.CanApproveDocument(User.Identity.Name);
                btnApprove.Enabled = !doc.ApprovedDate.HasValue && !doc.VoidDate.HasValue && RowID > 0;
                btnSave.Enabled = !doc.VoidDate.HasValue && !doc.ApprovedDate.HasValue;

                if (String.IsNullOrEmpty(hypPromptCustomer.Attributes["onclick"]))
                {
                    hypPromptCustomer.Attributes["onclick"] =
                    String.Format("showPromptPopUp('PromptCustomer.aspx?BranchID={0}', '{1}', 550, 900);",
                        ddlFindBranch.SelectedValue,
                        txtCustomerCode.ClientID);
                }
            }
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@DocumentTypeID"].Value = ddlFindDocumentType.SelectedValue;
            e.Command.Parameters["@DateFrom"].Value = calFindFromDate.SelectedDate.GetValueOrDefault().ToString("yyyy-MM-dd");
            e.Command.Parameters["@DateTo"].Value = calFindToDate.SelectedDate.GetValueOrDefault().ToString("yyyy-MM-dd");
            e.Command.Parameters["@CustomerCode"].Value = txtFindCustomerCode.Text;
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void cuvCustomerCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !String.IsNullOrEmpty(args.Value) && CustomerService.IsExist(args.Value);
        }
        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var row = e.Row.DataItem as DataRowView;
                if (row != null)
                {
                    if (!Convert.IsDBNull(row["ApprovedDate"]))
                    {
                        e.Row.Enabled = Convert.ToBoolean(row["IsLastState"]) == false;
                    }

                    if (!e.Row.Enabled)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                        e.Row.ToolTip = "This document is already approved and cannot be changed.";
                    }
                }
            }
        }
    }
}