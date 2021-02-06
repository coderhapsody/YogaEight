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
    public class WarehouseProvider : BaseProvider
    {
        public WarehouseProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdate(int id, int branchID, string code, string name, bool isActive)
        {
            var warehouse = id == 0 ? new Warehouse() : context.Warehouses.Single(wh => wh.ID == id);
            warehouse.BranchID = id == 0 ? branchID : warehouse.BranchID;
            warehouse.Code = code;
            warehouse.Name = name;
            warehouse.IsActive = isActive;

            if (id == 0)
                context.Add(warehouse);

            EntityHelper.SetAuditField(id, warehouse, principal.Identity.Name);

            context.SaveChanges();
        }

        public Warehouse GetWarehouse(int id)
        {
            return context.Warehouses.SingleOrDefault(wh => wh.ID == id);
        }

        public void DeleteWarehouse(int id)
        {
            var warehouse = GetWarehouse(id);
            context.Delete(warehouse);
            context.SaveChanges();
        }

        public IEnumerable<Warehouse> GetWarehouses(int branchID, bool activeOnly = true)
        {
            var query = context.Warehouses.Where(wh => wh.BranchID == branchID);

            if (activeOnly)
                query = query.Where(wh => wh.IsActive);

            return query.ToList();

        }
    }
}
