using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class SetActiveUserAtBranch : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public UserProvider UserService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlUser.DataSource = UserService.GetUsers();
                ddlUser.DataTextField = "UserName";
                ddlUser.DataValueField = "UserName";
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new DropDownListItem("Select user"));
                btnSave.Enabled = false;
            }
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
        }

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUser.SelectedIndex > 0)
            {
                cblBranches.DataSource = BranchService.GetActiveBranches();
                cblBranches.DataValueField = "ID";
                cblBranches.DataTextField = "Name";
                cblBranches.DataBind();
                foreach (var item in BranchService.GetAllowedBranches(ddlUser.SelectedValue))
                    cblBranches.Items.FindByValue(item.ID.ToString()).Selected = true;

                btnSave.Enabled = true;
            }
            else
            {
                cblBranches.DataSource = null;
                cblBranches.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IList<int> branchesID = (from ListItem item in cblBranches.Items
                                         where item.Selected
                                         select Convert.ToInt32(item.Value)).ToList();

                UserService.UpdateUserAtBranch(ddlUser.SelectedValue, branchesID);

                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    String.Format("Settings for <b>{0}</b> saved.", ddlUser.SelectedValue),
                    LabelStyleNames.AlternateMessage);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
            }
        }
    }
}