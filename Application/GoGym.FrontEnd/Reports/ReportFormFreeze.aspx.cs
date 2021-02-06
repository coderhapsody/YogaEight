using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoGym.FrontEnd.Reports
{
    public partial class ReportFormFreeze : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hypLookUpCustomer.Attributes["onclick"] = String.Format("showPromptPopUp('../PromptCustomer.aspx?', '{0}', 550, 900);", txtBarcode.ClientID);
            }
        }
    }
}