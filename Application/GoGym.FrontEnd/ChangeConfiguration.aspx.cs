using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd
{
    public partial class ChangeConfiguration : BaseForm
    {
        [Inject]
        public SecurityProvider SecurityService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigurations();
                PopulateDropDown();
            }
        }

        private void PopulateDropDown()
        {
            var roles = SecurityService.GetAllRoles();
            
            ddlRoleTrainer.DataSource = roles;
            ddlRoleTrainer.DataValueField = "ID";
            ddlRoleTrainer.DataTextField = "Name";
            ddlRoleTrainer.DataBind();

            ddlRoleSales.DataSource = roles;
            ddlRoleSales.DataValueField = "ID";
            ddlRoleSales.DataTextField = "Name";
            ddlRoleSales.DataBind();

        }

        private void LoadConfigurations()
        {
            try
            {
                chkBirthdayAlert.Checked =
                    Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.BirthdayAlert]);

                chkContractNotActive.Checked =
                    Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.ContractNotActiveAlert]);

                chkContractNotPaid.Checked =
                    Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.ContractNotPaid]);

                chkCreditCardExpired.Checked =
                    Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.CreditCardExpired]);

                chkCreditCardExpiring.Checked =
                    Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.CreditCardExpiringAlert]);

                chkLoginHistory.Checked =
                    Convert.ToBoolean(ConfigurationSingletonProvider.Instance[ConfigurationKeys.Login.History]);

                colGridRollOver.SelectedColor =
                    ColorTranslator.FromHtml(ConfigurationSingletonProvider.Instance[ConfigurationKeys.UI.GridRollOverColor]);

                ddlRoleTrainer.SelectedValue = ConfigurationSingletonProvider.Instance[ConfigurationKeys.Roles.TrainerRoleID];
                ddlRoleSales.SelectedValue = ConfigurationSingletonProvider.Instance[ConfigurationKeys.Roles.SalesRoleID];
            }
            catch (Exception ex)
            {
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                try
                {
                    ConfigurationSingletonProvider.Instance.SetUserName(User.Identity.Name);

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.BirthdayAlert] =
                        chkBirthdayAlert.Checked.ToString();

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.ContractNotActiveAlert] =
                        chkContractNotActive.Checked.ToString();

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.ContractNotPaid] =
                        chkContractNotPaid.Checked.ToString();

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.CreditCardExpired] =
                        chkCreditCardExpired.Checked.ToString();

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.CheckIn.CreditCardExpiringAlert] =
                        chkCreditCardExpiring.Checked.ToString();

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.Login.History] =
                        chkLoginHistory.Checked.ToString();

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.UI.GridRollOverColor] =
                        ColorTranslator.ToHtml(colGridRollOver.SelectedColor);

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.Roles.TrainerRoleID] =
                        ddlRoleTrainer.SelectedValue;

                    ConfigurationSingletonProvider.Instance[ConfigurationKeys.Roles.SalesRoleID] =
                        ddlRoleSales.SelectedValue;

                    WebFormHelper.SetLabelTextWithCssClass(lblStatus,
                        "Configurations are saved.",
                        LabelStyleNames.InfoMessage);
                }
                catch (Exception ex)
                {
                    WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    LogService.ErrorException(GetType().FullName, ex);
                }
            }
        }
    }
}