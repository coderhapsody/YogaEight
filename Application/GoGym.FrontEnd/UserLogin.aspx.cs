using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class UserLogin : BaseForm
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        [Inject]
        public BillingProvider BillingService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.IsPostBack)
            {
                txtUserName.Focus();
            }            
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            bool isValid = SecurityService.ValidateLogin(txtUserName.Text,
                txtPassword.Text,
                Request.UserHostAddress,
                Request.UserHostName, 
                true);            

            if (isValid)
            {
                FormsAuthentication.SetAuthCookie(txtUserName.Text, false);
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
            else
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, "Invalid user name or password", LabelStyleNames.ErrorMessage);
                txtUserName.Focus();
            }
        }
    }
}