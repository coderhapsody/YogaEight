using System;

namespace GoGym.Providers.ViewModels
{
    [Serializable]
    public class ReceivingDetailViewModel
    {
        public int ID { get; set; }
        public int ReceivingID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int QtyReceived { get; set; }
        public int QtyPO { get; set; }
        public string Notes { get; set; }

    }
}
