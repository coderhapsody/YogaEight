<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="CheckInHistory.aspx.cs" Inherits="GoGym.FrontEnd.CheckInHistory" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>

<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cphMainTitle">
    Check In History        
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster" AllowPaging="true" AllowSorting="true">
        <Columns>
            <asp:BoundField DataField="CheckInWhen" HeaderText="CheckInWhen" SortExpression="CheckInWhen" DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm:ss}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Messages" SortExpression="Messages" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%# String.Join("<br/>", Convert.ToString(Eval("Messages")).Split('|')) %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetCheckInHistory" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="CustomerCode" QueryStringField="Barcode" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
