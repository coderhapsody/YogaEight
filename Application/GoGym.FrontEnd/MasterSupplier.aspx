<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterSupplier.aspx.cs" Inherits="GoGym.FrontEnd.MasterSupplier" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Supplier
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="False">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdMaster">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="mvwForm" />
                    <telerik:AjaxUpdatedControl ControlID="grdMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>
<%--            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:MultiView runat="server" ID="mvwForm">
        <asp:View runat="server" ID="viwRead">
            <table class="fullwidth">
                <tr>
                    <td class="auto-style2">Name</td>
                    <td class="auto-style5">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFindName"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2"></td>
                    <td class="auto-style5"></td>
                    <td>
                        <telerik:RadButton runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"></telerik:RadButton>
                    </td>
                </tr>
            </table>
            <br />
            <asp:LinkButton runat="server" ID="lnbAddNew" Text="Add New" OnClick="lnbAddNew_Click" SkinID="AddNewButton" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton runat="server" ID="lnbDelete" Text="Delete" OnClick="lnbDelete_Click" SkinID="DeleteButton" OnClientClick="return confirm('Delete marked row(s) ?')" />
            &nbsp;<asp:Label runat="server" ID="lblStatus" EnableViewState="False"></asp:Label>
            <telerik:RadGrid ID="grdMaster" runat="server" AllowPaging="True" AllowSorting="True" DataSourceID="sdsMaster" AutoGenerateColumns="False" GroupingSettings-CaseSensitive="false" OnItemCommand="grdMaster_ItemCommand" GroupPanelPosition="Top">
                <GroupingSettings CaseSensitive="False" />
                <ClientSettings AllowColumnsReorder="True" EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView DataSourceID="sdsMaster" DataKeyNames="ID">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID" SortExpression="ID" UniqueName="ID" DataType="System.Int32" ReadOnly="True">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Name" SortExpression="Name" UniqueName="Name">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Address" FilterControlAltText="Filter Address column" HeaderText="Address" SortExpression="Address" UniqueName="Address">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NPWP" FilterControlAltText="Filter NPWP column" HeaderText="NPWP" SortExpression="NPWP" UniqueName="NPWP">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phone1" FilterControlAltText="Filter Phone1 column" HeaderText="Phone1" SortExpression="Phone1" UniqueName="Phone1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phone2" FilterControlAltText="Filter Phone2 column" HeaderText="Phone2" SortExpression="Phone2" UniqueName="Phone2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Groupable="false" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Groupable="false" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" data-value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="sdsMaster" runat="server" SelectCommand="proc_GetAllSuppliers" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>">
                <SelectParameters>
                    <asp:Parameter Name="Name" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View runat="server" ID="viwAddEdit">
            <asp:Label runat="server" ID="lblStatusAddEdit" EnableViewState="False"></asp:Label>
            <table class="fullwidth">
                <tr>
                    <td class="auto-style6">Name </td>
                    <td class="auto-style7">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" ValidationGroup="AddEdit" Width="200px"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvName" runat="server" ControlToValidate="txtName" CssClass="errorMessage" Display="Dynamic" EnableViewState="False" ErrorMessage="&lt;b&gt;Supplier Name&lt;/b&gt; must be specified" SetFocusOnError="True" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">Address</td>
                    <td class="auto-style7">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAddress" ValidationGroup="AddEdit" Width="500px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">NPWP</td>
                    <td class="auto-style7">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtNPWP" runat="server" ValidationGroup="AddEdit" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">Phone 1</td>
                    <td class="auto-style7">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPhone1" ValidationGroup="AddEdit" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">Phone 2</td>
                    <td class="auto-style7">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtPhone2" runat="server" ValidationGroup="AddEdit" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">Email</td>
                    <td class="auto-style7">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmail" ValidationGroup="AddEdit" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkTaxable" runat="server" Text="Taxable" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" EnableViewState="False" OnClick="btnSave_Click" Text="Save" ValidationGroup="AddEdit">
                        </telerik:RadButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <telerik:RadButton ID="btnCancel" runat="server" CausesValidation="False" EnableViewState="False" OnClick="btnCancel_Click" Text="Cancel" OnClientClicking="CancelConfirm">
                                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 110px;
        }

        .auto-style5 {
            width: 5px;
        }

        .auto-style6 {
            width: 120px;
        }

        .auto-style7 {
            width: 3px;
        }
    </style>

</asp:Content>
