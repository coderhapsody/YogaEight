using System;
using System.Data;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class PromptPurchaseOrder : System.Web.UI.Page
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        [Inject]
        public BranchProvider BranchService { get; set; }

        [Inject]
        public ItemProvider ItemService { get; set; }

        [Inject]
        public SupplierProvider SupplierService { get; set; }

        [Inject]
        public EmployeeProvider EmployeeService { get; set; }

        [Inject]
        public PurchaseOrderProvider PurchaseOrderService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RadHelper.SetUpGrid(grdMaster);
                FillDropDown();
            }
        }

        private void FillDropDown()
        {
            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();
            ddlFindBranch.SelectedValue = Request.QueryString["BranchID"];
            ddlFindBranch.Enabled = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdMaster.DataBind();
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = Convert.ToInt32(ddlFindBranch.SelectedValue);
            e.Command.Parameters["@DateFrom"].Value = dtpFindDateFrom.SelectedDate ?? new DateTime(1980, 1, 1);
            e.Command.Parameters["@DateTo"].Value = dtpFindDateTo.SelectedDate ?? new DateTime(2099, 12, 31);
            e.Command.Parameters["@DocumentNo"].Value = txtFindDocumentNo.Text;
        }

        protected void grdMaster_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                var hypSelect = e.Item.FindControl("hypSelect") as HyperLink;
                if (hypSelect != null)
                {
                    var dataRow = e.Item.DataItem as DataRowView;
                    string script =
                        String.Format(
                            "javascript:window.opener.document.getElementById('{0}').value='{1}'; window.opener.$telerik.findControl(window.opener.document.forms[0],'{2}').set_value('{3}'); window.close(); return false;",
                            Request["Code"],
                            dataRow["ID"],
                            Request["Name"],
                            dataRow["DocumentNo"]);
                    hypSelect.Attributes.Add("onclick", script);
                }
            }
        }
    }
}