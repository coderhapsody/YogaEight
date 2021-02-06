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
    public partial class MasterItem : BaseForm
    {
        [Inject]
        public ItemProvider ItemService { get; set; }
        [Inject]
        public ItemTypeProvider ItemTypeService { get; set; }
        [Inject]
        public ItemAccountProvider ItemAccountService { get; set; }
        
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
            ddlFindItemType.DataSource = ItemTypeService.GetAll();
            ddlFindItemType.DataTextField = "Description";
            ddlFindItemType.DataValueField = "ID";
            ddlFindItemType.DataBind();
            ddlFindItemType.Items.Insert(0, new DropDownListItem("All", 0.ToString()));

            ddlItemType.DataSource = ItemTypeService.GetAll();
            ddlItemType.DataTextField = "Description";
            ddlItemType.DataValueField = "ID";
            ddlItemType.DataBind();
            ddlItemType.Items.Insert(0, new DropDownListItem(String.Empty));

            ddlAccount.Items.Add(new DropDownListItem(String.Empty));
            foreach (var account in ItemAccountService.GetValuedAccounts())
            {
                ddlAccount.Items.Add(
                    new DropDownListItem(
                        String.Format("{0} - {1}", account.AccountNo, account.Description),
                        account.ID.ToString()));
            }
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            RowID = 0;
            WebFormHelper.ClearTextBox(txtBarcode, txtDescription);
            txtUnitPrice.Value = 0;
            ddlAccount.SelectedIndex = 0;
            ddlItemType.SelectedIndex = 0;
            chkIsActive.Checked = true;
            chkIsTaxed.Checked = true;
            txtBarcode.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                ItemService.Delete(id);
                Refresh();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
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
                            ItemService.Add(
                                txtBarcode.Text,
                                txtDescription.Text,
                                Convert.ToInt32(ddlAccount.SelectedValue),
                                Convert.ToInt32(ddlItemType.SelectedValue),
                                Convert.ToDecimal(txtUnitPrice.Text),
                                chkIsActive.Checked,
                                chkIsTaxed.Checked,
                                String.IsNullOrEmpty(txtUnitName1.Text.Trim()) ? "UNIT" : txtUnitName1.Text,
                                txtUnitName2.Text,
                                txtUnitName3.Text,
                                String.IsNullOrEmpty(txtUnitName2.Text.Trim())
                                    ? 0
                                    : Convert.ToInt32(txtUnitFactor2.Value),
                                String.IsNullOrEmpty(txtUnitName3.Text.Trim())
                                    ? 0
                                    : Convert.ToInt32(txtUnitFactor3.Value),
                                chkHasSessions.Checked
                                    ? Convert.ToInt16(txtSessionBalance.Value.GetValueOrDefault(0))
                                    : (short?) null);
                            break;
                        default:
                            ItemService.Update(
                                RowID,
                                txtBarcode.Text,
                                txtDescription.Text,
                                Convert.ToInt32(ddlAccount.SelectedValue),
                                Convert.ToInt32(ddlItemType.SelectedValue),
                                Convert.ToDecimal(txtUnitPrice.Text),
                                chkIsActive.Checked,
                                chkIsTaxed.Checked,
                                String.IsNullOrEmpty(txtUnitName1.Text.Trim()) ? "UNIT" : txtUnitName1.Text,
                                txtUnitName2.Text,
                                txtUnitName3.Text,
                                String.IsNullOrEmpty(txtUnitName2.Text.Trim())
                                    ? 0
                                    : Convert.ToInt32(txtUnitFactor2.Value.GetValueOrDefault(0)),
                                String.IsNullOrEmpty(txtUnitName3.Text.Trim())
                                    ? 0
                                    : Convert.ToInt32(txtUnitFactor3.Value.GetValueOrDefault(0)),
                                chkHasSessions.Checked
                                    ? Convert.ToInt16(txtSessionBalance.Value.GetValueOrDefault(0))
                                    : (short?) null);
                            break;
                    }
                    Refresh();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }

        private void Refresh()
        {
            mvwForm.SetActiveView(viwRead);
            gvwMaster.DataBind();
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                Item item = ItemService.Get(id);
                txtDescription.Text = item.Description;
                txtBarcode.Text = item.Barcode;
                txtUnitPrice.Value = Convert.ToDouble(item.UnitPrice);
                ddlAccount.SelectedValue = Convert.ToString(item.ItemAccountID);
                ddlItemType.SelectedValue = Convert.ToString(item.ItemTypeID);
                chkIsActive.Checked = item.IsActive;
                chkIsTaxed.Checked = item.IsTaxed;
                txtUnitName1.Text = item.UnitName1;
                txtUnitName2.Text = item.UnitName2;
                txtUnitName3.Text = item.UnitName3;
                txtUnitFactor2.Value = String.IsNullOrEmpty(item.UnitName2)
                    ? (double?) null
                    : Convert.ToDouble(item.UnitFactor2);
                txtUnitFactor3.Value = String.IsNullOrEmpty(item.UnitName3)
                    ? (double?) null
                    : Convert.ToDouble(item.UnitFactor3);
                txtSessionBalance.Value = item.SessionBalance.GetValueOrDefault(0);
                txtBarcode.Focus();
            }
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
            e.Command.Parameters["@Description"].Value = txtFindDescription.Text;
            e.Command.Parameters["@ItemTypeID"].Value = ddlFindItemType.SelectedValue;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }

        protected void ddlItemType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            //var itemType = ItemTypeService.Get(Convert.ToInt32(e.Value));
            //if(itemType.Type == "S" && itemType.HasSessions)

        }
    }
}