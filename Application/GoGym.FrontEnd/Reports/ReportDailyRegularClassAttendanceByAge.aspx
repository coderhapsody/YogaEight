<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportDailyRegularClassAttendanceByAge.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportDailyRegularClassAttendanceByAge"  StyleSheetTheme="Workspace"%>
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
    Daily Regular Class Attendance by Age        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">Branch</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" width="250px" />                
            </td>
        </tr>
        <tr>
            <td class="style1">Date</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDatePicker ID="calDate" runat="server"/>
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
            var date = $find("<%=calDate.ClientID%>").get_selectedDate();
            if (date != null) {
                showSimplePopUp('PrintPreview.aspx?RDL=DailyRegularClassAttendanceByAge&BranchID=' + id + '&Date=' + date.format("yyyy-MM-dd"));
            } else {
                alert('Date is invalid.');
            }
        }
    </script>
</asp:Content>

