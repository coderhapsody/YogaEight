<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="GoGym.FrontEnd.ChangePassword" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Change Password
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView runat="server" ID="mvwForm">
        <asp:View ID="View1" runat="server">
            <table class="noborder">
                <tr>
                    <td style="width: 200px;">Current Password</td>
                    <td style="width: 5px;">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCurrentPassword" TextMode="Password" MaxLength="30"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqvCurrentPassword" SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" ControlToValidate="txtCurrentPassword" ErrorMessage="<b>Current Password</b> must be specified"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cuvCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Current Password&lt;/b&gt; is invalid" OnServerValidate="cuvCurrentPassword_ServerValidate" SetFocusOnError="True"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>New Password</td>
                    <td>:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtNewPassword" TextMode="Password" MaxLength="30"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqvNewPassword" SetFocusOnError="true" CssClass="errorMessage" Display="Dynamic" ControlToValidate="txtNewPassword" ErrorMessage="<b>New Password</b> must be specified"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Confirm Password</td>
                    <td>:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtConfirmPassword" TextMode="Password" MaxLength="30"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rqvConfirmPassword" SetFocusOnError="true" CssClass="errorMessage" ControlToValidate="txtConfirmPassword" ErrorMessage="<b>Confirm Password</b> must be specified" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator runat="server" ID="cmpPassword" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword" CssClass="errorMessage" ErrorMessage="<b>Confirm Password</b> and <b>New Password</b> must be same" Display="Dynamic"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadButton runat="server" ID="btnChangePassword" SingleClick="true" SingleClickText="Changing your password" Text="Change Password" OnClick="btnChangePassword_Click"></telerik:RadButton>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <h1>Change Password Success</h1>
            <div>
                You can try to log in to the application using your new password.
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
