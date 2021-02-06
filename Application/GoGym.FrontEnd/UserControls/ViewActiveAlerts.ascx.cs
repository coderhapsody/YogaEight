using System;
using System.Web.UI.WebControls;

namespace GoGym.FrontEnd.UserControls
{
    public partial class ViewActiveAlerts : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (repAlerts.Items.Count == 0)
            //{
            //    WebFormHelper.SetLabelTextWithCssClass(
            //        lblMessage,
            //        "<h3>No alerts for today</h3>",
            //        LabelStyleNames.AlternateMessage);
            //}
        }
        protected void sdsAlert_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@Date"].Value = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}