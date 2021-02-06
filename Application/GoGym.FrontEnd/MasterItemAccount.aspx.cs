using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoGym.Data;
using GoGym.FrontEnd.Base;
using GoGym.FrontEnd.Helpers;
using GoGym.Providers;
using Ninject;
using Telerik.Web.UI;

namespace GoGym.FrontEnd
{
    public partial class MasterItemAccount : BaseForm
    {
        [Inject]
        public ItemAccountProvider ItemAccountService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillParentAccountDropDown();
                RowID = 0;
                RefreshTreeView();
            }
        }

        private void FillParentAccountDropDown()
        {
            ddlParentAccount.Items.Clear();
            ddlParentAccount.Items.Insert(0, new DropDownListItem("Root Account", "0"));

            foreach (var account in ItemAccountService.GetAll())
            {
                ddlParentAccount.Items.Add(new DropDownListItem(String.Format("{0} - {1}", account.AccountNo, account.Description), Convert.ToString(account.ID)));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                switch (RowID)
                {
                    case 0:
                        ItemAccountService.Add(
                            txtAccountNo.Text,
                            txtAccountDescription.Text,
                            Convert.ToInt32(ddlParentAccount.SelectedValue) == 0 ? null : (int?)Convert.ToInt32(ddlParentAccount.SelectedValue),
                            chkActive.Checked);
                        break;
                    default:
                        ItemAccountService.Update(
                            RowID,
                            txtAccountNo.Text,
                            txtAccountDescription.Text,
                            Convert.ToInt32(ddlParentAccount.SelectedValue) == 0 ? null : (int?)Convert.ToInt32(ddlParentAccount.SelectedValue),
                            chkActive.Checked);
                        break;
                }

                if (chkCascade.Checked)
                    ItemAccountService.DisableAccountCascade(txtAccountNo.Text, chkActive.Checked);

                FillParentAccountDropDown();
                RefreshTreeView();
                RestoreTreeView(txtAccountNo.Text);
                WebFormHelper.SetLabelTextWithCssClass(
                        lblMessage,
                        String.Format("Account <b>{0}</b> has been saved.", txtAccountNo.Text),
                        LabelStyleNames.AlternateMessage);

                lnbAddNew_Click(sender, e);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        private void RestoreTreeView(string fromAccountNo)
        {
            Stack<ItemAccount> stack = ItemAccountService.GetAccountHierarchy(fromAccountNo);
            TreeNodeCollection nodes = tvwAccount.Nodes;
            while (stack.Count > 0)
            {
                ItemAccount account = stack.Pop();
                foreach (TreeNode node in nodes)
                {
                    if (Convert.ToInt32(node.Value) == account.ID)
                    {
                        node.Expand();
                        nodes = node.ChildNodes;
                        break;
                    }
                }

            }
        }



        private void RefreshTreeView()
        {
            tvwAccount.Nodes.Clear();
            foreach (ItemAccount account in ItemAccountService.GetRootAccount())
            {
                var node = new TreeNode(String.Format("{0}{1} - {2}", account.IsActive ? String.Empty : "*", account.AccountNo, account.Description), account.ID.ToString());
                node.PopulateOnDemand = true;
                node.SelectAction = TreeNodeSelectAction.SelectExpand;
                node.Expanded = false;
                tvwAccount.Nodes.Add(node);
            }
        }

        protected void lnbAddNew_Click(object sender, EventArgs e)
        {
            RowID = 0;
            WebFormHelper.ClearTextBox(txtAccountNo, txtAccountDescription);
            ddlParentAccount.SelectedValue = "0";
            chkActive.Checked = true;
            chkCascade.Checked = false;
            txtAccountNo.Focus();
        }

        protected void lnbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (RowID > 0)
                {
                    int id = Convert.ToInt32(tvwAccount.SelectedNode.Value);
                    ItemAccount currentAccount = ItemAccountService.Get(id);
                    IEnumerable<ItemAccount> account = ItemAccountService.GetChildAccount(id);
                    if (account.Any())
                    {
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblMessage,
                            String.Format("Cannot delete account <b>{0}</b> since it has one or more child accounts", currentAccount.AccountNo),
                            LabelStyleNames.ErrorMessage);
                    }
                    else
                    {
                        ItemAccountService.Delete(new[] { id });
                        RefreshTreeView();
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblMessage,
                            String.Format("Account <b>{0}</b> has been deleted.", currentAccount.AccountNo),
                            LabelStyleNames.AlternateMessage);
                    }
                }
                else
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                            lblMessage,
                            "No account selected, select an account first.",
                            LabelStyleNames.ErrorMessage);
                }
                RefreshTreeView();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
                LogService.ErrorException(GetType().FullName, ex);
            }
        }

        protected void tvwAccount_SelectedNodeChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tvwAccount.SelectedNode.Value);
            RowID = id;
            ItemAccount account = ItemAccountService.Get(id);
            txtAccountNo.Text = account.AccountNo;
            txtAccountDescription.Text = account.Description;
            chkActive.Checked = account.IsActive;
            chkCascade.Checked = false;
            ddlParentAccount.SelectedValue = account.ParentID.HasValue ? account.ParentID.Value.ToString() : "0";
        }
        protected void tvwAccount_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            int id = Convert.ToInt32(e.Node.Value);
            foreach (ItemAccount account in ItemAccountService.GetChildAccount(id))
            {
                TreeNode node = new TreeNode();
                node.Text = String.Format("{0}{1} - {2}", account.IsActive ? String.Empty : "*", account.AccountNo, account.Description);
                node.Value = account.ID.ToString();
                node.PopulateOnDemand = true;
                node.SelectAction = TreeNodeSelectAction.SelectExpand;
                node.Expanded = false;
                e.Node.ChildNodes.Add(node);
            }
        }
    }
}