using System;

namespace GoGym.Providers.ViewModels
{

    public class MonthlyClosingViewModel
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public int ClosingMonth { get; set; }
        public string ClosingMonthName { get; set; }
        public int ClosingYear { get; set; }
        public DateTime ChangedWhen { get; set; }
        public string ChangedWho { get; set; }
    }
}