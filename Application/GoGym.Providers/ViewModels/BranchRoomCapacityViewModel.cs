using System;
using GoGym.Data;

namespace GoGym.Providers.ViewModels
{
    /// <summary>
    /// Summary description for BranchRoomCapacityViewModel
    /// </summary>
    public class BranchRoomCapacityViewModel
    {
        public int BranchID { get; set; }
        public Branch CurrentBranch { get; set; }
        public int Capacity { get; set; }

    }
}