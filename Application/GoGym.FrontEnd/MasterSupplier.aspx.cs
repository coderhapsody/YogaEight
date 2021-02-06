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
    public partial class MasterSupplier : BaseForm
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        [Inject]
        public SupplierProvider SupplierService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                RadHelper.SetUpGrid(grdMaster);
            }
        }

        //protected override void OnLoadComplete(EventArgs e)
        //{
        //    var privilege = SecurityService.GetPrivilege(
        //        ManagementService.GetConnectionString(this.GetCurrentCompanyCode()),
        //        CurrentPageName);

        //    if (mvwForm.GetActiveView() == viwRead)
        //    {
        //        lnbDelete.Enabled = privilege.AllowDelete;
        //        if (RowID == 0)
        //            lnbAddNew.Enabled = privilege.AllowAddNew;
        //    }
        //    else
        //    {
        //        if (btnSave.Enabled)
        //            btnSave.Enabled = RowID == 0 ? privilege.AllowAddNew : privilege.AllowUpdate;
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddEdit");
            if (Page.IsValid)
            {
                try
                {
                    SupplierService.AddOrUpdateSupplier(RowID,
                        txtName.Text,
                        txtAddress.Text,
                        txtNPWP.Text,
                        txtEmail.Text,
                        txtPhone1.Text,
                        txtPhone2.Text,
                        chkTaxable.Checked);

                    ReloadCurrentPage();
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatusAddEdit, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);   
                }

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            txtName.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] arrayOfID = RadHelper.GetRowIdForDeletion(grdMaster);
                if (SupplierService.CanDeleteSupplier(arrayOfID))
                {
                    SupplierService.DeleteSupplier(arrayOfID);
                    btnCancel_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);   
            }
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@Name"].Value = txtFindName.Text;
        }

        protected void grdMaster_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                RowID = Convert.ToInt32(e.CommandArgument);
                var supplier = SupplierService.GetSupplier(RowID);
                mvwForm.SetActiveView(viwAddEdit);

                if (supplier == null) return;

                txtName.Text = supplier.Name;
                txtPhone1.Text = supplier.Phone1;
                txtPhone2.Text = supplier.Phone2;
                txtEmail.Text = supplier.Email;
                txtAddress.Text = supplier.Address;
                txtNPWP.Text = supplier.NPWP;
                chkTaxable.Checked = supplier.Taxable;
                txtName.Focus();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdMaster.DataBind();
        }
    }
}