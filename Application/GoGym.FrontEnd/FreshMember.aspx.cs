using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class FreshMember : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        [Inject]
        public PackageProvider PackageService { get; set; }

        [Inject]
        public UserProvider UserService { get; set; }

        [Inject]
        public PaymentTypeProvider PaymentTypeService { get; set; }

        [Inject]
        public InvoiceProvider InvoiceService { get; set; }

        [Inject]
        public ContractProvider ContractService { get; set; }

        [Inject]
        public CreditCardTypeProvider CreditCardTypeService { get; set; }

// ReSharper disable once InconsistentNaming
        public List<PackageDetailViewModel> _PackageDetail
        {
            get
            {
                var packageDetailViewModels = ViewState["PackageDetail"] as List<PackageDetailViewModel>;
                if (packageDetailViewModels != null)
                    foreach (var item in packageDetailViewModels)
                        item.NetAmount = (item.Quantity * item.UnitPrice - (item.Discount / 100 * item.Quantity * item.UnitPrice)) / (item.IsTaxed ? 1.1M : 1M);

                return ViewState["PackageDetail"] as List<PackageDetailViewModel>;
            }
            set
            {
                if (ViewState["PackageDetail"] == null)
                    ViewState["PackageDetail"] = new List<PackageDetailViewModel>();

                foreach (var item in value)
                    item.NetAmount = (item.Quantity * item.UnitPrice - (item.Discount / 100 * item.Quantity * item.UnitPrice)) / (item.IsTaxed ? 1.1M : 1M);


                ViewState["PackageDetail"] = value;
            }
        }

