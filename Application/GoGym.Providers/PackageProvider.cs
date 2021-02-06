using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class PackageProvider : BaseProvider
    {
        public PackageProvider(FitnessEntities context, IPrincipal principal)
            : base(context, principal)
        {
        }

        public void Add(
                string name,
                int packageDuesInMonth,
                bool isActive,
                bool isOpenEnd,
                decimal freezeFee,
                IEnumerable<PackageDetailViewModel> detail)
        {
            var head = new PackageHeader();
            head.Name = name;
            head.PackageDuesInMonth = packageDuesInMonth;
            head.IsActive = isActive;
            head.OpenEnd = isOpenEnd;
            head.FreezeFee = freezeFee;
            EntityHelper.SetAuditFieldForInsert(head, principal.Identity.Name);
            context.Add(head);
            foreach (PackageDetailViewModel detailLine in detail)
            {
                var obj = new PackageDetail();
                obj.ItemID = detailLine.ItemID;
                obj.PackageID = detailLine.PackageID;
                obj.Quantity = detailLine.Quantity;
                obj.UnitPrice = detailLine.UnitPrice;
                obj.UnitName = detailLine.UnitName;
                obj.PackageHeader = head;
                context.Add(obj);
            }
            context.SaveChanges();
        }

        public void Update(
            int id,
            string name,
            int packageDuesInMonth,
            bool isActive,
            bool isOpenEnd,
            decimal freezeFee,
            IEnumerable<PackageDetailViewModel> detail)
        {
            context.Delete(
                context.PackageDetails.Where(row => row.PackageID == id));

            PackageHeader head = context.PackageHeaders.Single(row => row.ID == id);
            head.Name = name;
            head.PackageDuesInMonth = packageDuesInMonth;
            head.IsActive = isActive;
            head.OpenEnd = isOpenEnd;
            head.FreezeFee = freezeFee;
            EntityHelper.SetAuditFieldForUpdate(head, principal.Identity.Name);
            foreach (PackageDetailViewModel detailLine in detail)
            {
                var obj = new PackageDetail();
                obj.ItemID = detailLine.ItemID;
                obj.PackageID = detailLine.PackageID;
                obj.Quantity = detailLine.Quantity;
                obj.UnitPrice = detailLine.UnitPrice;
                obj.UnitName = detailLine.UnitName;
                obj.PackageHeader = head;
                context.Add(obj);
            }
            context.SaveChanges();
        }

        public void Delete(int[] id)
        {
            foreach (int item in id)
            {
                context.Delete(
                    context.PackageDetails.Where(row => row.PackageID == item));

                var package = context.PackageHeaders.SingleOrDefault(pkg => pkg.ID == item);
                if (package != null) 
                    package.Branches.Clear();
            }
            context.Delete(
                context.PackageHeaders.Where(row => id.Contains(row.ID)));



            context.SaveChanges();
        }

        public PackageHeader Get(int id)
        {
            return context.PackageHeaders.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<PackageDetailViewModel> GetDetail(int id)
        {
            var query = from head in context.PackageHeaders
                        join detail in context.PackageDetails on head.ID equals detail.PackageID
                        join item in context.Items on detail.ItemID equals item.ID
                        where head.ID == id && detail.PackageID == id
                        select new PackageDetailViewModel
                               {
                                   ID = detail.ID,
                                   ItemID = item.ID,
                                   ItemBarcode = item.Barcode,
                                   ItemDescription = item.Description,
                                   PackageID = head.ID,
                                   Quantity = detail.Quantity,
                                   UnitPrice = detail.UnitPrice,
                                   UnitName = detail.UnitName,
                                   Discount = 0,
                                   IsTaxed = item.IsTaxed,
                                   NetAmount = detail.Quantity * detail.UnitPrice / 1.1M,
                                   Total = detail.Quantity * detail.UnitPrice
                               };
            return query.ToList();
        }

        public IEnumerable<int> GetBranchesByPackage(int packageID)
        {
            var query = from package in context.PackageHeaders
                        from packagebranch in context.Branches //on package.ID equals packagebranch.PackageID
                        where package.ID == packageID
                        select packagebranch.ID;
            return query.ToList();
        }

        public IEnumerable<PackageHeader> GetPackagesInBranch(int branchID, bool activeOnly = true)
        {
            var query = from package in context.PackageHeaders
                        from packagebranch in context.Branches // on package.ID equals packagebranch.PackageID
                        where packagebranch.ID == branchID
                        select package;
            if(activeOnly)
                query = query.Where(package => package.IsActive);
            return query.ToList();
        }

        public IEnumerable<PackageHeader> GetAll()
        {
            var query = from package in context.PackageHeaders
                        where package.IsActive
                        select package;
            return query.ToList();
        }

        public void UpdatePackagesAtBranch(int packageID, IEnumerable<int> branchesID)
        {
            var package = context.PackageHeaders.SingleOrDefault(pkg => pkg.ID == packageID);
            package.Branches.Clear();

            foreach (int branchID in branchesID)
            {
                package.Branches.Add(context.Branches.Single(br => br.ID == branchID));

            }
            context.SaveChanges();
        }

        public IEnumerable<Class> PopulateClassForSelectedPackage(int packageID)
        {
            var result = new Dictionary<int, bool>();

            var classes = from acls in context.PackageHeaders
                          from cls in context.Classes 
                          where cls.IsActive && acls.ID == packageID
                          select cls;
            return classes.ToList();
        }


        public void UpdateClassPackage(int packageID, List<int> classesID)
        {
            var package = context.PackageHeaders.SingleOrDefault(pkg => pkg.ID == packageID);
            if (package != null)
            {
                package.Classes.Clear();

                foreach (int classID in classesID)
                {
                    Class activeClassInPackage = context.Classes.Single(cls => cls.ID == classID);
                    package.Classes.Add(activeClassInPackage);
                }
            }
            context.SaveChanges();
        }
    }
}
