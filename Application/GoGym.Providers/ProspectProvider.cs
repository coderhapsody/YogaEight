using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class ProspectProvider : BaseProvider
    {
        public ProspectProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdateProspect(int id,
                                int branchID,
                                string firstName,
                                string lastName,
                                DateTime date,
                                string identityNo,
                                string phone1,
                                string phone2,
                                string email,
                                DateTime? dateOfBirth,
                                int consultantID,                                
                                string source,
                                string sourceRef,
                                string notes,
                                bool enableFreeTrial,
                                DateTime? freeTrialFrom,
                                DateTime? freeTrialTo)
        {
            var prospect = id == 0 ? new Prospect() : context.Prospects.Single(prosp => prosp.ID == id);

            prospect.BranchID = branchID;
            prospect.FirstName = firstName;
            prospect.LastName = lastName;
            prospect.Date = date;
            prospect.IdentityNo = identityNo;
            prospect.Phone1 = phone1;
            prospect.Phone2 = phone2;
            prospect.Email = email;
            prospect.DateOfBirth = dateOfBirth;
            prospect.ConsultantID = consultantID;
            prospect.ProspectSource = source;
            prospect.ProspectRef = sourceRef;
            prospect.Notes = notes;
            prospect.FreeTrialValidFrom = enableFreeTrial ? freeTrialFrom.GetValueOrDefault(DateTime.Today) : (DateTime?)null;
            prospect.FreeTrialValidTo = enableFreeTrial ? freeTrialTo.GetValueOrDefault(DateTime.Today) : (DateTime?) null;
            EntityHelper.SetAuditField(id, prospect, principal.Identity.Name);
            if (id == 0)
                context.Add(prospect);

            context.SaveChanges();
        }

        public IEnumerable<string> GetProspectSources()
        {
            var sources = new List<string>()
                          {
                              "Flyer",
                              "Friend",
                              "Internet",
                              "TV Ad",
                              "Newspaper Ad",
                              "Walk-in",
                              "Drag-in"
                          };
            return sources.OrderBy(source => source);
        }

        public IEnumerable<string> GetFollowUpVias()
        {
            var sources = new List<string>()
                          {
                              "Phone",
                              "Text",
                              "Email",
                              "Letter",
                              "Gift",
                              "Visit"
                          };
            return sources.OrderBy(source => source);
        }

        public IEnumerable<string> GetFollowUpOutcomes()
        {
            var outcomes = new List<string>()
                           {
                               "Need Followup",
                               "Dead Prospect",                               
                               "Hot Prospect",
                               "Become Customer"
                           };
            return outcomes;
        }

        public void DeleteProspect(int id)
        {
            var followUps = context.ProspectFollowUps.Where(prosp => prosp.ProspectID == id).ToList();
            context.Delete(followUps);
            
            var prospect = context.Prospects.Single(prosp => prosp.ID == id);
            context.Delete(prospect);

            context.SaveChanges();
        }

        public Prospect GetProspect(int id)
        {
            return context.Prospects.SingleOrDefault(prosp => prosp.ID == id);
        }

        public ProspectFollowUp GetProspectFollowUp(int id)
        {
            return context.ProspectFollowUps.SingleOrDefault(prosp => prosp.ID == id);
        }

        public void AddOrUpdateFollowUp(int id, int prospectID, DateTime date, string followUpVia, string result, string outcome)
        {
            var followUp = id == 0
                ? new ProspectFollowUp()
                : context.ProspectFollowUps.Single(prosp => prosp.ID == id);

            followUp.ProspectID = prospectID;
            followUp.Date = date;
            followUp.FollowUpVia = followUpVia;
            followUp.Result = result;
            followUp.Outcome = outcome;

            EntityHelper.SetAuditField(id, followUp, principal.Identity.Name);

            if (id == 0)
                context.Add(followUp);

            context.SaveChanges();
        }

        public void DeleteFollowUp(int id)
        {
            var prospectFollowUp = context.ProspectFollowUps.Single(prosp => prosp.ID == id);            
            context.Delete(prospectFollowUp);
            context.SaveChanges();
        }
    }
}
