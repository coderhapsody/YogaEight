using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;

namespace GoGym.FrontEnd
{
    public partial class InquiryMembership : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            RadFilter1.FireApplyCommand();
            RadGrid1.MasterTableView.ExportToExcel();
        }
        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            RadFilter1.FireApplyCommand();
        }
    }
}