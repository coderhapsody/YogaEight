using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class MasterEmployee : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
                FillDropDown();
            }
        }

        private void FillDropDown()
        {
            ddlHomeBranch.DataSource = BranchService.GetAllowedBranches(User.Identity.Name);
            ddlHomeBranch.DataTextField = "Name";
            ddlHomeBranch.DataValueField = "ID";
            ddlHomeBranch.DataBind();
            ddlHomeBranch.Items.Insert(0, new DropDownListItem(String.Empty));

            ddlFindHomeBranch.DataSource = BranchService.GetAllowedBranches(User.Identity.Name);
            ddlFindHomeBranch.DataTextField = "Name";
            ddlFindHomeBranch.DataValueField = "ID";
            ddlFindHomeBranch.DataBind();
            ddlFindHomeBranch.SelectedValue = Convert.ToString(EmployeeService.GetHomeBranchID(User.Identity.Name));
            //ddlFindHomeBranch.Items.Insert(0, new DropDownListItem("All Branches", "0"));
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fileInfo = null;
                string fileName = null;

                if (fupPhoto.HasFile && !chkDeletePhoto.Checked)
                {
                    fileInfo = new FileInfo(fupPhoto.FileName);
                    fileName = chkDeletePhoto.Checked ? null : Guid.NewGuid().ToString().ToUpper() + fileInfo.Extension;
                    fupPhoto.SaveAs(Server.MapPath(ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoEmployees]) + @"\" + fileName);
                }
                else
                    fileName = Convert.ToString(ViewState["Photo"]);

                EmployeeService.Update(
                    RowID,
                    txtBarcode.Text,
                    lblUserName.Text,
                    Convert.ToInt32(ddlHomeBranch.SelectedValue),
                    txtFirstName.Text,
                    txtLastName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    chkDeletePhoto.Checked,
                    fileName,
                    chkIsActive.Checked,
                    chkCanApproveDocument.Checked,
                    chkCanEditActiveContract.Checked,
                    chkCanReprint.Checked);
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
            ReloadCurrentPage();
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                Employee employee = EmployeeService.Get(id);
                lblUserName.Text = employee.UserName;
                txtBarcode.Text = employee.Barcode;
                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                txtPhone.Text = employee.Phone;
                txtEmail.Text = employee.Email;
                chkIsActive.Checked = employee.IsActive;
                ddlHomeBranch.SelectedValue = Convert.ToString(employee.HomeBranchID);
                chkCanApproveDocument.Checked = employee.CanApproveDocument;
                chkCanEditActiveContract.Checked = employee.CanEditActiveContract;
                chkCanReprint.Checked = employee.CanReprint;
                ViewState["Photo"] = employee.Photo;

                if (!String.IsNullOrEmpty(employee.Photo))
                {
                    var file = new FileInfo(employee.Photo);
                    imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoEmployees] + @"\" + file.Name.Substring(0, file.Name.IndexOf(".", StringComparison.Ordinal)) + file.Extension + ".ashx?w=200";
                }
                else
                    imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoEmployees] + @"\default.png";
                chkDeletePhoto.Checked = false;
                txtBarcode.Focus();
            }
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
            e.Command.Parameters["@Name"].Value = txtFindName.Text;
            e.Command.Parameters["@HomeBranchID"].Value = Convert.ToInt32(ddlFindHomeBranch.SelectedValue);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
    }
}