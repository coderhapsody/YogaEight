using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class ItemTypeProvider : BaseProvider
    {
        public ItemTypeProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void Add(string description, string type)
        {
            var obj = new ItemType();
            obj.Description = description;
            obj.Type = Convert.ToChar(type);
            EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
            context.Add(obj);
            context.SaveChanges();
        }


        public void Update(int id, string description, string type)
        {
            var obj = context.ItemTypes.Single(row => row.ID == id);
            obj.Description = description;
            obj.Type = Convert.ToChar(type);
            EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.ItemTypes.Where(row => id.Contains(row.ID)));
        }

        public ItemType Get(int id)
        {
            return context.ItemTypes.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<ItemType> GetAll()
        {
            return context.ItemTypes.ToList();
        }
    }
}
