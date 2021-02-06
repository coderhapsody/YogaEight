using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class TrainingSessionDetail : System.Web.UI.Page
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int quotaID = Convert.ToInt32(Request.QueryString["QuotaID"]);
            LoadQuotaInfo(quotaID);
        }

        private void LoadQuotaInfo(int quotaID)
        {
            var quotaHeader = CustomerService.GetQuotaInfo(quotaID);
            var customer = quotaHeader.InvoiceHeader.Customer;
            if (customer != null)
            {
                lblCustomer.Text = String.Format("{0} - {1} {2}",
                    customer.Barcode,
                    customer.FirstName,
                    customer.LastName);
            }
        }
    }
}