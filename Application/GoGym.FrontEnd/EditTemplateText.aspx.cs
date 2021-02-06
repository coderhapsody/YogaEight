using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class EditTemplateText : BaseForm
    {
        [Inject]
        public TemplateTextProvider TemplateTextService { get; set; }
        private string pathToTemplate = HttpContext.Current.Server.MapPath("~/TemplateText/");

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                
                //var txtPresigningNotice = RadTabStrip1.Tabs[0].FindControl("txtPresigningNotice") as RadEditor;
                //var txtTermsConditions = RadTabStrip1.Tabs[1].FindControl("txtTermsConditions") as RadEditor;
                //var txtReceiptFooterText = RadTabStrip1.Tabs[2].FindControl("txtReceiptFooterText") as RadEditor;

                txtTermsConditions.Content = TemplateTextService.GetTermsConditionsText(pathToTemplate);
                txtPresigningNotice.Content = TemplateTextService.GetPresigningNotice(pathToTemplate);
                txtReceiptFooterText.Content = TemplateTextService.GetReceiptFooterText(pathToTemplate);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //var txtPresigningNotice = RadTabStrip1.Tabs[0].FindControl("txtPresigningNotice") as RadEditor;
                //var txtTermsConditions = RadTabStrip1.Tabs[1].FindControl("txtTermsConditions") as RadEditor;
                //var txtReceiptFooterText = RadTabStrip1.Tabs[2].FindControl("txtReceiptFooterText") as RadEditor;

                TemplateTextService.UpdatePresigningNotice(pathToTemplate, txtPresigningNotice.Content);
                TemplateTextService.UpdateTermsConditions(pathToTemplate, txtTermsConditions.Content);
                TemplateTextService.UpdateReceiptFooterText(pathToTemplate, txtReceiptFooterText.Content);

                WebFormHelper.SetLabelTextWithCssClass(lblStatus, "Text saved.", LabelStyleNames.InfoMessage);                
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);   
                LogService.ErrorException(GetType().FullName, ex);
            }
        }
    }
}