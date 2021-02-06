using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;
using Telerik.OpenAccess.Metadata.Fluent;

namespace GoGym.Providers.Base
{
    public abstract class BaseProvider
    {
        protected readonly FitnessEntities context;
        protected readonly IPrincipal principal;

        protected readonly string cryptographyKey = ConfigurationManager.AppSettings[ApplicationSettingKeys.CryptographyKey];

        public string CurrentUserName
        {
            get
            {
                return principal != null ? principal.Identity.Name : String.Empty;
            }
        }

        protected BaseProvider(FitnessEntities context, IPrincipal principal)
        {
            this.context = context;
            this.principal = principal;
        }

        [DebuggerStepThrough]
        protected void SetAuditFields(dynamic entity)
        {
            EntityHelper.SetAuditField(entity.ID, entity, principal.Identity.Name);
        }
    }
}
