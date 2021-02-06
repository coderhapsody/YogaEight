using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Utilities.Extensions;

namespace GoGym.Providers
{
    public class CustomerGradeProvider : BaseProvider
    {
        public CustomerGradeProvider(FitnessEntities context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public void AddGrade(string description)
        {
            var grade = new CustomerGrade();
            grade.Description = description;
            SetAuditFields(grade);
            context.Add(grade);
            context.SaveChanges();
        }

        public void UpdateGrade(int id, string description)
        {
            var grade = context.CustomerGrades.SingleOrDefault(gr => gr.ID == id);
            if(grade != null)
            {
                grade.Description = description;
                SetAuditFields(grade);
                context.SaveChanges();
            }
        }

        public CustomerGrade GetGrade(int id)
        {
            return context.CustomerGrades.SingleOrDefault(grade => grade.ID == id);
        }

        public void DeleteGrade(int id)
        {
            var grade = GetGrade(id);
            if(grade != null)
            {
                context.Delete(grade);
                context.SaveChanges();
            }
        }

        public IList<CustomerGrade> GetGrades(string sortExpression)
        {
            return context.CustomerGrades.OrderBy(sortExpression).ToList();
        }

        public void AddGradeHistory(int customerID, int customerGradeID, DateTime effectiveDate, string notes)
        {
            var gradeHistory = new CustomerGradeHistory();
            gradeHistory.CustomerID = customerID;
            gradeHistory.Date = DateTime.Today;
            gradeHistory.EffectiveDate = effectiveDate;
            gradeHistory.Notes = notes;
            gradeHistory.CustomerGradeID = customerGradeID;
            SetAuditFields(gradeHistory);
            context.Add(gradeHistory);
            context.SaveChanges();
        }

        public IEnumerable<CustomerGradeHistory> GetCustomerGradeHistories(int customerID)
        {
            var query = from gradeHist in context.CustomerGradeHistories
                        where gradeHist.CustomerID == customerID
                        orderby gradeHist.ID descending
                        select gradeHist;
            return query.ToList();

        }

        public CustomerGradeHistory GetGradeHistory(int id)
        {
            return context.CustomerGradeHistories.SingleOrDefault(gradeHist => gradeHist.ID == id);
        }

        public void UpdateGradeHistory(int id, int customerID, int customerGradeID, DateTime effectiveDate, string notes)
        {
            var gradeHistory = GetGradeHistory(id);
            gradeHistory.CustomerID = customerID;
            gradeHistory.Date = DateTime.Today;
            gradeHistory.EffectiveDate = effectiveDate;
            gradeHistory.Notes = notes;
            gradeHistory.CustomerGradeID = customerGradeID;
            SetAuditFields(gradeHistory);            
            context.SaveChanges();
        }


        public void DeleteGradeHistory(int id)
        {
            var gradeHistory = context.CustomerGrades.SingleOrDefault(gradeHist => gradeHist.ID == id);
            if(gradeHistory != null)
            {
                context.Delete(gradeHistory);
                context.SaveChanges();
            }
        }

        public CustomerGradeHistory GetLatestGrade(int customerID)
        {
            var query = from custGradeHist in context.CustomerGradeHistories
                        where custGradeHist.CustomerID == customerID
                              && custGradeHist.EffectiveDate <= DateTime.Today
                        orderby custGradeHist.ID descending
                        select custGradeHist;
            return query.FirstOrDefault();
        }
    }
}
