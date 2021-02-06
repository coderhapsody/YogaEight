using System;

namespace GoGym.Providers.Configuration
{
    public class CheckInConfiguration
    {
        public string ReportAgreementForm
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<string>("Report.AgreementForm"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("Report.AgreementForm", value); }
        }

        public bool ContractNotActiveAlert
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>(ConfigurationKeys.CheckIn.ContractNotActiveAlert); }
            set { ConfigurationSingletonProvider.Instance.SetValue(ConfigurationKeys.CheckIn.ContractNotActiveAlert, value); }
        }

        public bool ContractNotPaid
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>(ConfigurationKeys.CheckIn.ContractNotPaid); }
            set { ConfigurationSingletonProvider.Instance.SetValue(ConfigurationKeys.CheckIn.ContractNotPaid, value); }
        }

        public bool BirthdayAlert
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>(ConfigurationKeys.CheckIn.BirthdayAlert); }
            set { ConfigurationSingletonProvider.Instance.SetValue(ConfigurationKeys.CheckIn.BirthdayAlert, value); }
        }

        public bool CreditCardExpiringAlert
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>(ConfigurationKeys.CheckIn.CreditCardExpiringAlert); }
            set { ConfigurationSingletonProvider.Instance.SetValue(ConfigurationKeys.CheckIn.CreditCardExpiringAlert, value); }
        }

        public bool CreditCardExpired
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>(ConfigurationKeys.CheckIn.CreditCardExpired); }
            set { ConfigurationSingletonProvider.Instance.SetValue(ConfigurationKeys.CheckIn.CreditCardExpired, value); }
        }
    }
}