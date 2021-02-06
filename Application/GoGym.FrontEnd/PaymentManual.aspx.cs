﻿using System;
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
    public partial class PaymentManual : BaseForm
    {
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }
        [Inject]
        public PaymentProvider PaymentService { get; set; }
        [Inject]
        public ContractProvider ContractService { get; set; }
        [Inject]
        public ItemProvider ItemService { get; set; }
        [Inject]
        public ItemTypeProvider ItemTypeService { get; set; }
        [Inject]
        public CreditCardTypeProvider CreditCardTypeService { get; set; }
        [Inject]
        public PaymentTypeProvider PaymentTypeService { get; set; }        

        public List<PaymentDetailViewModel> _PaymentDetail
        {
            get
            {
                if (ViewState["PaymentDetail"] == null)
                    ViewState["PaymentDetail"] = new List<PaymentDetailViewModel>();
                return ViewState["PaymentDetail"] as List<PaymentDetailViewModel>;
            }
            set
            {
                if (ViewState["PaymentDetail"] == null)
                    ViewState["PaymentDetail"] = new List<PaymentDetailViewModel>();
                ViewState["PaymentDetail"] = value;
            }
        }

        public List<InvoiceDetailViewModel> _InvoiceDetail
        {
            get
            {
                if (ViewState["InvoiceDetail"] == null)
                    ViewState["InvoiceDetail"] = new List<InvoiceDetailViewModel>();
                return ViewState["InvoiceDetail"] as List<InvoiceDetailViewModel>;
            }
            set
            {
                if (ViewState["InvoiceDetail"] == null)
                    ViewState["InvoiceDetail"] = new List<InvoiceDetailViewModel>();
                ViewState["InvoiceDetail"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                calInvoiceDate.SelectedDate = DateTime.Today;
                calPaymentDate.SelectedDate = DateTime.Today;
                string invoiceNo = Request["InvoiceNo"];
                LoadInvoice(invoiceNo);

                ddlPaymentType.DataSource = PaymentTypeService.GetAll();
                ddlPaymentType.DataTextField = "Description";
                ddlPaymentType.DataValueField = "ID";
                ddlPaymentType.DataBind();

                ddlItemType.DataSource = ItemTypeService.GetAll();
                ddlItemType.DataTextField = "Description";
                ddlItemType.DataValueField = "ID";
                ddlItemType.DataBind();
            }
        }

        private void LoadInvoice(string invoiceNo)
        {
            InvoiceHeader invoiceHeader = InvoiceService.GetInvoice(invoiceNo);
            if (invoiceHeader != null)
            {
                lblInvoiceNo.Text = invoiceHeader.InvoiceNo;
                lblCustomer.Text = String.Format("{0} - {1} {2}", invoiceHeader.Customer.Barcode, invoiceHeader.Customer.FirstName, invoiceHeader.Customer.LastName);
                lblNotes.Text = invoiceHeader.Notes;
                calInvoiceDate.SelectedDate = invoiceHeader.Date;
                calPaymentDate.SelectedDate = DateTime.Today;
                lblEmployee.Text = String.Format("{0} - {1} {2}", invoiceHeader.Employee.Barcode, invoiceHeader.Employee.FirstName, invoiceHeader.Employee.LastName);
                lblNotes.Text = invoiceHeader.Notes;
                //lblDiscountValue.Text = invoiceHeader.DiscountValue.ToString("###,##0.00");            
                IEnumerable<InvoiceDetailViewModel> invoiceDetail = _InvoiceDetail = InvoiceService.GetDetail(invoiceNo).ToList();
                gvwInvoice.DataSource = _InvoiceDetail;
                gvwInvoice.DataBind();

                decimal totalAfterTax = invoiceDetail.Any() ? invoiceDetail.Sum(inv => inv.Total) - invoiceHeader.DiscountValue : 0M;
                decimal totalBeforeTax = invoiceDetail.Any() ? invoiceDetail.Sum(i => ((i.UnitPrice * i.Quantity) - (i.Discount / 100 * (i.UnitPrice * i.Quantity))) / (i.IsTaxable ? 1.1M : 1M)) - invoiceHeader.DiscountValue : 0;
                lblTotalBeforeTax.Text = totalBeforeTax.ToString("###,##0.00");
                lblTotalInvoice.Text = totalAfterTax.ToString("###,##0.00");
                lblTotalTax.Text = (totalAfterTax - totalBeforeTax).ToString("###,##0.00");

                ViewState["_totalInvoice"] = totalAfterTax;
            }
        }

        private void CalculateTotalPayment()
        {
            decimal totalPayment = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M);
            lblTotalPayment.Text = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M).ToString("###,##0.00");
            ViewState["_totalPayment"] = totalPayment;
        }

        protected void btnAddPayment_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0)
            {
                _PaymentDetail.Add(
                    new PaymentDetailViewModel()
                    {
                        ID = _PaymentDetail.Any() ? _PaymentDetail.Max(pay => pay.ID) + 1 : 1,
                        PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue),
                        PaymentType = ddlPaymentType.SelectedItem.Text,
                        CreditCardTypeID = ddlCreditCardType.SelectedValue.ToDefaultNumber<int>() == 0 ? (int?)null : ddlCreditCardType.SelectedValue.ToDefaultNumber<int>(),
                        CreditCardType = ddlCreditCardType.SelectedItem == null ? String.Empty : ddlCreditCardType.SelectedItem.Text,
                        ApprovalCode = txtApprovalCode.Text,
                        Amount = Convert.ToDecimal(txtPaymentAmount.Value.GetValueOrDefault()),
                        Notes = txtPaymentNotes.Text
                    });
                gvwPayment.DataSource = _PaymentDetail;
                gvwPayment.DataBind();
                txtPaymentAmount.Value = 0;
                txtApprovalCode.Text = String.Empty;
                CalculateTotalPayment();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("ExistingMember");
            if (Page.IsValid)
            {
                decimal totalInvoice = 0, totalPayment = 0;
                totalInvoice = Convert.ToDecimal(ViewState["_totalInvoice"]);
                totalPayment = Convert.ToDecimal(ViewState["_totalPayment"]);
                if (totalInvoice != totalPayment || (totalInvoice == 0 && totalPayment == 0))
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblStatus,
                        "Total Invoice is not equal Total Payment. Please verify this transaction again",
                        LabelStyleNames.ErrorMessage);
                    return;
                }

                try
                {
                    string paymentNo = InvoiceService.UpdateInvoiceAndPayment(lblInvoiceNo.Text,
                        calInvoiceDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                        calPaymentDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                        _InvoiceDetail,
                        _PaymentDetail);

                    /*string paymentNo = paymentProvider.Create(                
                    DateTime.Today,
                    lblInvoiceNo.Text,                
                    _PaymentDetail);*/

                    //ClientScript.RegisterStartupScript(this.GetType(), "payment", "alert('Payment " + paymentNo + " has been created for invoice " + lblInvoiceNo.Text + "'); window.opener.location.href='ExistingMemberCompleted.aspx?InvoiceNo="+ lblInvoiceNo.Text + "'; window.close(); ", true);
                    Response.Redirect(String.Format("ExistingMemberCompleted.aspx?InvoiceNo={0}&Prompt=1",
                        lblInvoiceNo.Text));

                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
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
            CalculateTotalPayment();
        }
        protected void gvwPayment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
        protected void gvwInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void btnPopupOK_Click(object sender, EventArgs e)
        {
            InvoiceHeader invoiceHeader = InvoiceService.GetInvoice(lblInvoiceNo.Text);

            _InvoiceDetail.Add(new InvoiceDetailViewModel()
            {
                ID = 0,
                InvoiceID = invoiceHeader.ID,
                ItemID = Convert.ToInt32(ddlItem.SelectedValue),
                ItemBarcode = ItemService.Get(Convert.ToInt32(ddlItem.SelectedValue)).Barcode,
                ItemDescription = ItemService.Get(Convert.ToInt32(ddlItem.SelectedValue)).Description,
                Quantity = Convert.ToInt32(txtQuantity.Text),
                UnitPrice = Convert.ToInt32(txtUnitPrice.Text),
                IsTaxable = true,
                Discount = 0,
                NetAmount = (Convert.ToInt32(txtQuantity.Text) * Convert.ToInt32(txtUnitPrice.Text)) / 1.1M,
                Total = (Convert.ToInt32(txtQuantity.Text) * Convert.ToInt32(txtUnitPrice.Text))
            });
            decimal totalAfterTax = _InvoiceDetail.Any() ? _InvoiceDetail.Sum(inv => inv.Total) - invoiceHeader.DiscountValue : 0M;
            decimal totalBeforeTax = _InvoiceDetail.Any() ? _InvoiceDetail.Sum(i => ((i.UnitPrice * i.Quantity) - (i.Discount / 100 * (i.UnitPrice * i.Quantity))) / (i.IsTaxable ? 1.1M : 1M)) - invoiceHeader.DiscountValue : 0;
            lblTotalBeforeTax.Text = totalBeforeTax.ToString("###,##0.00");
            lblTotalInvoice.Text = totalAfterTax.ToString("###,##0.00");
            lblTotalTax.Text = (totalAfterTax - totalBeforeTax).ToString("###,##0.00");

            ViewState["_totalInvoice"] = totalAfterTax;
            CalculateTotalPayment();

            gvwInvoice.DataSource = _InvoiceDetail;
            gvwInvoice.DataBind();

            txtQuantity.Value = 0;
            txtUnitPrice.Value = 0;
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

        protected void ddlItemType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
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
        }
    }
}