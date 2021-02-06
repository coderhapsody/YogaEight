using System;
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
    public partial class ExistingMemberCompleted : System.Web.UI.Page
    {
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }
        [Inject]
        public PaymentProvider PaymentService { get; set; }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Prompt"]))
            {
                this.MasterPageFile = "~/MasterPages/MasterPrompt.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["InvoiceNo"]))
            {
                string invoiceNo = Request.QueryString["InvoiceNo"];
                LoadInvoice(invoiceNo);

                PaymentHeader pay = PaymentService.GetPaymentOfInvoice(invoiceNo);
                if (pay != null)
                {
                    LoadPayment(pay.PaymentNo);
                }
            }


            btnPrint.Visible = Request.QueryString["HidePrint"] == null;
            btnClose.Visible = Request.QueryString["HidePrint"] != null;
        }

        private void LoadPayment(string paymentNo)
        {
            PaymentHeader pay = PaymentService.GetPayment(paymentNo);
            IList<PaymentDetailViewModel> paymentDetail = null;
            if (pay != null)
            {
                lblPaymentNo.Text = pay.PaymentNo;
                lblPaymentDate.Text = pay.Date.ToString("dddd, dd MMMM yyyy");
                lblStatusPayment.Text = pay.VoidDate.HasValue ? "Void" : "Active";
                paymentDetail = PaymentService.GetDetail(pay.InvoiceHeader.InvoiceNo);
                lblTotalPayment.Text = (paymentDetail.Any() ? paymentDetail.Sum(payment => payment.Amount) : 0).ToString("###,##0.00");
                gvwPayment.DataSource = paymentDetail;
                gvwPayment.DataBind();
            }
        }

        private void LoadInvoice(string invoiceNo)
        {
            InvoiceHeader invoiceHeader = InvoiceService.GetInvoice(invoiceNo);
            IList<InvoiceDetailViewModel> invoiceDetail = null;
            if (invoiceHeader != null)
            {
                lblBranch.Text = invoiceHeader.Branch.Name;
                lblInvoiceNo.Text = invoiceHeader.InvoiceNo;
                if (invoiceHeader.Customer == null)
                {
                    lblCustomerBarcode.Text = String.Empty;
                    lblCustomerName.Text = invoiceHeader.CustomerName;
                }
                else
                {
                    lblCustomerBarcode.Text = invoiceHeader.Customer.Barcode;
                    lblCustomerName.Text = String.Format("{0} {1}", invoiceHeader.Customer.FirstName, invoiceHeader.Customer.LastName);
                }
                lblNotes.Text = invoiceHeader.Notes;
                lblPurchaseDate.Text = invoiceHeader.Date.ToString("dddd, dd MMMM yyyy");
                lblSales.Text = String.Format("{0} - {1} {2}", invoiceHeader.Employee.Barcode, invoiceHeader.Employee.FirstName, invoiceHeader.Employee.LastName);
                lblStatusInvoice.Text = invoiceHeader.VoidDate.HasValue ? "Void" : "Active";
                lblNotes.Text = invoiceHeader.Notes;
                lblDiscountValue.Text = invoiceHeader.DiscountValue.ToString("###,##0.00");
                invoiceDetail = InvoiceService.GetDetail(invoiceNo);
                gvwInvoice.DataSource = invoiceDetail;
                gvwInvoice.DataBind();

                decimal totalAfterTax = invoiceDetail.Any() ? invoiceDetail.Sum(inv => inv.Total) - invoiceHeader.DiscountValue : 0M;
                decimal totalBeforeTax = invoiceDetail.Any() ? invoiceDetail.Sum(i => ((i.UnitPrice * i.Quantity) - (i.Discount / 100 * (i.UnitPrice * i.Quantity))) / (i.IsTaxable ? 1.1M : 1M)) - invoiceHeader.DiscountValue : 0;
                lblTotalBeforeTax.Text = totalBeforeTax.ToString("###,##0.00");
                lblTotalInvoice.Text = totalAfterTax.ToString("###,##0.00");
                lblTotalTax.Text = (totalAfterTax - totalBeforeTax).ToString("###,##0.00");

                //btnPrint.Attributes["onclick"] = String.Format("showSimplePopUp('PrintPreview.aspx?RDL=SalesReceipt&InvoiceNo={0}');", invoiceHeader.InvoiceNo);
                btnPrint.Attributes["onclick"] = String.Format("showSimplePopUp('PrintPreview.aspx?RDL=SalesReceipt&InvoiceNo={0}');", invoiceHeader.InvoiceNo);
            }
        }

        protected void gvwPayment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void gvwInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
    }
}