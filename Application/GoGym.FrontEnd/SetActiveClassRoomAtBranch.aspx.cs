using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class SetActiveClassRoomAtBranch : System.Web.UI.Page
    {
        [Inject]
        public ClassProvider ClassService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                foreach (var item in ClassService.GetAllClassRooms())
                    ddlItem.Items.Add(new DropDownListItem(String.Format("{0}", item.Name), item.ID.ToString()));
                ddlItem.Items.Insert(0, new DropDownListItem("Select item"));
                btnSave.Enabled = false;
            }
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedIndex > 0)
            {
                dlsBranches.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
                dlsBranches.DataBind();

                foreach (DataListItem item in dlsBranches.Items)
                {
                    (item.FindControl("chkBranch") as CheckBox).Checked = false;
                    (item.FindControl("txtCapacity") as TextBox).Text = "0";
                }

                var activeBranchRooms = ClassService.GetBranchesByClassRoom(Convert.ToInt32(ddlItem.SelectedValue)).ToArray();
                foreach (var activeBranchRoom in activeBranchRooms)
                {
                    foreach (DataListItem item in dlsBranches.Items)
                    {
                        if ((item.FindControl("chkBranch") as CheckBox).Attributes["Value"] == activeBranchRoom.BranchID.ToString())
                        {
                            (item.FindControl("chkBranch") as CheckBox).Checked = true;
                            (item.FindControl("txtCapacity") as TextBox).Text = activeBranchRoom.Capacity.ToString();
                        }
                    }
                }
            }
            else
            {
                dlsBranches.DataSource = null;
                dlsBranches.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IList<BranchRoomCapacityViewModel> branchesID = new List<BranchRoomCapacityViewModel>();
                foreach (DataListItem item in dlsBranches.Items)
                {
                    if ((item.FindControl("chkBranch") as CheckBox).Checked)
                        branchesID.Add(new BranchRoomCapacityViewModel()
                                       {
                                           BranchID =
                                               Convert.ToInt32(
                                                   (item.FindControl("chkBranch") as CheckBox).Attributes["Value"]),
                                           Capacity = Convert.ToInt32((item.FindControl("txtCapacity") as TextBox).Text)
                                       });
                }

                ClassService.UpdateRoomsAtBranch(
                    Convert.ToInt32(ddlItem.SelectedValue),
                    branchesID);

                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    String.Format("Settings for <b>{0}</b> saved.", ddlItem.SelectedItem.Text),
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