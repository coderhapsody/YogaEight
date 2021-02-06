using System;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ChangeCreditCard : BaseForm
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }
        [Inject]
        public CreditCardTypeProvider CreditCardTypeService { get; set; }
        [Inject]
        public BankProvider BankService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlCreditCardType.DataSource = CreditCardTypeService.GetAll();
                ddlCreditCardType.DataTextField = "Description";
                ddlCreditCardType.DataValueField = "ID";
                ddlCreditCardType.DataBind();

                ddlBank.DataSource = BankService.GetActiveBanks();
                ddlBank.DataTextField = "Name";
                ddlBank.DataValueField = "ID";
                ddlBank.DataBind();
                ddlBank.Items.Insert(0, new DropDownListItem(String.Empty));

                var cust = CustomerService.Get(Request["barcode"]);
                lblCustomer.Text = String.Format("{0} - {1} {2}",
                    cust.Barcode,
                    cust.FirstName.Trim(),
                    cust.LastName.Trim());

                calExpireDate.SelectedDate = DateTime.Today;
            }
        }

        protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@CustomerBarcode"].Value = Request["barcode"];
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCreditCardNo.Text.Trim().Length == 16 &&
                    ValidationHelper.IsValidCreditCardNumber(txtCreditCardNo.Text.Trim()))
                {
                    CustomerService.UpdateCreditCardInfo(
                        Request["barcode"],
                        Convert.ToInt32(ddlCreditCardType.SelectedValue),
                        Convert.ToInt32(ddlBank.SelectedValue),
                        txtCardHolderName.Text,
                        txtCardHolderIDNo.Text,
                        txtCreditCardNo.Text,
                        calExpireDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                        txtReason.Text);

                    ClientScript.RegisterStartupScript(GetType(),
                        "_alert",
                        String.Format("alert('Credit card information for {0} has been updated.')", Request["barcode"]),
                        true);

                    DynamicControlBinding.ClearTextBox(txtCardHolderIDNo, txtCardHolderName, txtCreditCardNo);
                    txtReason.Text = String.Empty;
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(),
                        "_alert",
                        String.Format("alert('Credit card number is invalid')"),
                        true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(),
                    "_alert",
                    String.Format("alert('{0}')", ex.Message),
                    true);
                LogService.ErrorException(GetType().FullName, ex);
            }
            finally
            {
                gvwMaster.DataBind();
            }
        }

        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DynamicControlBinding.HideGridViewRowId(0, e);
        }
    }
}