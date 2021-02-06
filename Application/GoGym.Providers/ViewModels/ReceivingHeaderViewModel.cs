using System;

namespace GoGym.Providers.ViewModels
{
    [Serializable]
    public class ReceivingHeaderViewModel
    {
        public int ID { get; set; }
        public int PurchaseOrderID { get; set; }
        public DateTime Date { get; set; }
        public string GoodIssueNo { get; set; }
        public string FreightInfo { get; set; }
        public DateTime VoidDate { get; set; }
        public string VoidReason { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
