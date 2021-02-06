using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class ContractQuestionProvider  :BaseProvider
    {
        public ContractQuestionProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdateQuestion(int id, string question, int seq, bool isActive)
        {
            var quest = id == 0 ? new ContractQuestion() : context.ContractQuestions.Single(q => q.ID == id);
            quest.Question = question;
            quest.Seq = seq;
            quest.IsActive = isActive;
            EntityHelper.SetAuditField(id, quest, principal.Identity.Name);

            if(id == 0)
                context.Add(quest);            

            context.SaveChanges();
        }

        public void DeleteQuestion(int id)
        {
            var quest = context.ContractQuestions.Single(q => q.ID == id);
            context.Delete(quest);
            context.SaveChanges();
        }

        public IEnumerable<ContractQuestion> GetActiveQuestions()
        {
            return context.ContractQuestions.Where(question => question.IsActive).ToList();
        }

        public ContractQuestion GetQuestion(int id)
        {
            return context.ContractQuestions.SingleOrDefault(q => q.ID == id);
        }

        public short GetAnswer(int contractID, int questionID)
        {
            var answer = context.ContractQuestionAnswers.SingleOrDefault(q => q.ContractID == contractID && q.QuestionID == questionID);
            return answer != null ? answer.Answer : (short)0;
        }

    }
}
