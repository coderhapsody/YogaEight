﻿using System;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.Reports
{
    public partial class ReportUnpaidContracts : BaseForm
    {
        [Inject]
        public BranchProvider BranchService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataBindingHelper.PopulateActiveBranches(BranchService, ddlBranch, User.Identity.Name, false);
            ddlBranch.SelectedValue = Convert.ToString(HomeBranchID);
        }
    }
}