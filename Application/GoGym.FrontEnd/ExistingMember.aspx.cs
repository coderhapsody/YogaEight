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
using GoGym.Providers.ViewModels;
using GoGym.Utilities.Extensions;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ExistingMember : BaseForm
    {
        [Inject]
        public CreditCardTypeProvider CreditCardTypeService { get; set; }
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public ItemProvider ItemService { get; set; }
        [Inject]
        public ItemTypeProvider ItemTypeService { get; set; }
        [Inject]
        public PaymentTypeProvider PaymentTypeService { get; set; }
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }

        public List<InvoiceDetailViewModel> _InvoiceDetail
        {
            get
            {
                return ViewState["InvoiceDetail"] as List<InvoiceDetailViewModel>;
            }
            set
            {
                if (ViewState["InvoiceDetail"] == null)
                    ViewState["InvoiceDetail"] = new List<InvoiceDetailViewModel>();
                ViewState["InvoiceDetail"] = value;
            }
        }

        public bool ExcludePayment
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ViewState["ExPayment"]);
                }
                catch
                {
                    ViewState["ExPayment"] = "0";
                    return false;
                }
            }
            set
            {
                if (ViewState["ExPayment"] == null)
                    ViewState["ExPayment"] = "0";
                ViewState["ExPayment"] = value;
            }
        }


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
            if (!this.IsPostBack)
            {
                FillDropDown();
                _InvoiceDetail = new List<InvoiceDetailViewModel>();
                _PaymentDetail = new List<PaymentDetailViewModel>();
                CalculateTotalInvoiceAndPayment();
                ExcludePayment = !String.IsNullOrEmpty(Request["ExPayment"]);
                pnlPayment.Visible = !ExcludePayment;

                ddlItemType_SelectedIndexChanged(sender, e);
                ddlItem_SelectedIndexChanged(sender, e);

                ddlBranch.Enabled = false;

                calDate.SelectedDate = DateTime.Today;
                hypLookUpCustomer.Attributes["onclick"] =
                    String.Format("showPromptPopUp('PromptCustomer.aspx?BranchID=' + $find('ddlBranch').get_selectedItem().get_value(), '{0}', 550, 900);", txtCustomerCode.ClientID);
            }
        }

        private void CalculateTotalInvoiceAndPayment()
        {
            lblTotalInvoice.Text = (_InvoiceDetail.Any() ?
                _InvoiceDetail.Sum(row => ((row.UnitPrice * row.Quantity) - row.Discount / 100 * (row.UnitPrice * row.Quantity))) - 0 : 0).ToString("###,##0.00");

            lblTotalPayment.Text = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M).ToString("###,##0.00");
        }

        private void FillDropDown()
        {
            ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Enabled = ddlBranch.Items.Count > 0;
            
            ddlItemType.DataSource = ItemTypeService.GetAll();
            ddlItemType.DataTextField = "Description";
            ddlItemType.DataValueField = "ID";
            ddlItemType.DataBind();

            ddlPaymentType.DataSource = PaymentTypeService.GetAll();
            ddlPaymentType.DataTextField = "Description";
            ddlPaymentType.DataValueField = "ID";
            ddlPaymentType.DataBind();

            ddlBranch_SelectedIndexChanged(null, null);
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDiscount.Text))
                    txtDiscount.Text = "0";

                int itemID = Convert.ToInt32(ddlItem.SelectedValue);
                Item item = ItemService.Get(itemID);
                if (_InvoiceDetail.All(detail => detail.ItemID != itemID))
                {
                    _InvoiceDetail.Add(
                        new InvoiceDetailViewModel()
                        {
                            ID = _InvoiceDetail.Any() ? _InvoiceDetail.Max(inv => inv.ID) + 1 : 1,
                            InvoiceID = RowID,
                            ItemID = item.ID,
                            ItemBarcode = item.Barcode,
                            ItemDescription = item.Description,
                            Quantity = Convert.ToInt32(txtQuantity.Text),
                            UnitName = ddlUnit.SelectedItem.Text,
                            UnitPrice = Convert.ToDecimal(txtUnitPrice.Text),
                            Discount = Convert.ToDecimal(txtDiscount.Text),
                            IsTaxable = chkIsTaxable.Checked,
                            NetAmount = ((Convert.ToInt32(txtQuantity.Text) *
                                          Convert.ToDecimal(txtUnitPrice.Text) -
                                          (Convert.ToDecimal(txtDiscount.Text) / 100) *
                                          (Convert.ToInt32(txtQuantity.Text) *
                                           Convert.ToDecimal(txtUnitPrice.Text))) /
                                         (chkIsTaxable.Checked ? 1.1M : 1M)),
                            Total = (Convert.ToInt32(txtQuantity.Text) *
                                     Convert.ToDecimal(txtUnitPrice.Text) -
                                     (Convert.ToDecimal(txtDiscount.Text) / 100) * (Convert.ToInt32(txtQuantity.Text) *
                                                                                    Convert.ToDecimal(txtUnitPrice.Text)))
                        });

                    WebFormHelper.ClearTextBox(txtQuantity, txtDiscount, txtUnitPrice);
                    ddlItemType.SelectedIndex = 0;
                    ddlItem.SelectedIndex = 0;
                    gvwOtherPurchase.DataSource = _InvoiceDetail;
                    gvwOtherPurchase.DataBind();                    
                    CalculateTotalInvoiceAndPayment();
                }
            }
            catch(Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void gvwOtherPurchase_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void gvwOtherPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                _InvoiceDetail.RemoveAll(inv => inv.ID == id);
                gvwOtherPurchase.DataSource = _InvoiceDetail;
                gvwOtherPurchase.DataBind();
            }
            CalculateTotalInvoiceAndPayment();
        }

        protected void btnAddPayment_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4 &&
               Convert.ToInt32(ddlCreditCardType.SelectedValue) == 0)
            {
                return;
            }
            _PaymentDetail.Add(
                new PaymentDetailViewModel()
                {
                    ID = _PaymentDetail.Any() ? _PaymentDetail.Max(pay => pay.ID) + 1 : 1,
                    PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue),
                    PaymentType = ddlPaymentType.SelectedItem.Text,
                    CreditCardTypeID = ddlCreditCardType.SelectedValue.ToDefaultNumber<int>() == 0 ? (int?)null : ddlCreditCardType.SelectedValue.ToDefaultNumber<int>(),
                    CreditCardType = ddlCreditCardType.SelectedItem == null ? String.Empty : ddlCreditCardType.SelectedItem.Text,
                    ApprovalCode = txtApprovalCode.Text,
                    Amount = Convert.ToDecimal(txtPaymentAmount.Text),
                    Notes = txtPaymentNotes.Text
                });
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

        private void _CalculateTotalInvoiceAndPayment(out decimal totalInvoice, out decimal totalPayment)
        {
            totalInvoice = (_InvoiceDetail.Any() ?
                _InvoiceDetail.Sum(row => ((row.UnitPrice * row.Quantity) - row.Discount / 100 * (row.UnitPrice * row.Quantity))) : 0);

            totalPayment = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("ExistingMember");
            if (Page.IsValid)
            {
                if (!ExcludePayment)
                {
                    decimal totalInvoice = 0;
                    decimal totalPayment = 0;
                    _CalculateTotalInvoiceAndPayment(out totalInvoice, out totalPayment);
                    if (totalInvoice != totalPayment || (totalInvoice == 0 && totalPayment == 0))
                    {
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblStatus,
                            "Total Invoice is not equal Total Payment. Please verify this transaction again",
                            LabelStyleNames.ErrorMessage);
                        return;
                    }
                }

                try
                {
                    string invoiceNo = InvoiceService.CreateExistingMemberInvoice(
                        Convert.ToInt32(ddlBranch.SelectedValue),
                        calDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                        txtCustomerCode.Text,
                        Convert.ToInt32(ddlSales.SelectedValue),
                        txtNotes.Text,
                        0,
                        _InvoiceDetail,
                        _PaymentDetail);

                    Response.Redirect(String.Format("ExistingMemberCompleted.aspx?InvoiceNo={0}", invoiceNo));
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
        protected void txtDiscountValue_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalInvoiceAndPayment();
        }


        protected void ddlItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int itemTypeID = Convert.ToInt32(ddlItemType.SelectedValue);
            ddlItem.Items.Clear();
            foreach (var item in ItemService.GetItemsByType(itemTypeID))
            {
                ddlItem.Items.Add(
                    new DropDownListItem(
                        item.Description,
                        item.ID.ToString()));
            }
            ddlItem.SelectedIndex = 0;
            ddlItem_SelectedIndexChanged(sender, e);
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
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            var units = ItemService.GetItemUnits(ddlItem.SelectedValue.ToDefaultNumber<int>());
            var item = ItemService.GetItem(ddlItem.SelectedValue.ToDefaultNumber<int>());

            if (units != null)
            {
                ddlUnit.Items.Clear();
                foreach (var unit in units)
                    ddlUnit.Items.Add(new DropDownListItem(unit.Trim(), unit.Trim()));
            }

            if (item != null)
            {
                chkIsTaxable.Checked = item.IsTaxed;
            }
            ddlUnit.SelectedIndex = 0;
        }
    }
}