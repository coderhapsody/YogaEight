using System;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class ChangePassword : BaseForm
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.ActiveViewIndex = 0;
            }
        }

        protected void cuvCurrentPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = SecurityService.ValidateLogin(User.Identity.Name, txtCurrentPassword.Text);
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {                
                string newPassword = txtNewPassword.Text;
                string oldPassword = txtCurrentPassword.Text;                            

                if (SecurityService.ValidateLogin(User.Identity.Name, oldPassword))
                {
                    SecurityService.ChangePassword(User.Identity.Name, newPassword);
                }

                mvwForm.ActiveViewIndex = 1;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(),
                    "changePasswordError",
                    String.Format("alert('{0}'", ex.Message),
                    true);

                LogService.ErrorException(GetType().FullName, ex);
            }
        }
    }
}