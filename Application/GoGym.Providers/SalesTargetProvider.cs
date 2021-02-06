using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class SalesTargetProvider : BaseProvider
    {
        public SalesTargetProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public SalesTarget GetTarget(int id)
        {
            return context.SalesTargets.SingleOrDefault(target => target.ID == id);
        }

        public void AddTarget(int branchID,
                              int year,
                              int month,
                              int freshMemberUnit,
                              int upgradeUnit,
                              int renewalUnit,
                              decimal freshMemberRevenue,
                              decimal upgradeRevenue,
                              decimal renewalRevenue,
                              decimal pilatesRevenue,
                              decimal vocalRevenue,
                              decimal eftCollectionRevenue,
                              int dropOffUnit,
                              decimal cancelFees,
                              int freezeUnit,
                              decimal freezeFees,
                              decimal otherRevenue)
        {
            var salesTarget = new SalesTarget();
            salesTarget.BranchID = branchID;
            salesTarget.Year = year;
            salesTarget.Month = month;
            salesTarget.FreshMemberUnit = freshMemberUnit;
            salesTarget.RenewalUnit = renewalUnit;
            salesTarget.UpgradeUnit = upgradeUnit;
            salesTarget.FreshMemberRevenue = freshMemberRevenue;
            salesTarget.RenewalRevenue = renewalRevenue;
            salesTarget.UpgradeRevenue = upgradeRevenue;
            salesTarget.PilatesRevenue = pilatesRevenue;
            salesTarget.VocalRevenue = vocalRevenue;
            salesTarget.EFTCollectionRevenue = eftCollectionRevenue;
            salesTarget.DropOffUnit = dropOffUnit;
            salesTarget.CancelFees = cancelFees;
            salesTarget.FreezeFees = freezeFees;
            salesTarget.FreezeUnit = freezeUnit;
            salesTarget.OtherRevenue = otherRevenue;
            EntityHelper.SetAuditFieldForInsert(salesTarget, principal.Identity.Name);
            context.Add(salesTarget);
            context.SaveChanges();
        }

        public void UpdateTarget(int id,
                                 int branchID,
                                 int year,
                                 int month,
                                 int freshMemberUnit,
                                 int upgradeUnit,
                                 int renewalUnit,
                                 decimal freshMemberRevenue,
                                 decimal upgradeRevenue,
                                 decimal renewalRevenue,
                                 decimal pilatesRevenue,
                                 decimal vocalRevenue,
                                 decimal eftCollectionRevenue,
                                 int dropOffUnit,
                                 decimal cancelFees,
                                 int freezeUnit,
                                 decimal freezeFees,
                                 decimal otherRevenue)
        {
            SalesTarget salesTarget = context.SalesTargets.SingleOrDefault(target => target.ID == id);
            if (salesTarget != null)
            {
                salesTarget.BranchID = branchID;
                salesTarget.Year = year;
                salesTarget.Month = month;
                salesTarget.FreshMemberUnit = freshMemberUnit;
                salesTarget.RenewalUnit = renewalUnit;
                salesTarget.UpgradeUnit = upgradeUnit;
                salesTarget.FreshMemberRevenue = freshMemberRevenue;
                salesTarget.RenewalRevenue = renewalRevenue;
                salesTarget.UpgradeRevenue = upgradeRevenue;
                salesTarget.PilatesRevenue = pilatesRevenue;
                salesTarget.VocalRevenue = vocalRevenue;
                salesTarget.EFTCollectionRevenue = eftCollectionRevenue;
                salesTarget.DropOffUnit = dropOffUnit;
                salesTarget.CancelFees = cancelFees;
                salesTarget.FreezeFees = freezeFees;
                salesTarget.FreezeUnit = freezeUnit;
                salesTarget.OtherRevenue = otherRevenue;
                EntityHelper.SetAuditFieldForUpdate(salesTarget, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public void DeleteTarget(int id)
        {
            context.Delete(
                context.SalesTargets.SingleOrDefault(target => target.ID == id));
            context.SaveChanges();
        }

        public void DeleteTarget(int[] id)
        {
            context.Delete(
                context.SalesTargets.Where(target => id.Contains(target.ID)));
            context.SaveChanges();
        }
    }
}
    

