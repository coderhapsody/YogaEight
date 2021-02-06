using System;
using System.Linq;
using System.Web.UI.WebControls;
using GoGym.Providers;
using Telerik.Web.UI;

namespace GoGym.FrontEnd.Helpers
{
    /// <summary>
    /// Summary description for DataBindingHelper
    /// </summary>
    public static class DataBindingHelper
    {
        public static void PopulateActiveBranches(BranchProvider branchProvider, RadDropDownList dropdown, bool addEmptyOption = false)
        {            
            dropdown.DataSource = branchProvider.GetActiveBranches();
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulateActiveBranches(BranchProvider branchProvider, DropDownList dropdown, string userName, bool addEmptyOption = false)
        {
            dropdown.DataSource = branchProvider.GetActiveBranches(userName);
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, String.Empty);
        }

        public static void PopulateActiveBranches(BranchProvider branchProvider, RadDropDownList dropdown, string userName, bool addEmptyOption = false)
        {
            dropdown.DataSource = branchProvider.GetActiveBranches(userName);
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulateAllowedBranches(BranchProvider branchProvider, RadDropDownList dropdown, string userName, bool addEmptyOption = false)
        {
            dropdown.DataSource = branchProvider.GetAllowedBranches(userName);
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulatePaidClasses(ClassProvider classProvider, DropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = classProvider.GetAllClasses().Where(cls => cls.IsPaid);
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, String.Empty);
        }

        public static void PopulatePaidClasses(ClassProvider classProvider, RadDropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = classProvider.GetAllClasses().Where(cls => cls.IsPaid);
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulateAllClasses(ClassProvider classProvider, DropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = classProvider.GetAllClasses();
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, String.Empty);
        }


        public static void PopulateBillingTypes(BillingTypeProvider billingTypeProvider, RadDropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = billingTypeProvider.GetActiveBillingTypes();
            dropdown.DataTextField = "Description";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulateCreditCardTypes(CreditCardTypeProvider creditCardTypeProvider, DropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = creditCardTypeProvider.GetAll();
            dropdown.DataTextField = "Description";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, String.Empty);
        }


        public static void PopulatePackages(PackageProvider packageProvider, RadDropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = packageProvider.GetAll();
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulateCustomerStatus(CustomerStatusProvider customerStatusProvider, DropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = customerStatusProvider.GetAll();
            dropdown.DataTextField = "Description";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, String.Empty);
        }

        public static void PopulateCustomerStatus(CustomerStatusProvider customerStatusProvider, RadDropDownList dropdown, bool addEmptyOption = false)
        {
            dropdown.DataSource = customerStatusProvider.GetAll();
            dropdown.DataTextField = "Description";
            dropdown.DataValueField = "ID";
            dropdown.DataBind();

            if (addEmptyOption)
                dropdown.Items.Insert(0, new DropDownListItem(String.Empty));
        }

        public static void PopulateMonths(RadDropDownList ddlMonth)
        {
            for(var month = 1; month <= 12; month++)
            {
                ddlMonth.Items.Add(new DropDownListItem(new DateTime(DateTime.Today.Year, month, 1).ToString("MMMM"), Convert.ToString(month)));
            }
        }
    }
}