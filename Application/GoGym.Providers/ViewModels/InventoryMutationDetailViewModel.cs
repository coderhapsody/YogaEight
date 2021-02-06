using System;

namespace GoGym.Providers.ViewModels
{
    [Serializable]
    public class InventoryMutationDetailViewModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemBarcode { get; set; }
        public string ItemDescription { get; set; }
        public int Qty { get; set; }
        public string Notes { get; set; }
        public string UnitName { get; set; }
    }
}
