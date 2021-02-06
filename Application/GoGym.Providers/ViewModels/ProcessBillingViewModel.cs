using System;

namespace GoGym.Providers.ViewModels
{
    public class ProcessBillingViewModel
    {
        public string CustomerBarcode { get; set; }
        public string CustomerName { get; set; }
        public string PackageName { get; set; }
        public string ContractNo { get; set; }
        public string CustomerStatus { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime NextDueDate { get; set; }
        public decimal DuesAmount { get; set; }
    }
}