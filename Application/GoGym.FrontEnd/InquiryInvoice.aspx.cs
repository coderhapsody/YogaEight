using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class InquiryInvoice : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }
        [Inject]
        public InvoiceProvider InvoiceService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlBranch.DataSource = BranchService.GetAllowedBranches(User.Identity.Name);
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();
                ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);

                RadHelper.SetUpDatePickers(calDateFrom, calDateTo, calPaymentDateFrom, calPaymentDateTo);
                calDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calPaymentDateFrom.SelectedDate = DateTime.Today.AddYears(-1);
                calPaymentDateTo.SelectedDate = DateTime.Today;
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
            e.Command.Parameters["@PaymentDateFrom"].Value = calPaymentDateFrom.SelectedDate;
            e.Command.Parameters["@PaymentDateTo"].Value = calPaymentDateTo.SelectedDate;
        }
        protected void gvwData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string invoiceNo = Convert.ToString((e.Row.DataItem as System.Data.DataRowView)["InvoiceNo"]);
                string invoiceType = Convert.ToString((e.Row.DataItem as System.Data.DataRowView)["InvoiceType"]);
                bool isActive = Convert.ToBoolean((e.Row.DataItem as System.Data.DataRowView)["IsActive"]);

                string hyperlinkOnClick = invoiceType == "Fresh Member" ?
                                            String.Format("window.open('FreshMemberCompleted.aspx?InvoiceNo={0}&HidePrint=1', 'invoice', 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=1100,height=600,scrollbars=yes,toolbar=no,status=no')", invoiceNo) :
                                            String.Format("window.open('ExistingMemberCompleted.aspx?InvoiceNo={0}&HidePrint=1', 'invoice', 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=1100,height=600,scrollbars=yes,toolbar=no,status=no')", invoiceNo);
                (e.Row.FindControl("hypViewDetail") as HyperLink).Attributes.Add("onclick", hyperlinkOnClick);

                e.Row.Enabled = isActive;
                if (!e.Row.Enabled)
                    e.Row.BackColor = Color.Red;
            }


        }
        protected void gvwData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }
    }
}