using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;
using Telerik.OpenAccess;

namespace GoGym.Providers
{
    public class ItemProvider : BaseProvider
    {
        public ItemProvider(FitnessEntities context, IPrincipal principal)
            : base(context, principal)
        {
        }

        public IEnumerable<Item> GetMonthlyDuesItem()
        {
            var query = from item in context.Items
                        join itemType in context.ItemTypes on item.ItemTypeID equals itemType.ID
                        where itemType.Description == "Monthly Dues"
                        select item;
            return query.ToList();
        }

        public void Add(
            string barcode,
            string description,
            int itemAccountID,
            int itemTypeID,
            decimal standardUnitPrice,
            bool isActive,
            bool isTaxed,
            string unitName1,
            string unitName2,
            string unitName3,
            int unitFactor2,
            int unitFactor3,
            short? sessionBalance)
        {
            var item = new Item();
            item.Barcode = barcode;
            item.Description = description;
            item.ItemAccountID = itemAccountID;
            item.ItemTypeID = itemTypeID;
            item.UnitPrice = standardUnitPrice;
            item.IsActive = isActive;
            item.UnitName1 = unitName1;
            item.UnitName2 = unitName2;
            item.UnitName3 = unitName3;
            item.UnitFactor2 = unitFactor2;
            item.UnitFactor3 = unitFactor3;
            item.SessionBalance = sessionBalance;
            item.IsTaxed = isTaxed;
            context.Add(item);
            EntityHelper.SetAuditFieldForInsert(item, principal.Identity.Name);
            context.SaveChanges();
        }        

        public void Update(
            int id,
            string barcode,
            string description,
            int itemAccountID,
            int itemTypeID,
            decimal standardUnitPrice,
            bool isActive,
            bool isTaxed,
            string unitName1,
            string unitName2,
            string unitName3,
            int unitFactor2,
            int unitFactor3,
            short? sessionBalance)
        {
            Item item = context.Items.Single(row => row.ID == id);
            item.Barcode = barcode;
            item.Description = description;
            item.ItemAccountID = itemAccountID;
            item.ItemTypeID = itemTypeID;
            item.UnitPrice = standardUnitPrice;
            item.IsActive = isActive;
            item.UnitName1 = unitName1;
            item.UnitName2 = unitName2;
            item.UnitName3 = unitName3;
            item.UnitFactor2 = unitFactor2;
            item.UnitFactor3 = unitFactor3;
            item.IsTaxed = isTaxed;
            item.SessionBalance = sessionBalance;

            EntityHelper.SetAuditFieldForUpdate(item, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.Items.Where(row => id.Contains(row.ID)));
            context.SaveChanges();
        }

        public Item Get(int id)
        {
            return context.Items.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Item> GetAll()
        {
            var query = from item in context.Items
                        select item;
            return query.ToList();
        }

        public IEnumerable<Item> GetAll(int[] branchesID)
        {
            var query = from item in context.Items
                        from itembranch in context.Branches
                        where item.IsActive && branchesID.Contains(itembranch.ID)
                        orderby item.Description
                        select item;
            return query.ToList();
        }

        public IEnumerable<Item> GetItemsByType(int itemTypeID)
        {
            var query = context.Items.Where(item => item.ItemTypeID == itemTypeID && item.IsActive);
            return query.ToList();
        }

        public IEnumerable<Item> GetMaterialItems()
        {
            var query = from item in context.Items
                        join itemType in context.ItemTypes on item.ItemTypeID equals itemType.ID
                        where itemType.Type == 'M'
                        select item;
            return query.ToList();
        } 

        public IEnumerable<int> GetBranchesByItem(int itemID)
        {
            var query = from item in context.Items
                        from itembranch in context.Branches
                        where item.ID == itemID
                        select itembranch.ID;
            return query.ToList();
        }

        public void UpdateItemsAtBranch(int itemID, IEnumerable<int> branchesID)
        {

            var item = context.Items.SingleOrDefault(row => row.ID == itemID);
            if (item != null)
            {
                item.Branches.Clear();

                foreach (int branchID in branchesID)
                {
                    item.Branches.Add(
                        context.Branches.Single(br => br.ID == branchID));                                  
                }
                context.SaveChanges();
            }
        }

        public IEnumerable<string> GetItemUnits(int itemID)
        {
            var units = new List<string>();

            var item = context.Items.SingleOrDefault(row => row.ID == itemID);
            if (item != null)
            {
                units.Add(item.UnitName1);

                if (!String.IsNullOrEmpty(item.UnitName2))
                {
                    units.Add(item.UnitName2.Trim());
                }

                if (!String.IsNullOrEmpty(item.UnitName3))
                {
                    units.Add(item.UnitName3.Trim());
                }
            }

            return units;
        }

        public int GetItemUnitRatio(int itemID, string unitName)
        {
            int ratio = 1;
            var item = context.Items.SingleOrDefault(i => i.ID == itemID);            
            if (item != null)
            {
                if (unitName == item.UnitName1)
                    ratio = 1;
                else if (unitName == item.UnitName2)
                    ratio = item.UnitFactor2.GetValueOrDefault(0);
                else if (unitName == item.UnitName3)
                    ratio = item.UnitFactor3.GetValueOrDefault(0);
                else
                    ratio = 1;
            }            

            return ratio;
        }

        public IEnumerable<Item> GetItems(string findExpression)
        {
            return context.Items.Where(item => item.Barcode.Contains(findExpression) ||
                                               item.Description.Contains(findExpression)).ToList();
        }

        public IEnumerable<Item> GetMaterialItems(string findExpression)
        {
            return context.Items.Include(it => it.ItemType)
                          .Where(item => item.ItemType.Type == 'M' &&
                                         (item.Barcode.Contains(findExpression) ||
                                         item.Description.Contains(findExpression)))
                          .ToList();
        }

        public Item GetItem(int itemID)
        {
            return context.Items.SingleOrDefault(it => it.ID == itemID);
        }
    }
}
