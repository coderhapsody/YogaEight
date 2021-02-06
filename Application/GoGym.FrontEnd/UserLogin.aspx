<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterUnregisteredPage.Master" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="GoGym.FrontEnd.UserLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script>
        function TextBoxKeyPress(sender, args) {
            if (args._keyCode === 13) {
                $find("<%=btnLogin.ClientID%>").click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <br/>
    <img src="Images/logo_login.png" />
    <br/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="server">
    <hr />
    <br/>
    <div class="centered">
        <table class="noborder">
            <tr>
                <td>User Name</td>
                <td>:</td>
                <td>
                    <telerik:RadTextBox ID="txtUserName" runat="server" ClientEvents-OnKeyPress="TextBoxKeyPress">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Password</td>
                <td>:</td>
                <td>
                    <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" ClientEvents-OnKeyPress="TextBoxKeyPress">
                    </telerik:RadTextBox>
                </td>

            </tr>
            <tr>
                <td></td>
                <td></td>
                <td><telerik:RadButton runat="server" ID="btnLogin" Text="Login" SingleClick="True" SingleClickText="Authenticating" EnableViewState="false" OnClick="btnLogin_Click"></telerik:RadButton> </td>
            </tr>
        </table>
        <asp:Label runat="server" ID="lblStatus" EnableViewState="false"></asp:Label>        
    </div>
<br/>
</asp:Content>
