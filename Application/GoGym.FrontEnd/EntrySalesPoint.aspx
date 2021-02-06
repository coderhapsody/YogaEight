<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="EntrySalesPoint.aspx.cs" Inherits="GoGym.FrontEnd.EntrySalesPoint" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Sales Point
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

    <table class="fullwidth">
        <tr>
            <td class="auto-style4">Contract</td>
            <td class="auto-style5">:</td>
            <td>
                <asp:Label ID="lblContractNo" runat="server" /></td>
        </tr>
        <tr>
            <td class="auto-style4">Customer</td>
            <td class="auto-style5">:</td>
            <td>
                <asp:Label ID="lblCustomer" runat="server" /></td>
        </tr>
        <tr>
            <td class="auto-style4">Contract Date</td>
            <td class="auto-style5">:</td>
            <td>
                <asp:Label ID="lblContractDate" runat="server" /></td>
        </tr>
        <tr>
            <td class="auto-style4">Package</td>
            <td class="auto-style5">:</td>
            <td>
                <asp:Label ID="lblPackage" runat="server" /></td>
        </tr>
    </table>
    <br />
    <br />
    <asp:UpdatePanel runat="server" ID="updSales" UpdateMode="Conditional">
        <ContentTemplate>

            <table class="fullwidth">
                <tr>
                    <td class="auto-style1">
                        <b>Sales</b></td>
                    <td class="auto-style3">
                        <b>Points</b></td>
                    <td class="auto-style2">
                        <b>Notes</b></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <telerik:RadDropDownList runat="server" ID="ddlSales" DropDownHeight="200px"></telerik:RadDropDownList>
                    </td>
                    <td class="auto-style3">
                        <telerik:RadNumericTextBox runat="server" ID="txtPointAmount" Width="70px"></telerik:RadNumericTextBox>
                    </td>
                    <td class="auto-style2">
                        <telerik:RadTextBox runat="server" ID="txtNotes" Width="300px"></telerik:RadTextBox>
                        <td>
                            <telerik:RadButton runat="server" ID="btnAddSales" Text="Add Sales" OnClick="btnAddSales_Click"></telerik:RadButton>
                        </td>
                        <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><asp:RequiredFieldValidator runat="server" ID="rqvSales" EnableViewState="False" CssClass="errorMessage" ControlToValidate="ddlSales" Text="*" ErrorMessage="Sales must be selected" SetFocusOnError="True"/> </td>
                    <td class="auto-style3"><asp:RequiredFieldValidator runat="server" ID="rqvPointAmount" EnableViewState="False" CssClass="errorMessage" ControlToValidate="txtPointAmount" Text="*" ErrorMessage="Point amount must be given" SetFocusOnError="True"/></td>
                    <td class="auto-style2">&nbsp;<td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </td>
                </tr>
            </table>
            <asp:GridView runat="server" ID="gvwSales" SkinID="GridViewDefaultSkin" Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster" OnRowCreated="gvwSales_RowCreated" AllowPaging="True" AllowSorting="True" OnRowCommand="gvwSales_RowCommand">
                <Columns>
                    <asp:BoundField DataField="SalesID" HeaderText="SalesID" SortExpression="SalesID" />
                    <asp:BoundField DataField="SalesBarcode" HeaderText="SalesBarcode" SortExpression="SalesBarcode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SalesName" HeaderText="SalesName" ReadOnly="True" SortExpression="SalesName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PointAmount" HeaderText="PointAmount" SortExpression="PointAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CreatedWhen" HeaderText="CreatedWhen" SortExpression="CreatedWhen" DataFormatString="{0:dd-MMM-yyyy HH:mm}" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CreatedWho" HeaderText="CreatedWho" SortExpression="CreatedWho" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <telerik:RadButton runat="server" ID="btnDelete" CommandName="DeleteRow" CausesValidation="False" CommandArgument='<%# Eval("SalesID") %>' Text="Delete" OnClientClicking="DeleteConfirm" />                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br/>
            <asp:Label ID="lblStatus" runat="server" EnableViewState="False" />
            <br />
            <div style="text-align: center;">
                Total Points:
                <asp:Label ID="lblTotalPoints" runat="server" />
            </div>

            <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetSalesPointByContract" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:QueryStringParameter Name="ContractID" QueryStringField="ContractID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAddSales" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="gvwSales" EventName="PageIndexChanging" />
            <asp:AsyncPostBackTrigger ControlID="gvwSales" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 200px;
        }

        .auto-style2 {
            width: 175px;
        }

        .auto-style3 {
            width: 90px;
        }

        .auto-style4 {
            width: 190px;
        }

        .auto-style5 {
            width: 4px;
        }
    </style>
</asp:Content>
