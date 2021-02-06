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
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ManageUsers : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        [Inject]
        public SecurityProvider SecurityService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.ActiveViewIndex = 0;
                ViewState["_Sort"] = "UserName";
                ViewState["_SortDirection"] = SortDirection.Ascending;

                ddlFindRoles.DataSource = SecurityService.GetAllRoles();
                ddlFindRoles.DataValueField = "ID";
                ddlFindRoles.DataTextField = "Name";
                ddlFindRoles.DataBind();

                ddlRole.DataSource = SecurityService.GetAllRoles();
                ddlRole.DataValueField = "ID";
                ddlRole.DataTextField = "Name";
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new DropDownListItem(String.Empty));
                ddlRole.SelectedIndex = 0;

                ddlHomeBranch.DataSource = BranchService.GetActiveBranches();
                ddlHomeBranch.DataTextField = "Name";
                ddlHomeBranch.DataValueField = "ID";
                ddlHomeBranch.DataBind();
                ddlHomeBranch.Items.Insert(0, new DropDownListItem(String.Empty));
            }
        }

        //private void BindToGrid(string sortExpression, SortDirection direction)
        //{
        //    var users = ViewState["_Users"] == null ? UserManagement.GetAllMembershipUserByRole(ddlFindRoles.SelectedItem.Text) :
        //                                              ViewState["_Users"] as List<MembershipUser>;

        //    switch (direction)
        //    {
        //        case SortDirection.Ascending:
        //            users = users.OrderBy(sortExpression).ToList();
        //            break;
        //        case SortDirection.Descending:
        //            users = users.OrderBy(sortExpression + " desc").ToList();
        //            break;
        //    }

        //    gvwMaster.DataSource = from user in users select new { user.UserName, user.CreationDate, user.LastLoginDate, user.IsLockedOut, user.Email };
        //    gvwMaster.DataBind();

        //    ViewState["_Users"] = users;
        //}

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditRow"))
            {
                string userName = e.CommandArgument.ToString();
                ViewState["_UserName"] = userName;
                mvwForm.ActiveViewIndex = 1;
                Employee emp = EmployeeService.Get(userName);
                txtUserName.Text = emp.UserName;
                txtEmail.Text = emp.Email;
                ddlRole.SelectedValue = Convert.ToString(emp.RoleID.GetValueOrDefault());
                rqvConfirmPassword.Enabled = rqvPassword.Enabled = txtConfirmPassword.Enabled = txtPassword.Enabled = false;
                txtPassword.BackColor = txtConfirmPassword.BackColor = System.Drawing.Color.Gray;
                try
                {
                    ddlHomeBranch.SelectedValue = emp.HomeBranchID.ToString();
                }
                catch { }
                txtBarcode.Text = emp.Barcode;

                cuvPassword.Enabled = rqvConfirmPassword.Enabled = rqvPassword.Enabled = false;
                cuvPassword.IsValid = rqvConfirmPassword.IsValid = rqvPassword.IsValid = true;

                chkIsActive.Checked = emp.IsActive;
                chkIsActive.Enabled = userName.ToLower() != "admin";
                txtUserName.Focus();
            }
            else if (e.CommandName.Equals("ResetPassword"))
            {
                mvwForm.ActiveViewIndex = 2;
                lblResetUserName.Text = e.CommandArgument.ToString();
                ViewState["_UserName"] = lblResetUserName.Text;
            }
        }
        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DynamicControlBinding.ChangeBackgroundColorRowOnHover(e);
        }

        protected void cuvPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Length >= 6;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string userName = ViewState["_UserName"].ToString();
            try
            {
                if (String.IsNullOrEmpty(userName))
                {
                    EmployeeService.Add(
                        txtUserName.Text,
                        txtBarcode.Text,
                        txtPassword.Text,
                        txtUserName.Text,
                        Convert.ToInt32(ddlHomeBranch.SelectedValue),
                        Convert.ToInt32(ddlRole.SelectedValue),
                        txtEmail.Text,
                        chkIsActive.Checked,
                        false);
                }
                else
                {
                    EmployeeService.Update(
                        txtUserName.Text,
                        txtBarcode.Text,
                        txtUserName.Text,
                        Convert.ToInt32(ddlHomeBranch.SelectedValue),
                        Convert.ToInt32(ddlRole.SelectedValue),
                        txtEmail.Text,
                        chkIsActive.Checked);
                }

                mvwForm.SetActiveView(View1);
                ddlFindRoles.SelectedValue = ddlRole.SelectedValue;
            }
            catch (Exception ex)
            {
                lblStatus.Text = String.Format("Cannot add/edit user information: {0}", ex.Message);
                LogService.ErrorException(GetType().FullName, ex);
            }            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnDoReset_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = ViewState["_UserName"].ToString();
                //UserManagement.UnlockUser(userName);
                string newPassword = SecurityService.ResetPassword(userName);
                lblNewPassword.Text = String.Format("New Password for <strong>{0}</strong> is <strong style='color:green;'>{1}</strong>", userName, newPassword);
                btnDoReset.Enabled = false;
            }
            catch (Exception ex)
            {
                DynamicControlBinding.SetLabelTextWithCssClass(lblNewPassword, String.Format("<b>Error:</b> {0}", ex.Message), LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            gvwMaster.Rows.Cast<GridViewRow>().Where(gridViewRow => (gridViewRow.Cells[gridViewRow.Cells.Count - 1].Controls[1] as CheckBox).Checked).ForEach(
                gridViewRow =>
                {
                    string userName = gridViewRow.Cells[0].Text;
                    try
                    {
                        if (userName.Equals("admin") || userName.Equals(User.Identity.Name))
                        {
                            throw new Exception(String.Format("User {0} cannot be deleted.", userName));
                        }                        
                        EmployeeService.Delete(userName);                        
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = String.Format("Cannot delete user <strong>{0}</strong>: {1}", userName, ex.Message);
                    }
                });            
            gvwMaster.DataBind();
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@RoleID"].Value = ddlFindRoles.SelectedValue;
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            ddlRole.SelectedIndex = 0;
            txtUserName.Text = String.Empty;
            txtPassword.Text = String.Empty;
            txtConfirmPassword.Text = String.Empty;
            txtEmail.Text = String.Empty;
            mvwForm.ActiveViewIndex = 1;
            ViewState["_UserName"] = String.Empty;
            rqvConfirmPassword.Enabled = rqvPassword.Enabled = txtConfirmPassword.Enabled = txtPassword.Enabled = true;
            txtPassword.BackColor = txtConfirmPassword.BackColor = System.Drawing.Color.White;
            txtBarcode.Text = String.Empty;
            ddlHomeBranch.SelectedIndex = 0;
            chkIsActive.Checked = true;
            cuvPassword.Enabled = rqvConfirmPassword.Enabled = rqvPassword.Enabled = true;
            txtUserName.Focus();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
    }
}