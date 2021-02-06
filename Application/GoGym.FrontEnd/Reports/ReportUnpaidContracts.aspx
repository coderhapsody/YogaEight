<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportUnpaidContracts.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportUnpaidContracts" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Unpaid Contracts
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table>
        <tr>
            <td class="style1">
                Branch</td>
            <td class="style2">
                :</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />                
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                <telerik:RadButton ID="btnViewReport" runat="server" Text="View Report" EnableViewState="False" OnClientClicking="ViewReportClick" />
            </td>
        </tr>        
    </table>

    <script language="javascript" type="text/javascript">
        function ViewReportClick(sender, args) {
            sender.set_autoPostBack(false);
            var id = $find("<%= ddlBranch.ClientID %>").get_selectedItem().get_value();
            showSimplePopUp('PrintPreview.aspx?RDL=UnpaidContract&BranchID=' + id);
        }
    </script>
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .style1
        {
            width: 130px;
        }
        .style2
        {
            width: 4px;
        }
    </style>
</asp:Content>


