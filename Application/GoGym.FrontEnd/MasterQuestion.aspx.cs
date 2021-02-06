using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class MasterQuestion : BaseForm
    {
        [Inject]
        public ContractQuestionProvider ContractQuestionService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetGridViewPageSize(gvwMaster);
            }
        }


        protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
            WebFormHelper.ChangeBackgroundColorRowOnHover(e);
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            mvwForm.SetActiveView(viwAddEdit);
            RowID = 0;
            WebFormHelper.ClearTextBox(txtDescription);
            txtDescription.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
                Array.ForEach(id, q => ContractQuestionService.DeleteQuestion(q));                
                Refresh();
            }
            catch (Exception ex)
            {
                mvwForm.ActiveViewIndex = 0;
                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReloadCurrentPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    ContractQuestionService.AddOrUpdateQuestion(RowID,
                        txtDescription.Text,
                        Convert.ToInt32(txtSeq.Value.GetValueOrDefault(1)),
                        chkIsActive.Checked);                    
                    ReloadCurrentPage();
                }
                catch (Exception ex)
                {
                    mvwForm.ActiveViewIndex = 0;
                    WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }

        private void Refresh()
        {
            gvwMaster.DataBind();            
        }

        protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                var question = ContractQuestionService.GetQuestion(id);
                txtDescription.Text = question.Question;
                txtSeq.Value = question.Seq;
                chkIsActive.Checked = question.IsActive;
                txtDescription.Focus();
            }
        }
    }
}