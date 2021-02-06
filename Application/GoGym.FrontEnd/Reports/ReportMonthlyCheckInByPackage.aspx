<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportMonthlyCheckInByPackage.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportMonthlyCheckInByPackage" StyleSheetTheme="Workspace"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .style1 {
            width: 130px;
        }

        .style2 {
            width: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Monthly Check-in By Package
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table>
        <tr>
            <td class="style1">Branch</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px"/>                
            </td>
        </tr>
        <tr>
            <td class="style1">Month</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlMonth" runat="server" Width="100px"/>                
            </td>
        </tr>
            <tr>
            <td class="style1">Year</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlYear" runat="server" Width="100px" />                
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnViewReport" runat="server" Text="View Report" EnableViewState="false" OnClientClicking="ViewReportClick" />
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function ViewReportClick(sender, args) {
            sender.set_autoPostBack(false);
            var id = $find("#<%= ddlBranch.ClientID %>").get_selectedItem().get_value();
            var month = $find("#<%= ddlMonth.ClientID %>").get_selectedItem().get_value();
            var year = $find("#<%= ddlYear.ClientID %>").get_selectedItem().get_text();
            showSimplePopUp('PrintPreview.aspx?RDL=MonthlyCheckInByPackage&BranchID=' + id + '&Month=' + month + '&Year=' + year);
        }
    </script>
</asp:Content>

