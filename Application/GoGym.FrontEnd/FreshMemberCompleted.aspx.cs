﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class FreshMemberCompleted : System.Web.UI.Page
    {
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }
        [Inject]
        public PaymentProvider PaymentService { get; set; }
        [Inject]
        public ContractProvider ContractService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["InvoiceNo"]))
            {
                string invoiceNo = Request.QueryString["InvoiceNo"];
                PaymentHeader pay = PaymentService.GetPaymentOfInvoice(invoiceNo);
                if (pay != null)
                {
                    LoadInvoice(invoiceNo);
                    LoadPayment(pay.PaymentNo);
                }
            }

            btnPrint.Visible = Request.QueryString["HidePrint"] == null;
            btnClose.Visible = Request.QueryString["HidePrint"] != null;
        }

        private void LoadPayment(string paymentNo)
        {
            IEnumerable<PaymentDetailViewModel> paymentDetail = null;
            PaymentHeader pay = PaymentService.GetPayment(paymentNo);
            if (pay != null)
            {
                lblPaymentNo.Text = pay.PaymentNo;
                lblPaymentDate.Text = pay.Date.ToString("dddd, dd MMMM yyyy");
                lblStatusPayment.Text = pay.VoidDate.HasValue ? "Void" : "Active";
                paymentDetail = PaymentService.GetDetail(pay.InvoiceHeader.InvoiceNo);
                gvwPayment.DataSource = paymentDetail;
                gvwPayment.DataBind();

                lblTotalPayment.Text = (paymentDetail.Any() ? paymentDetail.Sum(inv => inv.Amount) : 0M).ToString("###,##0.00");
            }
        }

        private void LoadInvoice(string invoiceNo)
        {
            InvoiceHeader invoiceHeader = InvoiceService.GetInvoice(invoiceNo);
            Contract contract = ContractService.GetContractByInvoiceNo(invoiceNo);
            IEnumerable<InvoiceDetailViewModel> invoiceDetail = null;
            if (invoiceHeader != null && contract != null)
            {
                lblBranch.Text = invoiceHeader.Branch.Name;
                lblInvoiceNo.Text = invoiceHeader.InvoiceNo;
                lblContractNo.Text = contract.ContractNo;
                lblCustomerBarcode.Text = invoiceHeader.Customer.Barcode;
                lblCustomerName.Text = String.Format("{0} {1}", invoiceHeader.Customer.FirstName, invoiceHeader.Customer.LastName);
                lblNotes.Text = invoiceHeader.Notes;
                lblPurchaseDate.Text = invoiceHeader.Date.ToString("dddd, dd MMMM yyyy");
                lblEffectiveDate.Text = invoiceHeader.Date.ToString("dddd, dd MMMM yyyy");
                lblBillingType.Text = contract.BillingType.Description;
                lblSales.Text = String.Format("{0} - {1} {2}", invoiceHeader.Employee.Barcode, invoiceHeader.Employee.FirstName, invoiceHeader.Employee.LastName);
                lblPackage.Text = contract.PackageHeader.Name;
                lblStatusInvoice.Text = invoiceHeader.VoidDate.HasValue ? "Void" : "Active";
                lblNotes.Text = invoiceHeader.Notes;
                lblDiscountValue.Text = invoiceHeader.DiscountValue.ToString("###,##0.00");
                invoiceDetail = InvoiceService.GetDetail(invoiceNo);
                gvwPackage.DataSource = invoiceDetail;
                gvwPackage.DataBind();

                decimal totalAfterTax = invoiceDetail.Any() ? invoiceDetail.Sum(i => (i.UnitPrice * i.Quantity) - (i.Discount / 100 * (i.UnitPrice * i.Quantity))) - invoiceHeader.DiscountValue : 0;
                decimal totalBeforeTax = invoiceDetail.Any() ? invoiceDetail.Sum(i => ((i.UnitPrice * i.Quantity) - (i.Discount / 100 * (i.UnitPrice * i.Quantity))) / (i.IsTaxable ? 1.1M : 1M)) - invoiceHeader.DiscountValue : 0;

                lblTotalBeforeTax.Text = totalBeforeTax.ToString("###,##0.00");
                lblTotalInvoice.Text = totalAfterTax.ToString("###,##0.00");
                lblTotalTax.Text = (totalAfterTax - totalBeforeTax).ToString("###,##0.00");

                //btnPrint.Attributes["onclick"] = String.Format("showSimplePopUp('PrintPreview.aspx?RDL=SalesReceipt&InvoiceNo={0}');", invoiceHeader.InvoiceNo);
                //btnPrint.Attributes["onclick"] = String.Format("showSimplePopUp('PrintPreview.aspx?RDL=SalesReceipt&InvoiceNo={0}');", invoiceHeader.InvoiceNo);                
            }
        }

        protected void gvwPackage_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void gvwPayment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
    }
}