﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class ViewBillingDetailHistory : System.Web.UI.Page
    {
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }        

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string invoiceNo = e.Row.Cells[1].Text;
                var invoiceDetails = InvoiceService.GetDetail(invoiceNo);
                if (invoiceDetails != null)
                {
                    var info = new StringBuilder();
                    info.AppendLine("Items for invoice " + invoiceNo + ":");
                    info.AppendLine();
                    foreach (var invoice in invoiceDetails)
                    {
                        info.AppendLine(String.Format("{0} [{1}], Qty: {2}", invoice.ItemBarcode, invoice.ItemDescription,
                            invoice.Quantity));
                    }
                    e.Row.ToolTip = info.ToString();
                }
            }
        }
    }
}