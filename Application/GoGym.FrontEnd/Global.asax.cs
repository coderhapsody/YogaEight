using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Ninject;
using Ninject.Extensions.Logging;

namespace GoGym.FrontEnd
{
    public class Global : System.Web.HttpApplication
    {
        [Inject]
        public ILogger Logger { get; set; }

        protected void Application_Start(object sender, EventArgs e)
        {            
            //Logger.Info("Application is starting " + HttpRuntime.AppDomainAppVirtualPath);            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            Logger.FatalException(ToString(), ex);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Logger.Info("Application has ended " + HttpRuntime.AppDomainAppVirtualPath);
        }
    }
}