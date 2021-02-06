using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGym.Providers
{
    public static class ConfigurationKeys
    {
        public static class CheckIn
        {
            public static readonly string BirthdayAlert = "CheckIn.BirthdayAlert";
            public static readonly string ContractNotActiveAlert = "CheckIn.ContractNotActiveAlert";
            public static readonly string ContractNotPaid = "CheckIn.ContractNotPaid";
            public static readonly string CreditCardExpired = "CheckIn.CreditCardExpired";
            public static readonly string CreditCardExpiringAlert = "CheckIn.CreditCardExpiringAlert";
        }

        public static class Login
        {
            public static readonly string History = "Login.History";
        }

        public static class UI
        {
            public static readonly string GridRollOverColor = "UI.GridRollOverColor";
        }

        public static class Roles
        {
            public static readonly string TrainerRoleID = "Roles.TrainerRoleID";
            public static readonly string SalesRoleID = "Roles.SalesRoleID";
        }
    }
}
