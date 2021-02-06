<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MasterWorkspace.Master" CodeBehind="ReportFormFreeze.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportFormFreeze" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 140px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Freeze Form
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="fullwidth">
        <tr>
            <td class="auto-style1">Barcode:</td>
            <td>
                <telerik:RadTextBox runat="server" ID="txtBarcode" CssClass="textbox"></telerik:RadTextBox>
                <asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server" Text="Look Up" />
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Date:</td>
            <td>From 
                <telerik:RadDatePicker runat="server" ID="calDateFrom"/>
                To
                 <telerik:RadDatePicker runat="server" id="calDateTo" />
            </td>
        </tr>       
        <tr>
            <td class="auto-style1"></td>
            <td>
                <asp:Button runat="server" ID="btnViewForm" Text="View Form" CssClass="button" />
            </td>
        </tr>
    </table>
    
    <script>
        $(function() {
            $("#<%=btnViewForm.ClientID%>").click(function(e) {
                e.preventDefault();

                var barcode = $("#<%=txtBarcode.ClientID%>").val();

                if (barcode.length == 0) {
                    alert("Customer barcode must be specified.");
                    return;
                }

                var dateFrom = $find("<%=calDateFrom.ClientID%>").get_selectedDate();
                var dateTo = $find("<%=calDateTo.ClientID%>").get_selectedDate();
                
                showSimplePopUp('PrintPreview.aspx?RDL=FreezeForm&Barcode=' + barcode + "&FreezeDateFrom=" + dateFrom.format("yyyy-MM-dd") + "&FreezeDateTo=" + dateTo.format("yyyy-MM-dd"));
            });
        });
    </script>
</asp:Content>

