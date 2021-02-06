using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class AreaProvider : BaseProvider
    {
        public AreaProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {                        
        }

        public void Add(string description)
        {
            var area = new Area();
            area.Description = description;
            EntityHelper.SetAuditFieldForInsert(area, principal.Identity.Name);
            context.Add(area);
            context.SaveChanges();
        }

        public void Update(int id, string description)
        {
            var area = context.Areas.Single(row => row.ID == id);
            area.Description = description;
            EntityHelper.SetAuditFieldForUpdate(area, principal.Identity.Name);            
            context.SaveChanges();
        }

        public void Delete(int[] areasId)
        {
            context.Delete(context.Areas.Where(row => areasId.Contains(row.ID)));
            context.SaveChanges();
        }

        public Area Get(int id)
        {
            return context.Areas.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Area> GetAll()
        {
            return context.Areas.ToList();
        }

        public string GetAreaName(int id)
        {
            var area = context.Areas.SingleOrDefault(row => row.ID == id);
            string result = area == null ? String.Empty : area.Description;
            return result;
        }
    }
}
