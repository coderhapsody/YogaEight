<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="TrxReceiving.aspx.cs" Inherits="GoGym.FrontEnd.TrxReceiving" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Inventory Receiving
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:MultiView runat="server" ID="mvwForm">
        <asp:View ID="viwRead" runat="server">
            <fieldset>
                <table class="fullwidth">
                    <tr>
                        <td class="auto-style2">Branch</td>
                        <td class="auto-style4">:</td>
                        <td>
                            <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px" />
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
            <asp:LinkButton runat="server" ID="lnbAddNew" Text="Add New" OnClick="lnbAddNew_Click" SkinID="AddNewButton" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton runat="server" ID="lnbDelete" Text="Delete" OnClick="lnbDelete_Click" SkinID="DeleteButton" OnClientClick="return confirm('Delete marked row(s) ?')" Visible="False" />
            <telerik:RadGrid ID="grdMaster" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" DataSourceID="sdsMaster" GridLines="None" ShowGroupPanel="True" AutoGenerateColumns="False" GroupingSettings-CaseSensitive="false" OnItemCommand="grdMaster_ItemCommand">
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
                        <telerik:GridBoundColumn DataField="DocumentNo" FilterControlAltText="Filter DocumentNo column" HeaderText="Document No" SortExpression="DocumentNo" UniqueName="DocumentNo">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="poDocNo" FilterControlAltText="Filter poDocNo column" HeaderText="PO Document No" SortExpression="poDocNo" UniqueName="poDocNo">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="GoodIssueNo" FilterControlAltText="Filter GoodIssueNo column" HeaderText="GoodIssueNo" SortExpression="GoodIssueNo" UniqueName="GoodIssueNo">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Date" FilterControlAltText="Filter Date column" HeaderText="Date" SortExpression="Date" UniqueName="Date" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
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
                        <telerik:GridTemplateColumn AllowFiltering="false" Groupable="false" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Groupable="false" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" data-value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="sdsMaster" runat="server" SelectCommand="proc_GetReceivings" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="DateFrom" Type="DateTime" DefaultValue="1980-01-01" />
                    <asp:Parameter Name="DateTo" Type="DateTime" DefaultValue="2099-12-31" />
                    <asp:Parameter Name="DocumentNo" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <asp:UpdatePanel ID="uppDetail" runat="server">
                <ContentTemplate>
                    <div id="detail">
                        <table class="fullwidth">
                            <tr>
                                <td style="width: 50%;">
                                    <table class="fullwidth">
                                        <tr>
                                            <td class="auto-style6">Branch</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <asp:Label ID="lblBranch" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style6">Document No.</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtDocumentNo" runat="server" Width="150px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style6">Receiving Date</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadDatePicker ID="receivingDate" runat="server">
                                                </telerik:RadDatePicker>
                                                &nbsp;<asp:RequiredFieldValidator ID="rqvReceivingDate" runat="server" ControlToValidate="receivingDate" CssClass="errorMessage" ErrorMessage="Date must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style6">Purchase Order</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtDocumentPO" runat="server" ReadOnly="true" ClientEvents-OnValueChanged="doPostBack">
                                                </telerik:RadTextBox>
                                                <asp:HyperLink runat="server" ID="hypLookUpPO" ImageUrl="Images/zoom.png" ToolTip="Look up Purchase Order"></asp:HyperLink>
                                                <asp:HiddenField runat="server" ID="hidPOID" />
                                                <telerik:RadButton ID="radBtnRefreshPO" runat="server" Text="Refresh Grid" Visible="false" OnClick="radBtnRefreshPO_Click"></telerik:RadButton>
                                                &nbsp;<asp:RequiredFieldValidator ID="rqvPO" runat="server" ControlToValidate="txtDocumentPO" CssClass="errorMessage" ErrorMessage="Purchase Order must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="auto-style6">Good Issue No.</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtGoodIssueNo" runat="server" Width="150px">
                                                </telerik:RadTextBox>
                                                &nbsp;<asp:RequiredFieldValidator ID="rqvGoodIssueNo" runat="server" ControlToValidate="txtGoodIssueNo" CssClass="errorMessage" ErrorMessage="Good Issue No must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style6">Warehouse</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadDropDownList runat="server" ID="ddlWarehouse"></telerik:RadDropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style6">Freight Info</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtFreightInfo" runat="server" Columns="50" Rows="3" TextMode="MultiLine" Width="400px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style6">Notes</td>
                                            <td class="auto-style7">:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtNotes" runat="server" Columns="50" Rows="3" TextMode="MultiLine" Width="400px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="text-align: right;">
                        <a href="#" id="toggleDetailArea">Toggle Detail Area</a>
                    </div>



                    <telerik:RadGrid runat="server" ID="grdDetail" CellSpacing="0" GridLines="None">
                        <MasterTableView AutoGenerateColumns="False">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="ID" DataField="ID" HeaderText="ID">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="ItemID" SortExpression="ItemID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyPO" runat="server" Text='<%# Eval("ItemID") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode" HeaderText="ItemCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="ItemName" DataField="ItemName" HeaderText="ItemName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="QtyPO" DataField="QtyPO" HeaderText="QtyPO" DataFormatString="{0:##,##0}">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="QtyReceived" SortExpression="QtyReceived">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox Width="60px" ID="txtQtyReceived" runat="server" MinValue="0"
                                            Skin="Web20" DbValue='<%# Bind("QtyReceived")%>' Type="Number">
                                            <NumberFormat DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn UniqueName="UnitName" DataField="UnitName" HeaderText="UnitName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Notes" SortExpression="Notes">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtNotes" runat="server" Columns="50" Rows="2" Text='<%# Bind("Notes") %>' TextMode="MultiLine" Width="200px">
                                        </telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>

                    <asp:Label ID="lblStatusAddEdit" runat="server" EnableViewState="False" />
                    <asp:ValidationSummary runat="server" ID="vsmSummaryAddEdit" ShowMessageBox="true" ShowSummary="False" DisplayMode="List" ValidationGroup="AddEdit" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />

            <telerik:RadButton runat="server" ID="btnSave" EnableViewState="false" Text="Save" OnClick="btnSave_Click" ValidationGroup="AddEdit"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnCancel" EnableViewState="false" CausesValidation="False" Text="Back" OnClick="btnCancel_Click" ValidationGroup="AddEdit"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnVoid" AutoPostBack="false" EnableViewState="false" CausesValidation="False" Text="Void" ValidationGroup="AddEdit" OnClientClicking="btnVoidClick"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnPrint" EnableViewState="false" CausesValidation="False" Text="Print" ValidationGroup="AddEdit" Visible="false"></telerik:RadButton>

            <script>
                function btnVoidClick() {
                    $find("<%= wndVoidReason.ClientID %>").show();
                }

                function doPostBack() {
                    var uniqueID = "<%= radBtnRefreshPO.UniqueID%>";
                    __doPostBack(uniqueID, '');
                }

                function ModalPopUpShow() {
                    $find("<%= txtVoidReason.ClientID %>").focus();
                }

                $(function () {
                    $("#toggleDetailArea").click(function () {
                        $("#detail").toggle("slow");
                    });
                });
            </script>

            <telerik:RadWindow runat="server" ID="wndVoidReason" Title="Enter Void Reason" Width="400px" Height="200px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close" OnClientShow="ModalPopUpShow" ReloadOnShow="True">
                <ContentTemplate>
                    <br />
                    <br />
                    <telerik:RadTextBox runat="server" ID="txtVoidReason" TextMode="MultiLine" Width="300px" Rows="3"></telerik:RadTextBox>
                    <br />
                    <br />
                    <div style="text-align: center;">
                        <telerik:RadButton runat="server" ID="btnProcessVoid" Text="Process Void" OnClick="btnProcessVoid_Click"></telerik:RadButton>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </asp:View>
    </asp:MultiView>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
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
    </style>
</asp:Content>
