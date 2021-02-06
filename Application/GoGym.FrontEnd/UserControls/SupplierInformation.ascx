<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierInformation.ascx.cs" Inherits="GoGym.FrontEnd.UserControls.SupplierInformation" %>
<B>Supplier Information</B>
<asp:DetailsView ID="dtvSupplier" runat="server" Height="50px" Width="100%" AutoGenerateRows="False" DataSourceID="odsSupplier" CellPadding="4" ForeColor="#333333" GridLines="None">    
    <AlternatingRowStyle BackColor="White" />
    <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
    <EditRowStyle BackColor="#2461BF" />
    <FieldHeaderStyle BackColor="#cd116f" ForeColor="white" Font-Bold="True" Width="200px" />
    <Fields>        
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
        <asp:BoundField DataField="NPWP" HeaderText="NPWP" SortExpression="NPWP" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
        <asp:BoundField DataField="Phone1" HeaderText="Phone1" SortExpression="Phone1" />
        <asp:BoundField DataField="Phone2" HeaderText="Phone2" SortExpression="Phone2" />
    </Fields>
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#FDE0FF" />
</asp:DetailsView>
<asp:ObjectDataSource ID="odsSupplier" runat="server" SelectMethod="GetSupplier" TypeName="GoGym.Providers.SupplierProvider" OnObjectCreating="odsSupplier_ObjectCreating" OnSelecting="odsSupplier_Selecting">
    <SelectParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

