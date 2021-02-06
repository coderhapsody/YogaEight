using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Helpers;

namespace GoGym.FrontEnd.MasterPages
{
    public partial class MasterWorkspace : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (BrowserCompatibility.IsUplevel)
            {
            }
        }

        protected void lgsLoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            FormsAuthentication.SignOut();            
        } 
    }
}