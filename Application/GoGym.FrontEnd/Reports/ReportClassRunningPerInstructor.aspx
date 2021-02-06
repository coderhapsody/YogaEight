<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportClassRunningPerInstructor.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportClassRunningPerInstructor" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .style1 {
            width: 130px;
        }

        .style2 {
            width: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Class Running Per Instructor
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
            <td class="style1">Instructor</td>
            <td class="style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlInstructor" runat="server" Width="100px" DropDownHeight="150px" />
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
            var month = $find("<%=ddlMonth.ClientID%>").get_selectedItem().get_value();
            var year = $find("<%=ddlYear.ClientID%>").get_selectedItem().get_text();
            var instructor = $find("<%=ddlInstructor.ClientID%>").get_selectedItem().get_value();            
            showSimplePopUp('PrintPreview.aspx?RDL=ClassRunningPerInstructor&InstructorID=' + instructor + '&BranchID=' + id + '&Month=' + month + '&Year=' + year);
        }
    </script>
</asp:Content>


