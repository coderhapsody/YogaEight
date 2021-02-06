using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class DocumentTypeProvider : BaseProvider
    {
        public DocumentTypeProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void Add(string description, bool isLastState, int changeStatusIDTo)
        {
            var docType = new DocumentType();
            docType.Description = description;
            docType.IsLastState = isLastState;
            docType.ChangeCustomerStatusIDTo = changeStatusIDTo == 0 ? (int?)null : changeStatusIDTo;
            context.Add(docType);
            context.SaveChanges();
        }

        public void Update(int id, string description, bool isLastState, int changeStatusIDTo)
        {
            DocumentType docType = context.DocumentTypes.SingleOrDefault(dt => dt.ID == id);
            if (docType != null)
            {
                docType.Description = description;
                docType.IsLastState = isLastState;
                docType.ChangeCustomerStatusIDTo = changeStatusIDTo == 0 ? (int?)null : changeStatusIDTo;
                context.SaveChanges();
            }
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.DocumentTypes.Where(dt => id.Contains(dt.ID)));
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            context.Delete(
                context.DocumentTypes.Single(dt => dt.ID == id));
            context.SaveChanges();
        }

        public DocumentType Get(int id)
        {
            return context.DocumentTypes.SingleOrDefault(docType => docType.ID == id);
        }
    }
}
