<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="VoidInvoice.aspx.cs" Inherits="GoGym.FrontEnd.VoidInvoice" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Void Invoice
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">Branch
            </td>
            <td class="style2">:
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />                
            </td>
        </tr>
        <tr>
            <td class="style1">Purchase Date
            </td>
            <td class="style2">:
            </td>
            <td>From
                <telerik:RadDatePicker runat="server" ID="calDateFrom" />
                &nbsp;to
                <telerik:RadDatePicker runat="server" ID="calDateTo" />
            </td>
        </tr>
        <tr>
            <td class="style1">Customer Barcode
            </td>
            <td class="style2">:
            </td>
            <td>
                <telerik:RadTextBox ID="txtCustomerBarcode" runat="server" Width="100px" ValidationGroup="FreshMember"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">Customer Name
            </td>
            <td class="style2">:
            </td>
            <td>
                <telerik:RadTextBox ID="txtCustomerName" runat="server" Width="200px"
                    ValidationGroup="FreshMember">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>
                <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;
            </td>
            <td class="style2">&nbsp;
            </td>
            <td>
                <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="False"
                    Text="Refresh" OnClick="btnRefresh_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadButton ID="btnVoid" runat="server" EnableViewState="False"
                    OnClientClicked="VoidClick"
                    Text="Process Void" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwData" runat="server" AutoGenerateColumns="False" DataSourceID="sdsData"
        SkinID="GridViewDefaultSkin" Width="100%" OnRowDataBound="gvwData_RowDataBound"
        AllowSorting="True" AllowPaging="True" OnRowCreated="gvwData_RowCreated">
        <Columns>
            <asp:BoundField DataField="Branch" HeaderText="Branch" SortExpression="Branch" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" SortExpression="CustomerName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Date" HeaderText="Invoice Date" SortExpression="Date" DataFormatString="{0:ddd, dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceType" HeaderText="InvoiceType" ReadOnly="True"
                SortExpression="InvoiceType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                SortExpression="Total" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkVoid" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsData" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
        SelectCommand="proc_GetInvoicesForVoid" SelectCommandType="StoredProcedure" OnSelecting="sdsData_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="PurchaseDateFrom" Type="DateTime" />
            <asp:Parameter Name="PurchaseDateTo" Type="DateTime" />
            <asp:Parameter Name="CustomerBarcode" Type="String" />
            <asp:Parameter Name="CustomerName" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <telerik:RadWindow runat="server" ID="wndVoidReason" Title="Enter Void Reason" Width="500px" Height="250px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close" ReloadOnShow="True">
        <ContentTemplate>
            Please enter a comment to describe why you want to void the invoice
        <asp:TextBox ID="txtNotes" Columns="40" Rows="5" TextMode="MultiLine" runat="server"
            Style="border: 1px solid black" />
            <br />
            <br />
            <asp:CheckBox ID="chkVoidPaymentOnly" runat="server" Text="Void Payment Only, Do Not Void Invoice" />
            <br />
            <div style="text-align: center;">
                <telerik:RadButton ID="btnProcessVoid" runat="server" Text="Process" EnableViewState="false"
                    Style="border: 1px solid black;" OnClick="btnProcessVoid_Click" />
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <script language="javascript" type="text/javascript">
        function VoidClick(e) {
            e.set_autoPostBack(false);

            var processVoid = false;
            $(":checkbox").each(function () {
                if (this.checked) {
                    processVoid = true;
                    return;
                }
            });

            if (processVoid) {
                var oWnd = $telerik.findWindow("<%=wndVoidReason.ClientID%>");
                oWnd.show();
                $("#<%= txtNotes.ClientID %>").focus();
            } else
                alert('No invoices marked for void process');

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .style2 {
            width: 1px;
        }
    </style>
</asp:Content>
