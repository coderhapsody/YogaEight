<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MasterPrompt.Master" CodeBehind="PromptPurchaseOrder.aspx.cs" Inherits="GoGym.FrontEnd.PromptPurchaseOrder" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 110px;
        }

        .auto-style4 {
            width: 4px;
        }

        .auto-style6 {
            width: 140px;
        }

        .auto-style7 {
            width: 2px;
        }

        .auto-style12 {
            width: 228px;
        }

        .col1,
        .col2,
        .col3 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 110px;
            line-height: 14px;
            float: left;
        }

        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul,
        .rcbHovered ul,
        .rcbDisabled ul {
            margin: 0;
            padding: 0;
            width: 100%;
            display: inline-block;
            list-style-type: none;
        }

        .auto-style15 {
            width: 120px;
        }

        .auto-style16 {
            width: 104px;
        }

        .auto-style17 {
            width: 123px;
        }

        .auto-style18 {
            width: 77px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="server">
    Purchase Order
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="server">
    <fieldset>
        <table class="fullwidth">
            <tr>
                <td class="auto-style2">Branch</td>
                <td class="auto-style4">:</td>
                <td>
                    <telerik:RadDropDownList ID="ddlFindBranch" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Document No.</td>
                <td class="auto-style4">:</td>
                <td>
                    <telerik:RadTextBox ID="txtFindDocumentNo" runat="server" Width="150px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Date</td>
                <td class="auto-style4">:</td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="dtpFindDateFrom"></telerik:RadDatePicker>
                    &nbsp;to&nbsp;
                            <telerik:RadDatePicker runat="server" ID="dtpFindDateTo"></telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4"></td>
                <td>
                    <telerik:RadButton runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"></telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style4">&nbsp;</td>
                <td>
                    <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <telerik:RadGrid ID="grdMaster" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" DataSourceID="sdsMaster" GridLines="None" ShowGroupPanel="True" AutoGenerateColumns="False" GroupingSettings-CaseSensitive="false" OnItemDataBound="grdMaster_ItemDataBound">
        <GroupingSettings CaseSensitive="False" />
        <ClientSettings AllowDragToGroup="True" AllowColumnsReorder="True" EnableRowHoverStyle="true">
        </ClientSettings>
        <MasterTableView DataSourceID="sdsMaster" DataKeyNames="ID">
            <Columns>
                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID" SortExpression="ID" UniqueName="ID" DataType="System.Int32" ReadOnly="True">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DocumentNo" FilterControlAltText="Filter DocumentNo column" HeaderText="DocumentNo" SortExpression="DocumentNo" UniqueName="DocumentNo">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Date" FilterControlAltText="Filter Date col\umn" HeaderText="Date" SortExpression="Date" UniqueName="Date" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Supplier" SortExpression="Name" UniqueName="Name">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EmployeeName" FilterControlAltText="Filter EmployeeName column" HeaderText="Employee" SortExpression="EmployeeName" UniqueName="EmployeeName">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column" HeaderText="Status" SortExpression="Status" UniqueName="Status">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="GrandTotal" FilterControlAltText="Filter GrandTotal column" HeaderText="GrandTotal" SortExpression="GrandTotal" UniqueName="GrandTotal" DataFormatString="{0:c}">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn AllowFiltering="false" Groupable="false" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:HyperLink ID="hypSelect" runat="server" Text="Select" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="sdsMaster" runat="server" SelectCommand="proc_GetPurchaseOrdersForReceiving" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="DateFrom" Type="DateTime" DefaultValue="1980-01-01" />
            <asp:Parameter Name="DateTo" Type="DateTime" DefaultValue="2099-12-31" />
            <asp:Parameter Name="DocumentNo" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
