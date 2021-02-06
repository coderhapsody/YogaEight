﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ProcessAccrualInvoice.aspx.cs" Inherits="GoGym.FrontEnd.ProcessAccrualInvoice" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 150px;
        }

        .auto-style2 {
            width: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Process Accrual Invoice
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlBranch" Width="250px" />
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Invoice Month / Year</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlMonth" Width="100px" />
                &nbsp;<telerik:RadDropDownList runat="server" ID="ddlYear" Width="100px" />
            </td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style2"></td>
            <td>
                <telerik:RadButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <br />
    <telerik:RadButton ID="btnProcess" runat="server" Text="Process Accrual" OnClientClicking="ProcessConfirm" OnClick="btnProcess_Click" />
    &nbsp;&nbsp;&nbsp;
    <asp:Label runat="server" ID="lblStatus" EnableViewState="False"></asp:Label>
    <asp:GridView runat="server" ID="gvwMaster" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%" OnRowCreated="gvwMaster_RowCreated">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceDate" HeaderText="InvoiceDate" SortExpression="InvoiceDate" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" ReadOnly="True" SortExpression="CustomerName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ContractDate" HeaderText="ContractDate" SortExpression="ContractDate" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="PackageName" HeaderText="PackageName" SortExpression="PackageName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="DuesInMonth" HeaderText="DuesInMonth" SortExpression="DuesInMonth" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceAmount" HeaderText="InvoiceAmount" ReadOnly="True" SortExpression="InvoiceAmount" DataFormatString="{0:###,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetInvoicesForAccrualProcess" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="Month" Type="Int32" />
            <asp:Parameter Name="Year" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <script>
        function ProcessConfirm(sender, args) {
            args.set_cancel(!window.confirm('This process is irreversible, are you sure want to start accrual process ?'));
        }
    </script>

</asp:Content>
