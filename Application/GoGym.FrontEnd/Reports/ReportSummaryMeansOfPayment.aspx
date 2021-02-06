<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportSummaryMeansOfPayment.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportSummaryMeansOfPayment" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Summary Means of Payment
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table>
        <tr>
            <td class="style1">Branch</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />
            </td>
        </tr>
        <tr>
            <td class="style1">Month</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlMonth" runat="server" Width="100px" />
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
                <telerik:RadButton ID="btnViewReport" runat="server" Text="View Report" EnableViewState="False" OnClientClicking="ViewReportClick" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function ViewReportClick(sender, args) {
            sender.set_autoPostBack(false);
            var id = $("<%= ddlBranch.ClientID %>").get_selectedItem().get_value();
            var month = $("<%= ddlMonth.ClientID %>").get_selectedItem().get_value();
            var year = $("<%= ddlYear.ClientID %>").get_selectedItem().get_text();
            showSimplePopUp('PrintPreview.aspx?RDL=SummaryMeansOfPayment&BranchID=' + id + '&Month=' + month + '&Year=' + year);
        }
    </script>
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 130px;
        }

        .style2 {
            width: 4px;
        }
    </style>
</asp:Content>
