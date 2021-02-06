<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportDailyPayment.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportDailyPayment"  StyleSheetTheme="Workspace"%>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Daily Payment
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">Branch</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />
            </td>
        </tr>
        <tr>
            <td class="style1">Date From</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDatePicker runat="server" ID="calDateFrom"/>
            </td>
        </tr>
        <tr>
            <td class="style1">Date To</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDatePicker runat="server" id="calDateTo" />
            </td>
        </tr>

        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnViewReport" runat="server" Text="View Report" OnClientClicking="ViewReportClick" />
            </td>
        </tr>
    </table>

    <script>
        function ViewReportClick(sender, args) {
            sender.set_autoPostBack(false);
            var id = $find("<%=ddlBranch.ClientID%>").get_selectedItem().get_value();
            var dateFrom = $find("<%=calDateFrom.ClientID%>").get_selectedDate();
            var dateTo = $find("<%=calDateTo.ClientID%>").get_selectedDate();
            if (dateFrom != null && dateTo != null) {
                showSimplePopUp('PrintPreview.aspx?RDL=DailyPayment&BranchID=' + id + '&PaymentDateFrom=' + dateFrom.format("yyyy-MM-dd") + '&PaymentDateTo=' + dateTo.format("yyyy-MM-dd"));
            } else {
                alert('Date is invalid.');
            }
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

