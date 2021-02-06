<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="RePrint.aspx.cs" Inherits="GoGym.FrontEnd.RePrint" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Re-Print Invoice
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManagerProxy runat="server" ID="scmScriptManagerProxy">
        <Services>
            <asp:ServiceReference Path="~/Services/CheckInService.svc" />
        </Services>
    </asp:ScriptManagerProxy>
    <table class="style1">
        <tr>
            <td class="style2">Invoice No.
            </td>
            <td class="style3">:
            </td>
            <td>
                <telerik:RadTextBox ID="txtInvoiceNo" runat="server" Width="120px" />&nbsp;
                <asp:HyperLink ID="hypLookUpInvoice" NavigateUrl="#" runat="server">Look Up</asp:HyperLink>
                &nbsp;
                <asp:RequiredFieldValidator ID="rqvInvoiceNo" runat="server"
                    ControlToValidate="txtInvoiceNo" CssClass="errorMessage"
                    EnableViewState="False"
                    ErrorMessage="&lt;b&gt;Invoice Number&lt;/b&gt; must be specified"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style2">&nbsp;
            </td>
            <td class="style3">&nbsp;
            </td>
            <td>
                <telerik:RadButton ID="btnReprint" runat="server" EnableViewState="False" Text="Re-Print This Invoice" OnClientClicking="ReprintClick" />
                &nbsp;<asp:Label ID="lblStatus0" runat="server" EnableViewState="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnDummy" runat="server" CausesValidation="False" CssClass="textbox"
        EnableViewState="False" Text="Cancel" Style="display: none;" />

    <telerik:RadWindow runat="server" ID="wndAuth" Width="550px" Height="300px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close">
        <ContentTemplate>
            <fieldset style="text-align: left;">
                <legend>Invoice Information</legend>
                <table>
                    <tr>
                        <td>Invoice No.</td>
                        <td>:</td>
                        <td><span id="invoiceNo"></span></td>
                    </tr>
                    <tr>
                        <td>Invoice type</td>
                        <td>:</td>
                        <td><span id="invoiceType"></span></td>
                    </tr>
                    <tr>
                        <td>Invoice Date</td>
                        <td>:</td>
                        <td><span id="invoiceDate"></span></td>
                    </tr>
                    <tr>
                        <td>Customer</td>
                        <td>:</td>
                        <td><span id="customer"></span></td>
                    </tr>
                    <tr>
                        <td>Contract No.</td>
                        <td>:</td>
                        <td><span id="contractNo"></span></td>
                    </tr>
                </table>
            </fieldset>
            <br/>
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>User Name
                    </td>
                    <td>:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUserName" runat="server" Width="120px"/>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>Password
                    </td>
                    <td>:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassword" runat="server" Width="120px" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadButton ID="btnPopupOK" runat="server" EnableViewState="False" Text="Authorize" OnClick="btnPopupOK_Click" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </telerik:RadWindow>
    <%--    <AjaxToolkit:ModalPopupExtender ID="mopAuth" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnPopupCancel" DropShadow="True" PopupControlID="pnlAuth" PopupDragHandleControlID="pnlAuth"  
        TargetControlID="btnDummy">        
    </AjaxToolkit:ModalPopupExtender>    --%>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 160px;
        }

        .style3 {
            width: 2px;
        }
    </style>

    <script>
        function ReprintClick(sender, args) {
            sender.set_autoPostBack(false);
            var invoiceNo = $find("<%=txtInvoiceNo.ClientID%>").get_value();
            var svc = new AjaxService.CheckInService();
            svc.GetInvoiceInfo(invoiceNo, function (invoiceInfo) {
                var oWnd = $telerik.findWindow("<%=wndAuth.ClientID%>");
                oWnd.show();
                document.getElementById("invoiceNo").innerText = invoiceInfo.InvoiceNo;
                document.getElementById("invoiceType").innerText = invoiceInfo.InvoiceType;
                document.getElementById("invoiceDate").innerText = moment(invoiceInfo.InvoiceDate).format("dddd, DD MMMM YYYY");
                document.getElementById("customer").innerText = invoiceInfo.CustomerName;
                document.getElementById("contractNo").innerText = invoiceInfo.ContractNo;
                $find("<%=txtUserName.ClientID%>").set_value('');
                $find("<%=txtPassword.ClientID%>").set_value('');
                $find("<%=txtUserName.ClientID%>").focus();
            }, function(error) {
                alert(error);
            });
        }
    </script>
</asp:Content>
