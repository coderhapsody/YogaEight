using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using GoGym.Utilities.Extensions;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class MasterPackage : BaseForm
    {
        public List<PackageDetailViewModel> Detail
        {
            get { return ViewState["Detail"] as List<PackageDetailViewModel>; }
            set { ViewState["Detail"] = value; }
        }

        [Inject]
        public ClassProvider ClassService { get; set; }
        [Inject]
        public PackageProvider PackageService { get; set; }
        [Inject]
        public ItemProvider ItemService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }        

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
                FillDropDown();
            }
        }

        private void FillDropDown()
        {
            int[] branchesID = BranchService.GetActiveBranches(User.Identity.Name).Select(branch => branch.ID).ToArray();
            ddlItem.Items.Add(new DropDownListItem(String.Empty)); 
            foreach (var item in ItemService.GetAll(branchesID))
                ddlItem.Items.Add(new DropDownListItem(String.Format("{0} - {1}", item.Barcode, item.Description), item.ID.ToString()));
            ddlItem.SelectedIndex = 0;
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
            WebFormHelper.ClearTextBox(txtName, txtQuantity, txtUnitPrice);
            ddlItem.SelectedIndex = 0;
            Detail = new List<PackageDetailViewModel>();
            RefreshDetail();
            chkIsActive.Checked = true;
            txtName.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                PackageService.Delete(id);
                Refresh();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("AddEdit");
                if (Page.IsValid)
                {
                    if (gvwDetail.Rows.Count > 0)
                    {
                        switch (RowID)
                        {
                            case 0:
                                PackageService.Add(
                                    txtName.Text,
                                    Convert.ToInt32(ddlDuesInMonth.SelectedValue),
                                    chkIsActive.Checked,
                                    chkOpenEnd.Checked,
                                    Convert.ToDecimal(txtFreezeFee.Value.GetValueOrDefault()),
                                    Detail);
                                break;
                            default:
                                PackageService.Update(
                                    RowID,
                                    txtName.Text,
                                    Convert.ToInt32(ddlDuesInMonth.SelectedValue),
                                    chkIsActive.Checked,
                                    chkOpenEnd.Checked,
                                    Convert.ToDecimal(txtFreezeFee.Value.GetValueOrDefault()),
                                    Detail);
                                break;
                        }
                        Refresh();
                    }
                    else
                    {
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblMessageDetail,
                            "Detail of package must have one or more items",
                            LabelStyleNames.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        private void Refresh()
        {
            mvwForm.SetActiveView(viwRead);
            gvwMaster.DataBind();
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    RowID = id;
                    mvwForm.SetActiveView(viwAddEdit);
                    PackageHeader package = PackageService.Get(id);
                    ddlDuesInMonth.SelectedValue = package.PackageDuesInMonth.ToString();
                    txtName.Text = package.Name;
                    chkIsActive.Checked = package.IsActive;
                    Detail = PackageService.GetDetail(id).ToList();
                    chkOpenEnd.Checked = package.OpenEnd;
                    txtFreezeFee.Value = Convert.ToDouble(package.FreezeFee);
                    RefreshDetail();
                    txtName.Focus();
                }
                else if (e.CommandName == "DefineClass")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    RowID = id;
                    mvwForm.SetActiveView(viwAddEdit2);
                    RefreshActiveClassInPackages();
                }
            }
            catch (Exception ex)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        private void RefreshActiveClassInPackages()
        {
            lblPackageName.Text = PackageService.Get(RowID).Name;

            var classes = ClassService.GetAllActiveClasses();
            cblClass.DataSource = classes;
            cblClass.DataValueField = "ID";
            cblClass.DataTextField = "Name";
            cblClass.DataBind();

            var activeClasses = PackageService.PopulateClassForSelectedPackage(RowID);
            foreach (var cls in activeClasses)
            {
                cblClass.Items.FindByValue(cls.ID.ToString(CultureInfo.InvariantCulture)).Selected = true;
            }
        }

        private void RefreshDetail()
        {
            gvwDetail.DataSource = Detail;
            gvwDetail.DataBind();
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@Name"].Value = txtFindName.Text;
        }
        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            Page.Validate("AddDetail");
            if (Page.IsValid)
            {
                int id = Detail.Any() ? Detail.Max(row => row.ID) + 1 : 1;
                Item selectedItem = ItemService.Get(Convert.ToInt32(ddlItem.SelectedValue));
                Detail.Add(
                    new PackageDetailViewModel()
                    {
                        ID = id,
                        ItemBarcode = selectedItem.Barcode,
                        ItemDescription = selectedItem.Description,
                        ItemID = selectedItem.ID,
                        Quantity = Convert.ToInt32(txtQuantity.Text),
                        UnitPrice = Convert.ToDecimal(txtUnitPrice.Text),
                        UnitName = selectedItem.UnitName1
                    });
                WebFormHelper.ClearTextBox(txtQuantity, txtUnitPrice);
                ddlItem.SelectedIndex = 0;
                gvwDetail.DataSource = Detail;
                gvwDetail.DataBind();
            }
        }
        protected void gvwDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Detail.RemoveAll(package => package.ID == id);
                gvwDetail.DataSource = Detail;
                gvwDetail.DataBind();
            }
        }
        protected void gvwDetail_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
        }
        protected void gvwClassDays_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
        }
        protected void btnSaveClassPackage_Click(object sender, EventArgs e)
        {
            try
            {
                var classesID = cblClass.Items.Cast<ListItem>()
                                              .Where(item => item.Selected)
                                              .Select(item => item.Value).ToList().CastAs(Convert.ToInt32);
                PackageService.UpdateClassPackage(RowID, classesID.ToList());
                mvwForm.SetActiveView(viwRead);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatusClassPackage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
            

        }
        protected void sdsClassPackage_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@PackageID"].Value = RowID;
        }
        protected void btnCancelPackage_Click(object sender, EventArgs e)
        {
            mvwForm.ActiveViewIndex = 0;
        }
    }
}