// ReSharper disable once InconsistentNaming
        public List<PaymentDetailViewModel> _PaymentDetail
        {
            get
            {
                return ViewState["PaymentDetail"] as List<PaymentDetailViewModel>;
            }
            set
            {
                if (ViewState["PaymentDetail"] == null)
                    ViewState["PaymentDetail"] = new List<PaymentDetailViewModel>();
                ViewState["PaymentDetail"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDown();
                _PaymentDetail = new List<PaymentDetailViewModel>();
                _PackageDetail = new List<PackageDetailViewModel>();
                CalculateTotalInvoiceAndPayment();
                calDate.SelectedDate = DateTime.Today;

                hypLookUpContract.Attributes["onclick"] = String.Format("showPromptPopUp('PromptContract.aspx?BranchID={0}', '{1}', 550, 900);", ddlBranch.SelectedValue, txtContractNo.ClientID);

                ddlBranch.Enabled = false;
            }
        }

        private void FillDropDown()
        {
            ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            //ddlBranch.Enabled = ddlBranch.Items.Count > 0;            

            ddlPaymentType.DataSource = PaymentTypeService.GetAll();
            ddlPaymentType.DataTextField = "Description";
            ddlPaymentType.DataValueField = "ID";
            ddlPaymentType.DataBind();

            ddlBranch_SelectedIndexChanged(null, null);
        }

        private void LoadPackageDetail()
        {
            PackageHeader pkg = ContractService.GetPackageInContract(txtContractNo.Text);
            IEnumerable<PackageDetailViewModel> package = PackageService.GetDetail(pkg.ID);
            _PackageDetail = package.ToList();
            gvwPackage.DataSource = package;
            gvwPackage.DataBind();
            CalculateTotalInvoiceAndPayment();

            lblPackage.Text = pkg.Name;
        }

        protected void gvwPackage_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        private void _CalculateTotalInvoiceAndPayment(decimal discountValue, out decimal totalInvoice, out decimal totalPayment)
        {
            totalInvoice = (_PackageDetail.Any() ?
                _PackageDetail.Sum(row => ((row.UnitPrice * row.Quantity) - row.Discount / 100 * (row.UnitPrice * row.Quantity)) * 1 /*(row.IsTaxed ? 1.1M : 1M)*/) : 0);
            totalInvoice -= discountValue;
            totalPayment = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M);
        }

        private void CalculateTotalInvoiceAndPayment()
        {
            decimal totalInvoice, totalPayment;
            _CalculateTotalInvoiceAndPayment(
                0,
                out totalInvoice,
                out totalPayment);
            lblTotalInvoice.Text = totalInvoice.ToString("###,##0.00");
            lblTotalPayment.Text = totalPayment.ToString("###,##0.00");
        }


        protected void gvwPackage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int packageID = Convert.ToInt32(e.CommandArgument);
            int rowIndex = ((e.CommandSource as LinkButton).NamingContainer as GridViewRow).RowIndex;
            if (e.CommandName == "EditPackage")
            {
                gvwPackage.EditIndex = rowIndex;
                gvwPackage.DataSource = _PackageDetail;
                gvwPackage.DataBind();
            }
            else if (e.CommandName == "DeletePackage")
            {
                _PackageDetail.RemoveAll(package => package.ID == packageID);
                gvwPackage.DataSource = _PackageDetail;
                gvwPackage.DataBind();
            }
            else if (e.CommandName == "CancelPackage")
            {
                gvwPackage.EditIndex = -1;
                gvwPackage.DataSource = _PackageDetail;
                gvwPackage.DataBind();
            }
            else if (e.CommandName == "SavePackage")
            {
                int quantity = Convert.ToInt32((gvwPackage.Rows[rowIndex].Cells[4].Controls[1] as RadNumericTextBox).Value);
                int unitPrice = Convert.ToInt32(Convert.ToDecimal((gvwPackage.Rows[rowIndex].Cells[6].Controls[1] as RadNumericTextBox).Value));
                decimal discount = Convert.ToDecimal((gvwPackage.Rows[rowIndex].Cells[7].Controls[1] as RadNumericTextBox).Value);
                bool isTaxed = (gvwPackage.Rows[rowIndex].Cells[8].Controls[1] as CheckBox).Checked;

                var package = _PackageDetail.SingleOrDefault(row => row.ID == packageID);
                if (package != null)
                {
                    int index = _PackageDetail.FindIndex(row => row.ID == packageID);
                    package.Quantity = quantity;
                    package.UnitPrice = unitPrice;
                    package.Discount = discount;
                    package.IsTaxed = isTaxed;
                    package.Total = quantity * unitPrice - (quantity * unitPrice * discount / 100);
                    _PackageDetail[index] = package;
                    gvwPackage.EditIndex = -1;
                    gvwPackage.DataSource = _PackageDetail;
                    gvwPackage.DataBind();
                }
            }
            CalculateTotalInvoiceAndPayment();
        }
        protected void gvwPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PackageDetailViewModel row = e.Row.DataItem as PackageDetailViewModel;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("chkIsTaxable") as CheckBox).Checked = row.IsTaxed;
            }
        }

        protected void btnAddPayment_Click(object sender, EventArgs e)
        {
            _PaymentDetail.Add(
                new PaymentDetailViewModel()
                {
                    ID = _PaymentDetail.Any() ? _PaymentDetail.Max(pay => pay.ID) + 1 : 1,
                    PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue),
                    PaymentType = ddlPaymentType.SelectedItem.Text,
                    CreditCardTypeID = ddlCreditCardType.SelectedItem == null ? (int?)null : Convert.ToInt32(ddlCreditCardType.SelectedValue),
                    CreditCardType = ddlCreditCardType.SelectedItem == null ? String.Empty : ddlCreditCardType.SelectedItem.Text,
                    ApprovalCode = txtApprovalCode.Text,
                    Notes = txtPaymentNotes.Text,
                    Amount = Convert.ToDecimal(txtPaymentAmount.Text)
                });
            txtPaymentNotes.Text = String.Empty;
            gvwPayment.DataSource = _PaymentDetail;
            gvwPayment.DataBind();
            txtPaymentAmount.Value = 0;
            txtApprovalCode.Text = String.Empty;
            CalculateTotalInvoiceAndPayment();
        }
        protected void gvwPayment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
        protected void gvwPayment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeletePayment")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                _PaymentDetail.RemoveAll(pay => pay.ID == id);
                gvwPayment.DataSource = _PaymentDetail;
                gvwPayment.DataBind();
            }
            CalculateTotalInvoiceAndPayment();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.Validate("FreshMember");
            if (this.IsValid)
            {
                decimal totalInvoice = 0, totalPayment = 0;
                _CalculateTotalInvoiceAndPayment(
                    0,
                    out totalInvoice,
                    out totalPayment);
                //if (totalInvoice != totalPayment || (totalInvoice == 0 && totalPayment == 0))
                if (totalInvoice != totalPayment)
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblStatus,
                        "Total Invoice is not equal Total Payment. Please verify this transaction again",
                        LabelStyleNames.ErrorMessage);
                    return;
                }

                try
                {
                    string invoiceNo = InvoiceService.CreateFreshMemberInvoice(
                        Convert.ToInt32(ddlBranch.SelectedValue),
                        txtContractNo.Text,
                        calDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                        Convert.ToInt32(ddlSales.SelectedValue),
                        txtNotes.Text,
                        0,
                        _PackageDetail,
                        _PaymentDetail);

                    Response.Redirect(String.Format("FreshMemberCompleted.aspx?InvoiceNo={0}", invoiceNo));
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
        protected void btnDummy_Click(object sender, EventArgs e)
        {
            LoadPackageDetail();
            CalculateTotalInvoiceAndPayment();
        }

        protected void cuvContractNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = txtContractNo.Text.Trim().Length > 0 &&
                           ContractService.IsValidContract(args.Value);
        }
        protected void txtDiscountValue_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalInvoiceAndPayment();
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int paymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue);
            ddlCreditCardType.Items.Clear();
            if (paymentTypeID == 4) //Credit Card
            {                
                foreach (var item in CreditCardTypeService.GetAll())
                {
                    ddlCreditCardType.Items.Add(
                        new DropDownListItem(
                            item.Description,
                            item.ID.ToString()));
                }
            }            
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int selectedBranchID = Convert.ToInt32(ddlBranch.SelectedValue);

            ddlSales.DataSource = EmployeeService.GetAll(selectedBranchID);
            ddlSales.DataTextField = "FirstName";
            ddlSales.DataValueField = "ID";
            ddlSales.DataBind();
            ddlSales.Items.Insert(0, new DropDownListItem(String.Empty));

            hypLookUpContract.Attributes["onclick"] = String.Format("showPromptPopUp('PromptContract.aspx?BranchID={0}', '{1}', 550, 900);", ddlBranch.SelectedValue, txtContractNo.ClientID);
        }
    }
}