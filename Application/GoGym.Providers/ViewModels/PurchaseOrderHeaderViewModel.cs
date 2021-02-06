using System;

namespace GoGym.Providers.ViewModels
{
    public class PurchaseOrderHeaderViewModel
    {
        public string DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierNPWP { get; set; }
        public bool SupplierTaxable { get; set; }
        public string Terms { get; set; }
        public string Notes { get; set; }
        public decimal SubTotal { get; set; }        
        public decimal DiscountValue { get; set; }
        public decimal GrandTotal { get; set; }
        public string Terbilang { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string EmployeeName { get; set; }
        public string ApproverName { get; set; }
        public string Status { get; set; }
    }
}
