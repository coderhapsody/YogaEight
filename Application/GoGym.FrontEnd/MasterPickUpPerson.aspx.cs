using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterPickUpPerson : BaseForm
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }
        [Inject]
        public CustomerStatusProvider CustomerStatusService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public AreaProvider AreaService { get; set; }
        [Inject]
        public ContractProvider ContractService { get; set; }
        [Inject]
        public CreditCardTypeProvider CreditCardTypeService { get; set; }
        [Inject]
        public BankProvider BankService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

            if (!this.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                FillDropDown();
            }

        }

        private void FillDropDown()
        {
            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();
            ddlFindBranch.Enabled = false;
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
            e.Command.Parameters["@Name"].Value = txtFindName.Text;
        }
        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
            }
        }
        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string customerCode = e.Row.Cells[1].Text;

                (e.Row.FindControl("hypPerson") as System.Web.UI.HtmlControls.HtmlAnchor).Attributes["onclick"] =
                    String.Format("window.open('MasterParents.aspx?PickUpPerson=1&CustomerCode={0}', 'Parent', 'width=800,height=500,location=no,resizable=yes')", customerCode);
            }
        }
    }
}