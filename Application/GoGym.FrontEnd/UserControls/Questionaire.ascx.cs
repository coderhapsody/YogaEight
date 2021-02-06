using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.UserControls
{
    public partial class Questionaire : System.Web.UI.UserControl
    {
        [Inject]
        public ContractQuestionProvider ContractQuestionService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        public void LoadQuestions(int contractID)
        {
            gvwQuestionaire.DataSource = ContractQuestionService.GetActiveQuestions();
            gvwQuestionaire.DataBind();

            if (contractID > 0)
            {
                foreach (GridViewRow gridRow in gvwQuestionaire.Rows)
                {
                    int questionID = Convert.ToInt32(gridRow.Cells[0].Text);
                    var checkBoxRow = (CheckBox) gridRow.FindControl("chkAnswer");
                    checkBoxRow.Checked = ContractQuestionService.GetAnswer(contractID, questionID) > 0;
                }
            }
        }

        public IDictionary<int, short> GetAnswers()
        {
            var answers = new Dictionary<int, short>();
            foreach (GridViewRow gridRow in gvwQuestionaire.Rows)
            {
                int questionID = Convert.ToInt32(gridRow.Cells[0].Text);
                var checkBoxRow = (CheckBox)gridRow.FindControl("chkAnswer");
                answers.Add(questionID, (short)(checkBoxRow.Checked ? 1 : 0));
            }
            return answers;
        }

        protected void gvwQuestionaire_RowCreated(object sender, GridViewRowEventArgs e)
        {
            WebFormHelper.HideGridViewRowId(e);
        }
    }
}