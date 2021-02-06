using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
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
    public partial class MasterCustomer : BaseForm
    {
        [Inject]
        public BillingTypeProvider BillingTypeService { get; set; }
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
        [Inject]
        public CustomerNotesProvider CustomerNotesService { get; set; }
        [Inject]
        public OccupationProvider OccupationService { get; set; }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(Request["mode"]))
            {
                MasterPageFile = "~/MasterPages/MasterPrompt.Master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                FillDropDown();
            }

            if (!String.IsNullOrEmpty(Request["mode"]))
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
                mvwForm.ActiveViewIndex = 1;                
                Customer customer = CustomerService.Get(Request["barcode"]);
                if (customer != null)
                {
                    LoadData(customer.ID);
                    hypLookUpPartner.Enabled = hypParent.Enabled = false;
                    gvwActiveContracts.Enabled = false;
                }
            }

            hypLookUpPartner.Attributes["onclick"] = String.Format("showPromptPopUp('PromptCustomer.aspx?BranchLocked=1', '{0}', 550, 900);", txtPartner.ClientID);
        }

        private void FillDropDown()
        {
            ddlFindBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();

            ddlArea.DataSource = AreaService.GetAll();
            ddlArea.DataTextField = "Description";
            ddlArea.DataValueField = "ID";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new DropDownListItem(String.Empty));

            ddlCreditCardType.DataSource = CreditCardTypeService.GetAll();
            ddlCreditCardType.DataTextField = "Description";
            ddlCreditCardType.DataValueField = "ID";
            ddlCreditCardType.DataBind();

            ddlOccupation.DataSource = OccupationService.GetAll();
            ddlOccupation.DataTextField = "Description";
            ddlOccupation.DataValueField = "ID";
            ddlOccupation.DataBind();
            ddlOccupation.Items.Insert(0, new DropDownListItem(String.Empty));

            ddlBank.DataSource = BankService.GetActiveBanks();
            ddlBank.DataTextField = "Name";
            ddlBank.DataValueField = "ID";
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, String.Empty);

            DataBindingHelper.PopulateBillingTypes(BillingTypeService, ddlBillingType, false);
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

            txtCardHolderID.ReadOnly = txtCardHolderName.ReadOnly = txtCardNo.ReadOnly = false;
            calExpiredDate.Enabled = ddlBank.Enabled = ddlCreditCardType.Enabled = true;
            WebFormHelper.ClearRadTextBox(txtFirstName, txtLastName, txtEmail, txtAddress, txtCardNo, txtPhone, txtZipCode);
            txtBarcode.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            CustomerService.Delete(id);
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
                    switch (RowID)
                    {
                        case 0:
                            //customerProvider.Add(
                            //    txtBarcode.Text, 
                            //    txtFirstName.Text, 
                            //    txtLastName.Text, 
                            //    txtAddress.Text,
                            //    txtZipCode.Text, txtEmail.Text, txtPhone.Text, 
                            //    Convert.ToInt32(ddlHomeBranch.SelectedValue), 
                            //    Convert.ToInt32(ViewState["ActiveContractID"]),
                            //    Convert.ToInt32(ddlArea.SelectedValue),
                            //    Convert.ToInt32(hidPartner.Value),
                            //    Convert.ToInt32(hidParent.Value),
                            //    txtCardNo.Text,
                            //    Convert.ToInt32(ddlPaymentType.SelectedValue),
                            //    true,
                            //    Convert.ToInt32(hidStatus.Value));                    
                            break;
                        default:
                            FileInfo fi = null;
                            string fileName = null;

                            if (fupPhoto.HasFile && !chkDeletePhoto.Checked)
                            {
                                fi = new FileInfo(fupPhoto.FileName);
                                fileName = chkDeletePhoto.Checked ? null : Guid.NewGuid().ToString().ToUpper() + fi.Extension;
                                fupPhoto.SaveAs(Server.MapPath(ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoCustomers]) + @"\" + fileName);
                            }
                            else
                                fileName = Convert.ToString(ViewState["Photo"]);

                            Customer partner = CustomerService.Get(txtPartner.Text);
                            int partnerID = partner == null ? 0 : partner.ID;

                            CustomerService.Update(
                                RowID,
                                txtBarcode.Text,
                                txtFirstName.Text,
                                txtLastName.Text,
                                txtSurname.Text,
                                calDate.SelectedDate.GetValueOrDefault(),
                                txtAddress.Text,
                                txtZipCode.Text,
                                txtIDCardNo.Text,
                                ddlOccupation.SelectedIndex == 0 ? (int?) null : Convert.ToInt32(ddlOccupation.SelectedValue),
                                txtMailingAddress.Text, 
                                txtMailingZipCode.Text,
                                txtEmail.Text,
                                txtPhone.Text,
                                txtCellPhone.Text,
                                txtCellPhone2.Text,
                                txtWorkPhone.Text,
                                ddlArea.SelectedValue.ToDefaultNumber<int>(),
                                partnerID,
                                ddlBillingType.SelectedValue.ToDefaultNumber<int>(),
                                txtCardNo.Text,
                                Convert.ToInt32(ddlCreditCardType.SelectedValue),
                                txtCardHolderName.Text,
                                txtCardHolderID.Text,
                                ddlBank.SelectedValue.ToDefaultNumber<int>(),
                                calExpiredDate.SelectedDate.GetValueOrDefault(),
                                chkDeletePhoto.Checked,
                                fileName);

                            if (!txtCardNo.ReadOnly && Convert.ToInt32(ddlBillingType.SelectedValue) != 1)
                                CustomerService.UpdateCreditCardInfo(
                                    txtBarcode.Text,
                                    Convert.ToInt32(ddlCreditCardType.SelectedValue),
                                    ddlBank.SelectedValue.ToDefaultNumber<int>(),
                                    txtCardHolderName.Text,
                                    txtCardHolderID.Text,
                                    txtCardNo.Text,
                                    calExpiredDate.SelectedDate.GetValueOrDefault(),
                                    "Switch to auto payment");


                            break;
                    }
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                mvwForm.ActiveViewIndex = 0;
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
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                LoadData(id);
            }
        }

        private void LoadData(int customerID)
        {
            Customer cust = CustomerService.Get(customerID);
            txtBarcode.ReadOnly = true;
            txtFirstName.Text = cust.FirstName;
            txtLastName.Text = cust.LastName;
            txtBarcode.Text = cust.Barcode;
            txtAddress.Text = cust.Address;
            ddlBillingType.SelectedValue = cust.BillingTypeID.ToString();
            txtCardNo.Text = cust.CardNo;
            txtCardHolderName.Text = cust.CardHolderName;
            txtCardHolderID.Text = cust.CardHolderID;
            if (cust.BankID.HasValue)
                ddlBank.SelectedValue = cust.BankID.ToString();

            if (cust.ExpiredDate.HasValue)
                calExpiredDate.SelectedDate = cust.ExpiredDate.Value;
            calDate.SelectedDate = cust.DateOfBirth.HasValue ? cust.DateOfBirth.Value : DateTime.Today;
            txtEmail.Text = cust.Email;
            txtPhone.Text = cust.HomePhone;
            txtCellPhone.Text = cust.CellPhone1;
            txtCellPhone2.Text = cust.CellPhone2;
            txtWorkPhone.Text = cust.WorkPhone;
            txtZipCode.Text = cust.ZipCode;
            txtIDCardNo.Text = cust.IDCardNo;
            if(cust.OccupationID.HasValue)
                ddlOccupation.SelectedValue = Convert.ToString(cust.OccupationID);
            else
                ddlOccupation.SelectedIndex = 0;
            txtMailingAddress.Text = cust.MailingAddress;
            txtMailingZipCode.Text = cust.MailingZipCode;
            txtPartner.Text = cust.Customer1 == null ? String.Empty : cust.Customer1.Barcode;
            if (cust.CreditCardTypeID.HasValue)
                ddlCreditCardType.SelectedValue = cust.CreditCardTypeID.ToString();
            if (cust.AreaID.HasValue)
                ddlArea.SelectedValue = cust.AreaID.ToString();
            else
                ddlArea.SelectedIndex = 0;
            lblHomeBranch.Text = cust.Branch.Name;
            ViewState["Photo"] = cust.Photo;


            RefreshActiveContracts(cust.Barcode);

            CustomerStatusHistory customerStatus = CustomerStatusService.GetLatestStatus(cust.Barcode);
            lblStatus.Text = customerStatus == null ? "OK" : customerStatus.CustomerStatus.Description;
            lblStatusNotes.Text = customerStatus == null ? String.Empty : customerStatus.Notes;

            if (!String.IsNullOrEmpty(cust.Photo))
            {
                FileInfo file = new FileInfo(cust.Photo);
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoCustomers] + @"\" + file.Name.Substring(0, file.Name.IndexOf(".")) + file.Extension + ".ashx?w=200";
            }
            else
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoCustomers] + @"\default.png";
            chkDeletePhoto.Checked = false;

            hypParent.Attributes["onclick"] = String.Format("window.open('MasterParents.aspx?CustomerCode={0}', 'Parent', 'width=800,height=500,location=no,resizable=yes')", txtBarcode.Text);
            hypViewStatusHistory.Attributes["onclick"] = String.Format("window.open('ViewCustomerStatusHistory.aspx?CustomerCode={0}', 'Parent', 'width=1100,height=500,location=no,resizable=yes')", txtBarcode.Text);

            gvwParents.DataBind();

            txtCardHolderID.ReadOnly = txtCardHolderName.ReadOnly = txtCardNo.ReadOnly = Convert.ToInt32(ddlBillingType.SelectedValue) == 3;
            calExpiredDate.Enabled = ddlBank.Enabled = ddlCreditCardType.Enabled = !txtCardNo.ReadOnly;

            txtBarcode.Focus();
        }

        private void RefreshActiveContracts(string barcode)
        {
            List<Contract> listContract = ContractService.GetActiveContracts(barcode).ToList();
            gvwActiveContracts.DataSource = from contract in listContract
                                            orderby contract.ClosedDate ascending, contract.VoidDate ascending
                                            select new
                                            {
                                                ContractNo = contract.ContractNo,
                                                DuesAmount = contract.DuesAmount,
                                                Date = contract.Date,
                                                PurchaseDate = contract.PurchaseDate,
                                                EffectiveDate = contract.EffectiveDate,
                                                NextDuesDate = contract.NextDuesDate,
                                                ExpiredDate = contract.ExpiredDate,
                                                Status = ContractService.DecodeStatus(Convert.ToChar(contract.Status))
                                            };
            gvwActiveContracts.DataBind();
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
            e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
            e.Command.Parameters["@Name"].Value = txtFindName.Text;
            e.Command.Parameters["@ParentName"].Value = txtFindParentName.Text;
            e.Command.Parameters["@PhoneNo"].Value = txtFindPhoneNo.Text;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwMaster.DataBind();
        }

        protected void cuvPartner_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Trim().Length > 0 && CustomerService.IsExist(args.Value);
        }
        protected void sdsParents_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@CustomerBarcode"].Value = txtBarcode.Text;
        }
        protected void cuvCardNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Convert.ToInt32(ddlBillingType.SelectedValue) == (int)BillingTypeEnum.ManualPayment)
                args.IsValid = true;
            else
                args.IsValid = !String.IsNullOrEmpty(txtCardNo.Text) && !String.IsNullOrEmpty(txtCardHolderName.Text);

        }
        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string custBarcode = e.Row.Cells[1].Text;

                var notes = CustomerNotesService.GetTopNotes(custBarcode);
                var txt = new StringBuilder();
                txt.AppendLine("Notes for " + custBarcode + ": ").AppendLine();
                foreach (var note in notes)
                    txt.AppendLine("* " + note.ChangedWhen.ToLongDateString() + " *").AppendLine(note.Notes).AppendLine();
                e.Row.ToolTip = txt.ToString();

                var hypNotes = e.Row.FindControl("hypNotes") as HyperLink;
                if (hypNotes != null)
                {
                    hypNotes.Attributes.Add("onclick", String.Format("showPromptPopUp('InputCustomerNotes.aspx?barcode={0}&candelete=1', null, 600, 900)", custBarcode));
                }


                var hypCheckInHistory = e.Row.FindControl("hypCheckInHistory") as HyperLink;
                if (hypCheckInHistory != null)
                {
                    hypCheckInHistory.Attributes.Add("onclick", String.Format("showPromptPopUp('CheckInHistory.aspx?barcode={0}', null, 600, 900)", custBarcode));
                }

                var hypInvoiceHistory = e.Row.FindControl("hypInvoiceHistory") as HyperLink;
                if (hypInvoiceHistory != null)
                {
                    hypInvoiceHistory.Attributes.Add("onclick", String.Format("showPromptPopUp('InvoiceHistory.aspx?barcode={0}', null, 600, 1200)", custBarcode));
                }

                var hypChangeCC = e.Row.FindControl("hypChangeCC") as HyperLink;
                if (hypChangeCC != null)
                {
                    hypChangeCC.Enabled = CustomerService.IsBillingTypeAutoPayment(custBarcode);
                    if (!hypChangeCC.Enabled)
                    {
                        hypChangeCC.Attributes.Add("onclick", String.Format("alert('Billing type for this customer is manual payment');"));
                        hypChangeCC.ToolTip = "Billing type for this customer is manual payment";
                    }
                    else
                        hypChangeCC.Attributes.Add("onclick", String.Format("showPromptPopUp('ChangeCreditCard.aspx?barcode={0}', null, 600, 1200)", custBarcode));
                }
            }
        }
        protected void gvwActiveContracts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandSource is LinkButton)
                {
                    int rowIndex = ((e.CommandSource as LinkButton).NamingContainer as GridViewRow).RowIndex;
                    string contractNo = Convert.ToString(e.CommandArgument);
                    if (e.CommandName == "EditContract")
                    {
                        gvwActiveContracts.EditIndex = rowIndex;
                    }
                    else if (e.CommandName == "CancelContract")
                    {
                        gvwActiveContracts.EditIndex = -1;
                    }
                    else if (e.CommandName == "SaveContract")
                    {
                        DateTime nextDuesDate = (gvwActiveContracts.Rows[rowIndex].Cells[7].Controls[1] as RadDatePicker).SelectedDate.GetValueOrDefault();
                        DateTime expiredDate = (gvwActiveContracts.Rows[rowIndex].Cells[6].Controls[1] as RadDatePicker).SelectedDate.GetValueOrDefault();
                        ContractService.UpdateNextDuesDate(contractNo, expiredDate, nextDuesDate);
                        gvwActiveContracts.EditIndex = -1;
                    }

                    RefreshActiveContracts(txtBarcode.Text);
                }
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatusNotes,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
        protected void gvwActiveContracts_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void cuvCreditCardNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = args.Value.Trim().Length == 16 && ValidationHelper.IsValidCreditCardNumber(args.Value);
        }

    }
}