<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportCustomerStatusHistory.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportCustomerStatusHistory"  StyleSheetTheme="Workspace"%>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Customer Status History         
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
            <td class="style1">Start Date</td>
            <td class="style2">:</td>
            <td>From
                <telerik:RadDatePicker runat="server" ID="calDateFrom"></telerik:RadDatePicker>                
                &nbsp;
                To
                <telerik:RadDatePicker runat="server" ID="calDateTo"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td class="style1">Status</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlStatus" runat="server" Width="150px" />
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnViewReport" runat="server" Text="View Report" OnClientClicking="ViewReportClick" />
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <script>
        function ViewReportClick(sender, args) {
            sender.set_autoPostBack(false);
            var id = $find("<%=ddlBranch.ClientID%>").get_selectedItem().get_value();
            var customerStatusID = $find("<%=ddlStatus.ClientID%>").get_selectedItem().get_value();
            var dateFrom = $find("<%=calDateFrom.ClientID%>").get_selectedDate();
            var dateTo = $find("<%=calDateTo.ClientID%>").get_selectedDate();
            if (dateFrom != null && dateTo != null) {
                showSimplePopUp('PrintPreview.aspx?RDL=CustomerStatusHistory&BranchID=' + id + '&StartDateFrom=' + dateFrom.format("yyyy-MM-dd") + '&StartDateTo=' + dateTo.format("yyyy-MM-dd") + '&CustomerStatusID=' + customerStatusID);
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
