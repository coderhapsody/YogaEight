<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterItemAccount.aspx.cs" Inherits="GoGym.FrontEnd.MasterItemAccount" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Item Account
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">

    <table class="style1">
        <tr>
            <td style="width: 30%; vertical-align: top;">
                <p>
                    <em>Account with asterisk sign (*) is inactive account</em>
                </p>
                <asp:TreeView ID="tvwAccount" runat="server" PopulateNodesFromClient="true" EnableClientScript="true"
                    OnSelectedNodeChanged="tvwAccount_SelectedNodeChanged"
                    OnTreeNodePopulate="tvwAccount_TreeNodePopulate">
                </asp:TreeView>
            </td>
            <td style="vertical-align: top;">
                <p>
                    <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false"
                        Text="Add New" SkinID="AddNewButton" OnClick="lnbAddNew_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false"
                            Text="Delete" OnClientClick="return confirm('Delete marked row(s) ?')"
                            SkinID="DeleteButton" OnClick="lnbDelete_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                </p>
                <table class="style1">
                    <tr>
                        <td class="style7">Account No.</td>
                        <td class="style5">:</td>
                        <td class="style6">
                            <telerik:RadTextBox ID="txtAccountNo" runat="server" Width="100px" ValidationGroup="AddEdit" />
                            <asp:RequiredFieldValidator ID="rqvAccountNo" runat="server" ErrorMessage="<b>Account Number</b> must be specified uniquely" ControlToValidate="txtAccountNo" EnableViewState="false" SetFocusOnError="true" ValidationGroup="AddEdit" CssClass="errorMessage" />

                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Description</td>
                        <td class="style3">:</td>
                        <td>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" ValidationGroup="AddEdit" Width="250px" />
                            <asp:RequiredFieldValidator ID="rqvDescription" runat="server" ErrorMessage="<b>Description</b> must be specified" ControlToValidate="txtAccountDescription" EnableViewState="false" SetFocusOnError="true" ValidationGroup="AddEdit" CssClass="errorMessage" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Parent Account</td>
                        <td class="style3">:</td>
                        <td>
                            <telerik:RadDropDownList ID="ddlParentAccount" runat="server" DropDownHeight="200px" Width="300px" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" Text="This account is active" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                        <td>
                            <asp:CheckBox ID="chkCascade" runat="server"
                                Text="Cascade account state to its child" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style7">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                        <td>
                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="AddEdit" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style3 {
            width: 7px;
        }

        .style5 {
            width: 7px;
            height: 17px;
        }

        .style6 {
            height: 17px;
        }

        .style7 {
            width: 120px;
        }
    </style>
</asp:Content>


