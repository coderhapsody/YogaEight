﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="InquiryInvoice.aspx.cs" Inherits="GoGym.FrontEnd.InquiryInvoice" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Inquiry Invoice
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">
                Branch
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px"/>                
            </td>
        </tr>
        <tr>
            <td class="style1">
                Purchase Date
            </td>
            <td class="style2">
                :
            </td>
            <td>
                From
                <telerik:RadDatePicker runat="server" ID="calDateFrom"></telerik:RadDatePicker>
                &nbsp;to
                <telerik:RadDatePicker runat="server" ID="calDateTo"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Payment Date</td>
            <td class="style2">
                :</td>
            <td>From
                <telerik:RadDatePicker runat="server" ID="calPaymentDateFrom"></telerik:RadDatePicker>
                &nbsp;to
                <telerik:RadDatePicker runat="server" ID="calPaymentDateTo"></telerik:RadDatePicker>                               
        </tr>
        <tr>
            <td class="style1">
                Customer Barcode
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadTextBox ID="txtCustomerBarcode" runat="server" Width="100px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Customer Name
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadTextBox ID="txtCustomerName" runat="server" Width="200px" />
            </td>
        </tr>        
        <tr>
            <td class="style1">
                &nbsp;
            </td>
            <td class="style2">
                &nbsp;
            </td>
            <td>
                <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" EnableViewState="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwData" runat="server" AutoGenerateColumns="False" DataSourceID="sdsData"
        SkinID="GridViewDefaultSkin" Width="100%" OnRowDataBound="gvwData_RowDataBound"
        AllowSorting="True" AllowPaging="True" OnRowCreated="gvwData_RowCreated">
        <Columns>
            <asp:BoundField DataField="Branch" HeaderText="Branch" SortExpression="Branch" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" SortExpression="CustomerName"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceType" HeaderText="InvoiceType" ReadOnly="True"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="InvoiceType" />
            <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" DataFormatString="{0:###,##0.00}"
                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" SortExpression="Total" />
            <asp:BoundField DataField="PaymentStatus" HeaderText="PaymentSatus" ReadOnly="True" 
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="PaymentStatus" />
            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Void" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypViewDetail" runat="server" Text="View Detail" Style="cursor: pointer;" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsData" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
        SelectCommand="proc_InquiryInvoices" SelectCommandType="StoredProcedure" OnSelecting="sdsData_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="PurchaseDateFrom" Type="DateTime" />
            <asp:Parameter Name="PurchaseDateTo" Type="DateTime" />
            <asp:Parameter Name="CustomerBarcode" Type="String" />
            <asp:Parameter Name="CustomerName" Type="String" />
            <asp:Parameter Name="PaymentDateFrom" Type="DateTime" />
            <asp:Parameter Name="PaymentDateTo" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <div id="dialogVoid" style="display: none; width: 500px; font-size: 90%; text-align: left;"
        title="Change Status to Back to User">
        Please enter a comment to describe why you want to void the invoice
        <asp:TextBox ID="txtNotes" Columns="80" Rows="5" TextMode="MultiLine" runat="server"
            Style="border: 1px solid black" />
        <br />
        <br />
    </div>    
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
        .style2
        {
            width: 1px;
        }
    </style>
</asp:Content>
