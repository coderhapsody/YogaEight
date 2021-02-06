using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Utilities.Extensions;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class MasterContract : BaseForm
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }
        [Inject]
        public CreditCardTypeProvider CreditCardTypeService { get; set; }
        [Inject]
        public ContractProvider ContractService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public BillingTypeProvider BillingTypeService { get; set; }
        [Inject]
        public PackageProvider PackageService { get; set; }
        [Inject]
        public BankProvider BankService { get; set; }
        [Inject]
        public AreaProvider AreaService { get; set; }
        [Inject]
        public ItemProvider ItemService { get; set; }
        [Inject]
        public OccupationProvider OccupationService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                FillDropDown();
                RadHelper.SetUpDatePickers(calBirthDateFather,
                    calBirthDateMother,
                    calDate,
                    calDateOfBirth,
                    calEffectiveDate,
                    calExpiredDate,
                    calNextDuesDate);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
            }

            hypLookUpCustomer.Attributes["onclick"] =
                String.Format("showPromptPopUp('PromptCustomer.aspx?', '{0}', 550, 900); ", txtCustomerBarcode.ClientID);
        }

        private void FillDropDown(bool isAddNew = true)
        {
            ddlArea.DataSource = AreaService.GetAll();
            ddlArea.DataTextField = "Description";
            ddlArea.DataValueField = "ID";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem(String.Empty, "0"));

            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();


            ddlBillingType.DataSource = BillingTypeService.GetActiveBillingTypes(isAddNew);
            ddlBillingType.DataTextField = "Description";
            ddlBillingType.DataValueField = "ID";
            ddlBillingType.DataBind();
            ddlBillingType.Items.Insert(0, String.Empty);

            ddlPackage.DataSource = PackageService.GetPackagesInBranch(Convert.ToInt32(ddlFindBranch.SelectedValue), isAddNew);
            ddlPackage.DataTextField = "Name";
            ddlPackage.DataValueField = "ID";
            ddlPackage.DataBind();
            ddlPackage.Items.Insert(0, String.Empty);

            ddlOccupation.DataSource = OccupationService.GetAll();
            ddlOccupation.DataValueField = "ID";
            ddlOccupation.DataTextField = "Description";
            ddlOccupation.DataBind();
            ddlOccupation.Items.Insert(0, new DropDownListItem(String.Empty));

            ddlBillingBank.DataSource = BankService.GetActiveBanks(isAddNew);
            ddlBillingBank.DataTextField = "Name";
            ddlBillingBank.DataValueField = "ID";
            ddlBillingBank.DataBind();
            ddlBillingBank.Items.Insert(0, String.Empty);


            ddlCardExpiredMonth.DataSource = CommonHelper.GetMonthNames();
            ddlCardExpiredMonth.DataTextField = "Value";
            ddlCardExpiredMonth.DataValueField = "Key";
            ddlCardExpiredMonth.DataBind();
            ddlCardExpiredMonth.SelectedValue = DateTime.Today.Month.ToString(CultureInfo.InvariantCulture);

            ddlCardExpiredYear.DataSource = Enumerable.Range(2005, DateTime.Today.Year + 10 - 2005);
            ddlCardExpiredYear.DataBind();
            ddlCardExpiredYear.SelectedValue = DateTime.Today.Year.ToString();

            ddlMonthlyDuesItem.DataSource = ItemService.GetMonthlyDuesItem();
            ddlMonthlyDuesItem.DataTextField = "Description";
            ddlMonthlyDuesItem.DataValueField = "ID";
            ddlMonthlyDuesItem.DataBind();
            ddlMonthlyDuesItem.Items.Insert(0, String.Empty);

            DataBindingHelper.PopulateCreditCardTypes(CreditCardTypeService, ddlBillingCardType, true);
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

            ResetFields();

            btnVoid.Enabled = false;
            btnCloseContract.Enabled = false;
            chkIsTransfer.Enabled = true;
            btnPrint.Enabled = false;
            calDateOfBirth.SelectedDate = DateTime.Today;
            txtCustomerBarcode.Text = String.Empty;                
            WebFormHelper.ClearTextBox(txtNotes, txtCustomerFirstName, txtCustomerLastName,
                txtFatherEmail, txtFatherName, txtFatherPhone, txtMotherEmail, txtMotherName, txtMotherPhone, txtBillingCardHolderID, txtBillingCardHolderName, txtBillingCardNo);
            txtDuesAmount.Value = 0;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();            
        }

        private void ResetFields()
        {
            chkGenerateNewBarcodeCustomer.Visible = true;
            hypLookUpCustomer.Visible = true;
            chkIsTransfer.Checked = false;
            lblBranch.Text = ddlFindBranch.SelectedItem.Text;
            lblContractNo.Text = "(Generated by System)";
            calDate.SelectedDate = DateTime.Today;
            calEffectiveDate.SelectedDate = DateTime.Today;
            chkGenerateNewBarcodeCustomer.Checked = true;
            ddlPackage.SelectedIndex = 0;
            ddlBillingType.SelectedIndex = 0;
            ddlArea.SelectedIndex = 0;
            lblStatus.Text = "Pending";
            txtCustomerBarcode.ReadOnly = false;
            gvwPackage.DataSource = null;
            gvwPackage.DataBind();
            btnVoid.Enabled = false;
            lblActiveDate.Text = "Not Active";
            txtDuesAmount.Value = 0;
            txtAdditionalDuesAmount.Value = 0;
            txtIDCardNo.Text = String.Empty;
            //calExpiredDate.Enabled = false;
            calNextDuesDate.SelectedDate = calEffectiveDate.SelectedDate.GetValueOrDefault().AddMonths(1);
            txtHomePhone.Text = txtWorkPhone.Text = txtCellPhone.Text = String.Empty;
            WebFormHelper.ClearTextBox(txtFatherName, txtFatherPhone, txtIDCardNoFather, txtIDCardNoMother, txtMotherName, txtMotherPhone);
            chkFather.Checked = false;
            chkMother.Checked = false;
            chkFatherBirthDateUnknown.Checked = true;
            chkMotherBirthDateUnknown.Checked = true;
            ddlMonthlyDuesItem.SelectedIndex = 0;
            ddlRenewalOrUpgrade.SelectedIndex = 0;
            ddlOccupation.SelectedIndex = 0;
            lblClosedDate.Text = "(Generated by System)";
            lblVoidDate.Text = "(Generated by System)";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Validate("AddEdit");
                if (IsValid)
                {
                    if (calDateOfBirth.SelectedDate > DateTime.Today)
                    {
                        DynamicControlBinding.SetLabelTextWithCssClass(
                                    lblMessageAddEdit,
                                    "Invalid date of birth",
                                    LabelStyleNames.ErrorMessage);
                        return;
                    }

                    if (ddlBillingType.SelectedItem.Text.ToUpper() == "AUTO PAYMENT")
                    {
                        cuvCreditCardNo.Validate();
                        if (cuvCreditCardNo.IsValid)
                        {
                            if (ddlBillingCardType.SelectedIndex == 0 ||
                                ddlBillingBank.SelectedIndex == 0 ||
                               String.IsNullOrEmpty(txtBillingCardHolderID.Text) ||
                               String.IsNullOrEmpty(txtBillingCardHolderName.Text) ||
                               String.IsNullOrEmpty(txtBillingCardNo.Text))
                            {
                                DynamicControlBinding.SetLabelTextWithCssClass(
                                    lblMessageAddEdit,
                                    "Auto payment information is incomplete",
                                    LabelStyleNames.ErrorMessage);
                                return;
                            }
                        }

                        if (txtDuesAmount.Text.ToDefaultNumber<decimal>() == 0 ||
                            ddlMonthlyDuesItem.SelectedValue.ToDefaultNumber<int>() == 0)
                        {
                            DynamicControlBinding.SetLabelTextWithCssClass(
                                    lblMessageAddEdit,
                                    "Both <b>Dues Amount</b> and <b>Monthly Dues Item</b> fields must be specified if billing type is manual payment",
                                    LabelStyleNames.ErrorMessage);
                            return;
                        }

                    }

                    if (chkGenerateNewBarcodeCustomer.Checked)
                    {
                        if (chkFather.Checked && (
                            String.IsNullOrEmpty(txtFatherName.Text)))
                        {
                            DynamicControlBinding.SetLabelTextWithCssClass(
                                lblStatusParent,
                                "Parent Information (Father) is incomplete", cssClass: LabelStyleNames.ErrorMessage);
                            return;
                        }

                        if (chkMother.Checked && (
                            String.IsNullOrEmpty(txtMotherName.Text)))
                        {
                            DynamicControlBinding.SetLabelTextWithCssClass(
                                lblStatusParent,
                                "Parent Information (Mother) is incomplete",
                                LabelStyleNames.ErrorMessage);
                            return;
                        }
                    }

                    switch (RowID)
                    {
                        case 0:
                            ContractService.Add(
                                calDate.SelectedDate.GetValueOrDefault(),
                                !chkGenerateNewBarcodeCustomer.Checked,
                                txtCustomerBarcode.Text,
                                chkIsTransfer.Checked,
                                txtCustomerFirstName.Text,
                                txtCustomerLastName.Text,
                                calDateOfBirth.SelectedDate.GetValueOrDefault(),
                                txtIDCardNo.Text,
                                ddlOccupation.SelectedIndex == 0 ? (int?)null :Convert.ToInt32(ddlOccupation.SelectedValue),
                                Convert.ToInt32(ddlFindBranch.SelectedValue),
                                Convert.ToInt32(ddlPackage.SelectedValue),
                                null,
                                calEffectiveDate.SelectedDate.GetValueOrDefault(),
                                Convert.ToInt32(ddlBillingType.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingCardType.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingBank.SelectedValue),
                                txtBillingCardNo.Text,
                                txtBillingCardHolderName.Text,
                                txtBillingCardHolderID.Text,
                                new DateTime(Convert.ToInt32(ddlCardExpiredYear.SelectedValue),
                                             Convert.ToInt32(ddlCardExpiredMonth.SelectedValue),
                                             DateTime.DaysInMonth(Convert.ToInt32(ddlCardExpiredYear.SelectedValue),
                                                                  Convert.ToInt32(ddlCardExpiredMonth.SelectedValue))),
                                'P',
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlMonthlyDuesItem.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToDecimal(txtDuesAmount.Value),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToDecimal(txtAdditionalDuesAmount.Value),
                                calNextDuesDate.SelectedDate.GetValueOrDefault(),
                                calExpiredDate.SelectedDate.GetValueOrDefault(),
                                txtHomePhone.Text,
                                txtCellPhone.Text,
                                txtWorkPhone.Text,
                                txtMailingAddress.Text,
                                txtMailingZipCode.Text,
                                txtAddress.Text,
                                txtZipCode.Text,
                                Convert.ToInt32(ddlArea.SelectedValue),
                                chkFather.Checked,
                                txtFatherName.Text,
                                txtIDCardNoFather.Text,
                                chkFatherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateFather.SelectedDate,
                                txtFatherPhone.Text,
                                txtFatherEmail.Text,
                                chkMother.Checked,
                                txtMotherName.Text,
                                txtIDCardNoMother.Text,
                                chkMotherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateMother.SelectedDate,
                                txtMotherPhone.Text,
                                txtMotherEmail.Text,
                                txtNotes.Text,
                                ddlRenewalOrUpgrade.SelectedValue
                                );
                            break;
                        default:
                            ContractService.Update(
                                RowID,
                                Convert.ToInt32(ddlPackage.SelectedValue),
                                calDate.SelectedDate.GetValueOrDefault(),
                                calEffectiveDate.SelectedDate.GetValueOrDefault(),
                                txtIDCardNo.Text,
                                ddlOccupation.SelectedIndex == 0 ? (int?)null : Convert.ToInt32(ddlOccupation.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingCardType.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingBank.SelectedValue),
                                txtBillingCardNo.Text,
                                txtBillingCardHolderName.Text,
                                txtBillingCardHolderID.Text,
                                new DateTime(Convert.ToInt32(ddlCardExpiredYear.SelectedValue),
                                             Convert.ToInt32(ddlCardExpiredMonth.SelectedValue),
                                             DateTime.DaysInMonth(Convert.ToInt32(ddlCardExpiredYear.SelectedValue),
                                                                  Convert.ToInt32(ddlCardExpiredMonth.SelectedValue))),
                                txtHomePhone.Text,
                                txtCellPhone.Text,
                                txtWorkPhone.Text,
                                txtMailingAddress.Text,
                                txtMailingZipCode.Text,
                                txtAddress.Text,
                                txtZipCode.Text,
                                Convert.ToInt32(ddlArea.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlMonthlyDuesItem.SelectedValue),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToDecimal(txtDuesAmount.Value.GetValueOrDefault()),
                                Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToDecimal(txtAdditionalDuesAmount.Value.GetValueOrDefault()),
                                calNextDuesDate.SelectedDate.GetValueOrDefault(),
                                calExpiredDate.SelectedDate.GetValueOrDefault(),
                                chkFather.Checked,
                                txtFatherName.Text,
                                txtIDCardNoFather.Text,
                                chkFatherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateFather.SelectedDate,
                                txtFatherPhone.Text,
                                txtFatherEmail.Text,
                                chkMother.Checked,
                                txtMotherName.Text,
                                txtIDCardNoMother.Text,
                                chkMotherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateMother.SelectedDate,
                                txtMotherPhone.Text,
                                txtMotherEmail.Text,
                                txtNotes.Text
                                );
                            break;
                    }
                    Refresh();
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
                    FillDropDown(false);
                    int id = Convert.ToInt32(e.CommandArgument);
                    RowID = id;
                    var employee = EmployeeService.Get(User.Identity.Name);

                    mvwForm.SetActiveView(viwAddEdit);
                    chkGenerateNewBarcodeCustomer.Visible = false;
                    Contract contract = ContractService.Get(id);
                    chkGenerateNewBarcodeCustomer.Checked = false;
                    lblBranch.Text = contract.Branch.Name;
                    lblContractNo.Text = contract.ContractNo;
                    calDate.SelectedDate = contract.Date;
                    calDateOfBirth.SelectedDate = contract.Customer.DateOfBirth.HasValue ? contract.Customer.DateOfBirth.Value : DateTime.Today;
                    txtCustomerBarcode.Text = contract.Customer.Barcode;
                    txtCustomerBarcode.ReadOnly = true;
                    lblCustomerName.Text = String.Format("{0} {1}", contract.Customer.FirstName.Trim(), contract.Customer.LastName.Trim());
                    ddlPackage.SelectedValue = contract.PackageID.ToString();
                    ddlPackage_SelectedIndexChanged(sender, null);
                    txtHomePhone.Text = contract.Customer.HomePhone;
                    txtCellPhone.Text = contract.Customer.CellPhone1;
                    txtWorkPhone.Text = contract.Customer.WorkPhone;
                    calEffectiveDate.SelectedDate = contract.EffectiveDate;

                    ddlBillingType.SelectedValue = contract.BillingTypeID.ToString();

                    ddlBillingCardType.SelectedValue = contract.Customer.CreditCardTypeID.ToString();
                    ddlBillingBank.SelectedValue = contract.Customer.BankID.ToString();
                    txtBillingCardNo.Text = contract.Customer.CardNo;
                    txtBillingCardHolderName.Text = contract.Customer.CardHolderName;
                    txtBillingCardHolderID.Text = contract.Customer.CardHolderID;

                    if (contract.Customer.ExpiredDate.HasValue)
                    {
                        ddlCardExpiredMonth.SelectedValue = contract.Customer.ExpiredDate.Value.Month.ToString();
                        ddlCardExpiredYear.SelectedValue = contract.Customer.ExpiredDate.Value.Year.ToString();
                    }

                    txtMailingAddress.Text = contract.Customer.MailingAddress;
                    txtMailingZipCode.Text = contract.Customer.MailingZipCode;
                    txtAddress.Text = contract.Customer.Address;
                    txtZipCode.Text = contract.Customer.ZipCode;
                    txtIDCardNo.Text = contract.Customer.IDCardNo;
                    if(contract.Customer.OccupationID.HasValue)
                        ddlOccupation.SelectedValue = Convert.ToString(contract.Customer.OccupationID);
                    else
                        ddlOccupation.SelectedIndex = 0;

                    if (ddlArea.Items.FindByValue(contract.Customer.AreaID.ToString()) != null)
                        ddlArea.SelectedValue = contract.Customer.AreaID.ToString();
                    else
                        ddlArea.SelectedIndex = 0;
                    lblStatus.Text = ContractService.DecodeStatus(Convert.ToChar(contract.Status));
                    txtNotes.Text = contract.Notes;

                    lblActiveDate.Text = contract.ActiveDate.HasValue ? contract.ActiveDate.Value.ToString("dddd, dd MMMM yyyy") : "Not Active";

                    btnVoid.Enabled = contract.Status == "A";

                    if (EmployeeService.Get(User.Identity.Name).CanEditActiveContract)
                        btnSave.Enabled = true;
                    else
                        btnSave.Enabled = contract.Status == "P";

                    lblClosedDate.Text = contract.ClosedDate.HasValue ? contract.ClosedDate.Value.ToString("dddd, dd MMMM yyyy") : "This contract has not been closed";
                    lblVoidDate.Text = contract.VoidDate.HasValue ? contract.VoidDate.Value.ToString("dddd, dd MMMM yyyy") : "This contract has not been void";
                    calExpiredDate.Enabled = true;
                    calExpiredDate.SelectedDate = contract.ExpiredDate;

                    ddlMonthlyDuesItem.SelectedValue = Convert.ToString(contract.BillingItemID);
                    calNextDuesDate.SelectedDate = contract.NextDuesDate.GetValueOrDefault();
                    txtDuesAmount.Value = Convert.ToDouble(contract.DuesAmount);
                    txtAdditionalDuesAmount.Value = Convert.ToDouble(contract.AdditionalDuesAmount);

                    Person father = contract.Customer.People.SingleOrDefault(p => p.Connection == 'F');
                    chkFather.Checked = father != null;
                    if (father != null)
                    {
                        txtFatherName.Text = father.Name;
                        txtFatherPhone.Text = father.Phone1;
                        txtIDCardNoFather.Text = father.IDCardNo;
                        txtFatherEmail.Text = father.Email;
                        chkFatherBirthDateUnknown.Checked = !father.BirthDate.HasValue;
                        if (father.BirthDate.HasValue)
                            calBirthDateFather.SelectedDate = father.BirthDate.GetValueOrDefault();
                    }

                    Person mother = contract.Customer.People.SingleOrDefault(p => p.Connection == 'M');
                    chkMother.Checked = mother != null;
                    if (mother != null)
                    {
                        txtMotherName.Text = mother.Name;
                        txtMotherPhone.Text = mother.Phone1;
                        txtIDCardNoMother.Text = mother.IDCardNo;
                        txtMotherEmail.Text = mother.Email;
                        chkMotherBirthDateUnknown.Checked = !mother.BirthDate.HasValue;
                        if (mother.BirthDate.HasValue)
                            calBirthDateMother.SelectedDate = mother.BirthDate.GetValueOrDefault();
                    }

                    btnVoid.Enabled = !contract.VoidDate.HasValue;
                    btnCloseContract.Enabled = !contract.VoidDate.HasValue;

                    btnVoid.Enabled = !contract.ClosedDate.HasValue;
                    btnCloseContract.Enabled = !contract.ClosedDate.HasValue;

                    if (!String.IsNullOrEmpty(Convert.ToString(contract.ContractType)))
                    {
                        chkIsTransfer.Checked = contract.ContractType == 'T';

                        if (contract.ContractType != 'T')
                            ddlRenewalOrUpgrade.SelectedValue = Convert.ToString(contract.ContractType);
                    }
                    ddlRenewalOrUpgrade.Enabled = false;
                    chkIsTransfer.Enabled = false;

                    ClientScript.RegisterStartupScript(this.GetType(), "_cust", "$(document).ready(function() { $('#customer').show(); });", true);
                    hypLookUpCustomer.Visible = false;

                    var printPreviewUrl = ResolveUrl("~/Reports/PrintPreview.aspx");
                    btnPrint.Attributes["onclick"] = String.Format("showSimplePopUp('{0}?RDL=AgreementForm&ContractNo={1}')",
                            printPreviewUrl, lblContractNo.Text);
                    btnPrint.Enabled = true;

                    btnCloseContract.Enabled = employee.CanEditActiveContract;
                    btnVoid.Enabled = employee.CanEditActiveContract;
                }
            }
            catch (Exception ex)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@ContractNo"].Value = txtFindContractNo.Text;
            e.Command.Parameters["@CustomerName"].Value = txtFindCustomerName.Text;
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }
        protected void cuvExistingCustomer_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (chkGenerateNewBarcodeCustomer.Checked)
                args.IsValid = true;
            else
                args.IsValid = txtCustomerBarcode.Text.Trim().Length > 0 && CustomerService.IsExist(txtCustomerBarcode.Text);
        }
        protected void cuvNewCustomer_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (chkGenerateNewBarcodeCustomer.Checked)
                args.IsValid = txtCustomerFirstName.Text.Trim().Length > 0 && txtCustomerLastName.Text.Trim().Length > 0;
            else
                args.IsValid = true;
        }
        protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var package = PackageService.GetDetail(Convert.ToInt32(ddlPackage.SelectedValue));
            gvwPackage.DataSource = package;
            gvwPackage.DataBind();
        }

        protected void gvwPackage_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
        }

        protected void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                var list = ContractService.GetActiveInvoices(lblContractNo.Text).ToList();
                string invoices = String.Empty;
                list.ForEach(inv => invoices += @"<li>" + inv.InvoiceNo + @"</li");
                if (list.Any())
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblMessageAddEdit,
                        @"This contract has invoice already, please void invoice first: <ul>" + invoices + "</ul>",
                        LabelStyleNames.ErrorMessage);
                }
                else
                {
                    ContractService.VoidContract(lblContractNo.Text);
                    mvwForm.SetActiveView(viwRead);
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblMessage,
                        @"Contract <b> " + lblContractNo.Text + "</b> has been processed as VOID",
                        LabelStyleNames.AlternateMessage);
                    btnVoid.Enabled = false;
                    btnCloseContract.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
        protected void calEffectiveDate_DateChanged(object sender, EventArgs e)
        {
            calNextDuesDate.SelectedDate = calEffectiveDate.SelectedDate.GetValueOrDefault().AddMonths(1);
        }
        protected void cuvCreditCardNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Trim().Length == 16 && ValidationHelper.IsValidCreditCardNumber(args.Value);
        }
        protected void btnCloseContract_Click(object sender, EventArgs e)
        {
            try
            {
                ContractService.CloseContract(lblContractNo.Text);
                btnVoid.Enabled = false;
                btnCloseContract.Enabled = false;
                WebFormHelper.SetLabelTextWithCssClass(
                        lblMessage,
                        @"Contract <b> " + lblContractNo.Text + "</b> has been processed as CLOSED",
                        LabelStyleNames.AlternateMessage);
                mvwForm.SetActiveView(viwRead);
            }
            catch (Exception ex)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hypSalesPoints = e.Row.FindControl("hypSalesPoints") as HyperLink;                
                if (hypSalesPoints != null)
                {
                    var contractID = Convert.ToInt32(e.Row.Cells[0].Text);
                    hypSalesPoints.Attributes.Add("onclick",
                        String.Format("showPromptPopUp('EntrySalesPoint.aspx?ContractID={0}', null, 600, 1200)",
                            contractID));
                }

                var hypPrint = e.Row.FindControl("hypPrint") as HyperLink;
                if(hypPrint != null)
                {
                    string contractNo = e.Row.Cells[1].Text;
                    var printPreviewUrl = ResolveUrl("~/Reports/PrintPreview.aspx");
                    hypPrint.Attributes.Add("onclick", String.Format("showSimplePopUp('{0}?RDL=AgreementForm&ContractNo={1}')", printPreviewUrl, contractNo));
                }
            }
        }
    }
}