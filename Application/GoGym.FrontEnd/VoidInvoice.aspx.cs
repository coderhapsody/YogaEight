using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class VoidInvoice : System.Web.UI.Page
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }
        [Inject]
        public CustomerStatusProvider CustomerStatusService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Cache.Insert(System.Web.Caching.Cache.
            if (!this.IsPostBack)
            {
                ddlBranch.DataSource = BranchService.GetActiveBranches(User.Identity.Name);
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();

                calDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calDateTo.SelectedDate = DateTime.Today;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvwData.DataBind();
        }
        protected void sdsData_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@PurchaseDateFrom"].Value = calDateFrom.SelectedDate;
            e.Command.Parameters["@PurchaseDateTo"].Value = calDateTo.SelectedDate;
            e.Command.Parameters["@CustomerBarcode"].Value = txtCustomerBarcode.Text;
            e.Command.Parameters["@CustomerName"].Value = txtCustomerName.Text;
        }
        protected void gvwData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string invoiceNo = Convert.ToString((e.Row.DataItem as System.Data.DataRowView)["InvoiceNo"]);
            //    string invoiceType = Convert.ToString((e.Row.DataItem as System.Data.DataRowView)["InvoiceType"]);
            //    if (invoiceType == "Membership Invoicing")
            //    {
            //        InvoiceHeader invoice = InvoiceService.GetInvoice(invoiceNo);
            //        if (invoice != null)
            //        {
            //            if (CustomerStatusService.GetStatusHistory(invoice.Customer.Barcode).ToList().Count > 0)
            //            {
            //                e.Row.Enabled = false;
            //                e.Row.ToolTip = "Cannot void this invoice because status for this customer has been changed";
            //                //e.Row.BackColor = Color.Red   ;
            //            }
            //            //if(customerStatus invoice.Customer.Barcode
            //        }
            //    }

            //    //    string hyperlinkOnClick = invoiceType == "Fresh Member" ?
            //    //                                String.Format("window.open('FreshMemberCompleted.aspx?InvoiceNo={0}&HidePrint=1', 'invoice', 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=1000,height=600,scrollbars=yes,toolbar=no,status=no')", invoiceNo) :
            //    //                                String.Format("window.open('ExistingMemberCompleted.aspx?InvoiceNo={0}&HidePrint=1', 'invoice', 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=1000,height=600,scrollbars=yes,toolbar=no,status=no')", invoiceNo);
            //    //    (e.Row.FindControl("hypViewDetail") as HyperLink).Attributes.Add("onclick", hyperlinkOnClick);

            //}


        }

        protected void btnProcessVoid_Click(object sender, EventArgs e)
        {
            var query = from row in gvwData.Rows.Cast<GridViewRow>()
                        where (row.FindControl("chkVoid") as CheckBox).Checked
                        select new
                        {
                            InvoiceNo = row.Cells[1].Text
                        };

            foreach (var invoice in query)
            {
                InvoiceHeader currentInvoice = InvoiceService.GetInvoice(invoice.InvoiceNo);
                if (currentInvoice != null)
                {
                    InvoiceService.ProcessVoid(invoice.InvoiceNo, txtNotes.Text, chkVoidPaymentOnly.Checked);
                    lblStatus.Text += String.Format("Invoice <b>{0}</b> has been marked as void <br/>", currentInvoice.InvoiceNo);
                }
            }

            txtNotes.Text = String.Empty;
            gvwData.DataBind();
        }
        protected void gvwData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
    }
}