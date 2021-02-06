using System;

namespace GoGym.Data
{
    /// <summary>
    /// Summary description for EntityHelper
    /// </summary>
    public static class EntityHelper
    {
        public static void SetAuditField(int id, dynamic entity, string userName)
        {
            if (id == 0) 
                SetAuditFieldForInsert(entity, userName);
            else
                SetAuditFieldForUpdate(entity, userName);

        }


        public static void SetAuditFieldForInsert(dynamic entity, string userName)
        {
            entity.ChangedWhen = DateTime.Now;
            entity.ChangedWho = userName;
            entity.CreatedWhen = DateTime.Now;
            entity.CreatedWho = userName;        
        }


        public static void SetAuditFieldForUpdate(dynamic entity, string userName)
        {
            entity.ChangedWhen = DateTime.Now;
            entity.ChangedWho = userName;
        }
    }
}