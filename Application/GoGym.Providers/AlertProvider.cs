using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class AlertProvider : BaseProvider
    {
        public AlertProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdateAlert(int id, string subject, string description, DateTime startDate, DateTime? endDate, bool active, int? backColor)
        {
            var alert = id == 0 ? new Alert() : context.Alerts.Single(al => al.ID == id);
            alert.Subject = subject;
            alert.Description = description;
            alert.StartDate = startDate;
            alert.EndDate = endDate;
            if (endDate.HasValue)
                alert.EndDate = new DateTime(alert.EndDate.Value.Year, alert.EndDate.Value.Month, alert.EndDate.Value.Day, 23, 59, 59);
            alert.Active = active;
            alert.BackColor = backColor;

            EntityHelper.SetAuditField(id, alert, principal.Identity.Name);            
            
            if(id == 0)
                context.Add(alert);

            context.SaveChanges();
        }

        public void DeleteAlert(int id)
        {
            var alert = context.Alerts.Single(al => al.ID == id);
            context.Delete(alert);
            context.SaveChanges();
        }

        public IEnumerable<Alert> GetAlerts(bool activeOnly = true)
        {
            IQueryable<Alert> query = context.Alerts;

            if (activeOnly)
            {
                query = query.Where(alert => alert.Active);
            }

            return query.ToList();
        }

        public Alert GetAlert(int alertID)
        {
            return context.Alerts.Single(al => al.ID == alertID);
        }
    }
}
