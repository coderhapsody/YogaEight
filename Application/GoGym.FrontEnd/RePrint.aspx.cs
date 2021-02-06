using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class RePrint : System.Web.UI.Page
    {
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }
        [Inject]
        public EmployeeProvider EmployeeService { get; set; }
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hypLookUpInvoice.Attributes.Add("onclick", String.Format("showPromptPopUp('PromptInvoice.aspx?', '{0}', 550, 1000);", txtInvoiceNo.ClientID));
            }
        }
        
        protected void btnPopupOK_Click(object sender, EventArgs e)
        {
            if (SecurityService.ValidateLogin(txtUserName.Text, txtPassword.Text) &&
                EmployeeService.CanReprintInvoice(txtUserName.Text))
            {
                InvoiceHeader invoice = InvoiceService.GetInvoice(txtInvoiceNo.Text);
                
                //TODO
                //mopAuth.Hide();
                switch (invoice.InvoiceType)
                {
                    case 'F':
                        Response.Redirect(String.Format("FreshMemberCompleted.aspx?InvoiceNo={0}", invoice.InvoiceNo));
                        break;

                    case 'X':
                        Response.Redirect(String.Format("ExistingMemberCompleted.aspx?InvoiceNo={0}", invoice.InvoiceNo));
                        break;
                }
            }
            else
            {
                //TODO
                //mopAuth.Hide();
                WebFormHelper.SetLabelTextWithCssClass(lblStatus0,
                    "Invalid user name/password, or user does not have permission to reprint invoice.",
                    LabelStyleNames.ErrorMessage);

            }
        }
    }
}