<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportCreditCardChangeHistory.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportCreditCardChangeHistory"  StyleSheetTheme="Workspace"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 110px;
        }
        .auto-style2 {
            width: 7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Credit Card Change History
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />                
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Month</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlMonth" runat="server" Width="100px"  />                
            </td>
        </tr>
            <tr>
            <td class="auto-style1">Year</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlYear" runat="server" Width="100px"/>                
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnViewReport" runat="server" Text="View Report" OnClientClicking="ViewReportClick" />
            </td>
        </tr>        
    </table>

    <script>
        function ViewReportClick(sender, args) {
            sender.set_autoPostBack(false);
            var id = $find("<%=ddlBranch.ClientID%>").get_selectedItem().get_value();
            var month = $find("<%=ddlMonth.ClientID%>").get_selectedItem().get_value();
            var year = $find("<%=ddlYear.ClientID%>").get_selectedItem().get_text();
            showSimplePopUp('PrintPreview.aspx?RDL=CreditCardChangeHistory&BranchID=' + id + '&Month=' + month + '&Year=' + year);
        }
    </script>
</asp:Content>

