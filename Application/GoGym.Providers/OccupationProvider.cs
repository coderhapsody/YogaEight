using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class OccupationProvider : BaseProvider
    {
        public OccupationProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void Add(string description)
        {
            var obj = new Occupation();
            obj.Description = description;
            EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
            context.Add(obj);
            context.SaveChanges();
        }


        public void Update(int id, string description)
        {
            var obj = context.ItemTypes.Single(row => row.ID == id);
            obj.Description = description;
            EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.Occupations.Where(row => id.Contains(row.ID)));
        }

        public Occupation Get(int id)
        {
            return context.Occupations.SingleOrDefault(row => row.ID == id);
        }

        public IList<Occupation> GetAll()
        {
            return context.Occupations.ToList();
        }
    }
}
