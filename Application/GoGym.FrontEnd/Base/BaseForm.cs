using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using GoGym.Providers;
using Ninject;
using Ninject.Extensions.Logging;

namespace GoGym.FrontEnd.Base
{
    public abstract class BaseForm : System.Web.UI.Page
    {
        [Inject]
        public ILogger LogService { get; set; }

        [Inject]
        public EmployeeProvider EmployeeService { get; set; }

        public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }

        public string CurrentPageName
        {
            get
            {
                return Path.GetFileName(HttpContext.Current.Request.PhysicalPath);
            }
        }

        public int HomeBranchID
        {
            get
            {
                return EmployeeService.GetHomeBranchID(User.Identity.Name);
            }
        }

        protected virtual void ReloadCurrentPage()
        {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path), true);
        }


    }
}