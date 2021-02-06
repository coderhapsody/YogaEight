using System;

namespace GoGym.Providers.ViewModels
{
    [Serializable]
    public class PurchaseOrderDetailViewModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitName { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsTaxed { get; set; }
        public decimal TaxValue { get; set; }
        public decimal Total { get; set; }
    }
}
