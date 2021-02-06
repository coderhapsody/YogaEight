using System;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class InputCustomerNotes : BaseForm
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }
        [Inject]
        public CustomerNotesProvider CustomerNotesService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            tblNotes.Visible = String.IsNullOrEmpty(Request["mode"]);
            if (!Page.IsPostBack)
            {
                string custBarcode = Request["barcode"];
                if (!String.IsNullOrEmpty(custBarcode))
                {
                    Customer customer = CustomerService.Get(custBarcode);
                    if (customer != null)
                    {
                        litCustomerName.Text = String.Format("{0} {1} ({2})", customer.FirstName, customer.LastName, customer.Barcode);
                    }
                }

                if (!String.IsNullOrEmpty(Request["mode"]))
                    btnSave.Enabled = txtNotes.Enabled = false;
                else
                    btnSave.Enabled = txtNotes.Enabled = true;


                lblDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            }
            //litCustomerName.Text = 
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerNotesService.Add(
                    Request["barcode"],
                    txtNotes.Text,
                    (short)(chkShowCheckIn.Checked ? 1 : 0));
                txtNotes.Text = String.Empty;
                chkShowCheckIn.Checked = false;
                gvwNotes.DataBind();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
        protected void gvwNotes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteNote")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                CustomerNotesService.Delete(id);
                gvwNotes.DataBind();
            }
            else if (e.CommandName == "Toggle")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                CustomerNotesService.ToggleNote(id);
                gvwNotes.DataBind();

            }
        }
        protected void gvwNotes_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);

            var lnbDelete = e.Row.FindControl("lnbDelete") as LinkButton;
            if (lnbDelete != null)
                lnbDelete.Visible = String.IsNullOrEmpty(Request["mode"]);
        }
    }
}