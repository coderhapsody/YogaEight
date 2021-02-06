<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportFBDailySales.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportFBDailySales"  StyleSheetTheme="Workspace"%>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    F&B Daily Sales
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table>
        <tr>
            <td class="style1">Branch</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server"  Width="250px"/>
                
            </td>
        </tr>
        <tr>
            <td class="style1">Month</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDatePicker runat="server" ID="calDate" />
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
            var id = $find("<%=ddlBranch.ClientID%>").get_selectedItem().get_value();
            var date = $find("<%=calDate.ClientID%>").get_selectedDate();
            if (date != null) {
                showSimplePopUp('PrintPreview.aspx?RDL=FBDailySales&BranchID=' + id + '&Date=' + date.format("yyyy-MM-dd"));
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
