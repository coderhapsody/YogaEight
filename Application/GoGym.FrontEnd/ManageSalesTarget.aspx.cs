using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Utilities.Extensions;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ManageSalesTarget : BaseForm
    {
        [Inject]
        public SalesTargetProvider SalesTargetService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.ActiveViewIndex = 0;
                DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, User.Identity.Name, false);
                DataBindingHelper.PopulateActiveBranches(BranchService, ddlFindBranch, User.Identity.Name, false);
                ddlBranch.Enabled = ddlFindBranch.Enabled = false;

                for (int year = DateTime.Today.Year - 3; year <= DateTime.Today.Year; year++)
                {
                    ddlFindYear.Items.Add(new DropDownListItem(year.ToString()));
                }
                ddlFindYear.FindItemByText(DateTime.Today.Year.ToString()).Selected = true;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEdit");
            if (Page.IsValid)
            {
                try
                {
                    switch (RowID)
                    {
                        case 0:
                            SalesTargetService.AddTarget(
                                Convert.ToInt32(ddlBranch.SelectedValue),
                                mypPeriode.SelectedDate.GetValueOrDefault(DateTime.Today).Year,
                                mypPeriode.SelectedDate.GetValueOrDefault(DateTime.Today).Month,
                                Convert.ToInt32(txtFreshMemberUnit.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtUpgradeUnit.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtRenewalUnit.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtFreshMemberRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtUpgradeRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtRenewalRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtPilatesRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtVocalRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtEFTCollectionRevenue.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtDropOffUnit.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtCancelFees.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtFreezeUnit.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtFreezeFees.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtOtherRevenue.Value.GetValueOrDefault()));
                            break;
                        default:
                            SalesTargetService.UpdateTarget(
                                RowID,
                                Convert.ToInt32(ddlBranch.SelectedValue),
                                mypPeriode.SelectedDate.GetValueOrDefault(DateTime.Today).Year,
                                mypPeriode.SelectedDate.GetValueOrDefault(DateTime.Today).Month,
                                Convert.ToInt32(txtFreshMemberUnit.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtUpgradeUnit.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtRenewalUnit.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtFreshMemberRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtUpgradeRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtRenewalRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtPilatesRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtVocalRevenue.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtEFTCollectionRevenue.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtDropOffUnit.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtCancelFees.Value.GetValueOrDefault()),
                                Convert.ToInt32(txtFreezeUnit.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtFreezeFees.Value.GetValueOrDefault()),
                                Convert.ToDecimal(txtOtherRevenue.Value.GetValueOrDefault()));
                            break;
                    }
                    mvwForm.ActiveViewIndex = 0;
                    gvwMaster.DataBind();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
        }
        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                mvwForm.ActiveViewIndex = 1;
                var salesTarget = SalesTargetService.GetTarget(id);
                RowID = id;
                ddlBranch.SelectedValue = salesTarget.BranchID.ToString();
                mypPeriode.SelectedDate = new DateTime(salesTarget.Year, salesTarget.Month, 1);
                txtFreshMemberUnit.Value = Convert.ToDouble(salesTarget.FreshMemberUnit);
                txtFreshMemberRevenue.Value = Convert.ToDouble(salesTarget.FreshMemberRevenue);
                txtRenewalUnit.Value = Convert.ToDouble(salesTarget.RenewalUnit);
                txtRenewalRevenue.Value = Convert.ToDouble(salesTarget.RenewalRevenue);
                txtUpgradeUnit.Value = Convert.ToDouble(salesTarget.UpgradeUnit);
                txtUpgradeRevenue.Value = Convert.ToDouble(salesTarget.UpgradeRevenue);
                txtVocalRevenue.Value = Convert.ToDouble(salesTarget.VocalRevenue);
                txtPilatesRevenue.Value = Convert.ToDouble(salesTarget.PilatesRevenue);
                txtOtherRevenue.Value = Convert.ToDouble(salesTarget.OtherRevenue);
                txtEFTCollectionRevenue.Value = Convert.ToDouble(salesTarget.EFTCollectionRevenue);
                txtDropOffUnit.Value = Convert.ToDouble(salesTarget.DropOffUnit);
                txtCancelFees.Value = Convert.ToDouble(salesTarget.CancelFees);
                txtFreezeFees.Value = Convert.ToDouble(salesTarget.FreezeFees);
                txtFreezeUnit.Value = Convert.ToDouble(salesTarget.FreezeUnit);                
                txtFreshMemberUnit.Focus();
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@Year"].Value = ddlFindYear.SelectedItem.Text;
        }
        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            RowID = 0;
            mypPeriode.SelectedDate = DateTime.Today;
            mvwForm.ActiveViewIndex = 1;
            WebFormHelper.ClearTextBox(
                txtFreshMemberUnit,
                txtFreshMemberRevenue,
                txtRenewalUnit,
                txtRenewalRevenue,
                txtUpgradeUnit,
                txtUpgradeRevenue,
                txtVocalRevenue,
                txtPilatesRevenue,
                txtOtherRevenue,
                txtFreezeFees,
                txtFreezeUnit,
                txtEFTCollectionRevenue,
                txtDropOffUnit,
                txtCancelFees);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }
        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            SalesTargetService.DeleteTarget(id);
        }
    }
}