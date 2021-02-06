using System;

namespace GoGym.Providers.ViewModels
{
    public class BillingRejectionViewModel : BillingViewModel
    {
        public string DeclineCode { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class BillingAcceptedViewModel : BillingViewModel
    {
        public string VerificationCode { get; set; }
        public string InvoiceNo { get; set; }
    }
}