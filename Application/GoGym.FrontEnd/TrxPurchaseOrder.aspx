<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="TrxPurchaseOrder.aspx.cs" Inherits="GoGym.FrontEnd.TrxPurchaseOrder" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Purchase Order&nbsp; <asp:Label id="lblPONo" runat="server" />
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
                        <telerik:GridBoundColumn DataField="DocumentNo" FilterControlAltText="Filter DocumentNo column" HeaderText="DocumentNo" SortExpression="DocumentNo" UniqueName="DocumentNo">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentDate" FilterControlAltText="Filter Date col\umn" HeaderText="Date" SortExpression="Date" UniqueName="Date" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy}">
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
                        <telerik:GridBoundColumn DataField="StatusReceiving" FilterControlAltText="Filter StatusReceiving column" HeaderText="StatusReceiving" SortExpression="StatusReceiving" UniqueName="StatusReceiving">
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
            <asp:SqlDataSource ID="sdsMaster" runat="server" SelectCommand="proc_GetPurchaseOrders" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="DateFrom" Type="DateTime" DefaultValue="1980-01-01" />
                    <asp:Parameter Name="DateTo" Type="DateTime" DefaultValue="2099-12-31" />
                    <asp:Parameter Name="DocumentNo" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
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
                                    <td class="auto-style6">Document Date</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <telerik:RadDatePicker ID="dtpDate" runat="server">
                                        </telerik:RadDatePicker>
                                        &nbsp;<asp:RequiredFieldValidator ID="rqvSupplier2" runat="server" ControlToValidate="dtpDate" CssClass="errorMessage" ErrorMessage="Date must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Delivery Date</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <telerik:RadDatePicker ID="dtpExpectedDate" runat="server">
                                        </telerik:RadDatePicker>
                                        &nbsp;<asp:RequiredFieldValidator ID="rqvExpectedDate" runat="server" ControlToValidate="dtpExpectedDate" CssClass="errorMessage" ErrorMessage="Expected Date must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Supplier</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <telerik:RadComboBox runat="server" ID="cboSupplier" Width="350px" HighlightTemplatedItems="true" MarkFirstMatch="true" OnItemsRequested="cboSupplier_ItemsRequested" EnableVirtualScrolling="true" ShowDropDownOnTextboxClick="False" AutoPostBack="True" OnSelectedIndexChanged="cboSupplier_SelectedIndexChanged">
                                            <HeaderTemplate>
                                                <ul>
                                                    <li class="col1" style="display: none;">ID</li>
                                                    <li class="col2">Name</li>
                                                    <li class="col3">Email</li>
                                                </ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ul>
                                                    <li class="col1" style="display: none;">
                                                        <%# DataBinder.Eval(Container.DataItem, "ID") %></li>
                                                    <li class="col1">
                                                        <%# DataBinder.Eval(Container.DataItem, "Name") %></li>
                                                    <li class="col2">
                                                        <%# DataBinder.Eval(Container.DataItem, "Email") %></li>
                                                </ul>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rqvSupplier" runat="server" ControlToValidate="cboSupplier" CssClass="errorMessage" ErrorMessage="Supplier must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Terms</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <telerik:RadDropDownList ID="ddlTerms" runat="server" DefaultMessage="Select Terms">
                                           <%-- <Items>
                                                <telerik:DropDownListItem runat="server" Selected="True" Text="COD" Value="COD" />
                                            </Items>--%>
                                        </telerik:RadDropDownList>
                                        <asp:RequiredFieldValidator ID="rqvTerms" runat="server" ControlToValidate="ddlTerms" CssClass="errorMessage" ErrorMessage="Terms must be specified" SetFocusOnError="True" ValidationGroup="AddEdit">*</asp:RequiredFieldValidator>
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
                                <tr>
                                    <td class="auto-style6">Discount Value</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="ntbDiscValue" runat="server" Width="150px">
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 50%; text-align: left; vertical-align: top;">
                            <asp:UpdatePanel runat="server" ID="uppSupplierInformation" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <gogym:SupplierInformation ID="SupplierInformation" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cboSupplier" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <br/>
                            Status PO: <asp:Label ID="lblPOStatus" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align:right;">
            <a href="#" id="toggleDetailArea">Toggle Detail Area</a>
                </div>
                <%--<telerik:RadAjaxPanel runat="server" ID="ajpDetail">--%>
            <asp:UpdatePanel ID="uppDetail" runat="server">
                <ContentTemplate>


                    <table class="fullwidth">
                        <tr style="font-weight: bold;">
                            <td>Item</td>
                            <td>Quantity</td>
                            <td class="auto-style16">Unit</td>
                            <td class="auto-style17">Unit Price</td>
                            <td class="auto-style18">Disc. (%)</td>
                            <td>&nbsp;</td>
                            <td></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style12">
                                <telerik:RadComboBox runat="server" ID="cboItem" AutoPostBack="true" EnableLoadOnDemand="true" Width="300px" ShowDropDownOnTextboxClick="false" MarkFirstMatch="true" HighlightTemplatedItems="True" EnableVirtualScrolling="true" OnItemsRequested="cboItem_ItemsRequested" OnSelectedIndexChanged="cboItem_SelectedIndexChanged" MaxHeight="200px">
                                    <HeaderTemplate>
                                        <ul>
                                            <li class="col1">Code</li>
                                            <li class="col2">Name</li>
                                            <%--<li class="col3">IsTaxed</li>--%>
                                        </ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <ul>
                                            <li class="col1">
                                                <%# DataBinder.Eval(Container.DataItem, "Barcode") %></li>
                                            <li class="col2">
                                                <%# DataBinder.Eval(Container.DataItem, "Description") %></li>
                                            <%--                                        <li class="col3">
                                            <%# DataBinder.Eval(Container.DataItem, "IsTaxed") %></li>--%>
                                        </ul>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </td>
                            <td class="auto-style15">
                                <telerik:RadNumericTextBox ID="ntbQty" runat="server" Width="100px" DataType="System.Int32" ShowSpinButtons="True" MinValue="1">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat AllowRounding="False" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td class="auto-style16">
                                <telerik:RadDropDownList runat="server" ID="ddlUnit" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged" OnClientItemSelecting="test"></telerik:RadDropDownList>
                            </td>
                            <td class="auto-style17">
                                <telerik:RadNumericTextBox ID="ntbUnitPrice" runat="server" Width="120px">
                                </telerik:RadNumericTextBox>
                            </td>
                            <td class="auto-style18">
                                <telerik:RadNumericTextBox ID="ntbDisc" runat="server" Width="50px">
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadButton ID="btnAddDetail" runat="server" OnClick="btnAddDetail_Click" Text="Add Detail" ValidationGroup="AddDetail">
                                </telerik:RadButton>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RequiredFieldValidator ID="rqvSupplier0" runat="server" ControlToValidate="cboItem" CssClass="errorMessage" ErrorMessage="Item must be specified" SetFocusOnError="True" ValidationGroup="AddDetail" Display="None">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:RangeValidator ID="ravQty" runat="server" ControlToValidate="ntbQty" CssClass="errorMessage" EnableViewState="False" ErrorMessage="Quantity must be greater than zero" MaximumValue="999999" MinimumValue="1" SetFocusOnError="True" Display="None" ValidationGroup="AddDetail">*</asp:RangeValidator>
                            </td>
                            <td class="auto-style16">
                                <asp:RequiredFieldValidator ID="rqvUnit" runat="server" ControlToValidate="ddlUnit" CssClass="errorMessage" ErrorMessage="Unit must be specified" SetFocusOnError="True" ValidationGroup="AddDetail" Display="None">*</asp:RequiredFieldValidator>
                            </td>
                            <td class="auto-style17">&nbsp;</td>
                            <td class="auto-style18">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <telerik:RadGrid runat="server" ID="grdDetail" CellSpacing="0" GridLines="None" OnItemCommand="grdDetail_ItemCommand">
                        <MasterTableView AutoGenerateColumns="False">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="ID" DataField="ID" HeaderText="ID">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
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
                                <telerik:GridBoundColumn UniqueName="Quantity" DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:###,##0}">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="UnitName" DataField="UnitName" HeaderText="UnitName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="UnitPrice" DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:c}">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="DiscountRate" DataField="DiscountRate" HeaderText="DiscountRate">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn UniqueName="IsTaxed" DataField="IsTaxed" HeaderText="Tax" />
                                <telerik:GridBoundColumn UniqueName="Total" DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <telerik:RadButton ID="btnDeleteRow" runat="server" CommandName="DeleteDetail" CommandArgument='<%# Eval("ID") %>' Text="Delete" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:Label ID="lblStatusAddEdit" runat="server" EnableViewState="False" />
                    <asp:ValidationSummary runat="server" ID="vsmSummaryAddDetail" ShowMessageBox="true" ShowSummary="False" DisplayMode="List" ValidationGroup="AddDetail" />
                    <asp:ValidationSummary runat="server" ID="vsmSummaryAddEdit" ShowMessageBox="true" ShowSummary="False" DisplayMode="List" ValidationGroup="AddEdit" />
                    <%--</telerik:RadAjaxPanel>--%>
                    <br />
                    Total Before Tax:
            <asp:Label ID="lblTotalBeforeTax" runat="server" Text="Total Before Tax" Font-Bold="True" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total Tax:
            <asp:Label ID="lblTaxAmouont" runat="server" Text="Total Tax" Font-Bold="True" />
                    &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; Total After Tax:
            <asp:Label ID="lblTotalAfterTax" runat="server" Text="Total After Tax" Font-Bold="True" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <br />
            <br />
            <telerik:RadButton runat="server" ID="btnSave" EnableViewState="false" Text="Save" OnClick="btnSave_Click" ValidationGroup="AddEdit" SingleClick="True" SingleClickText="Saving"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnCancel" EnableViewState="false" CausesValidation="False" Text="Back" OnClick="btnCancel_Click" ValidationGroup="AddEdit"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnVoid" AutoPostBack="false" EnableViewState="false" CausesValidation="False" Text="Void" ValidationGroup="AddEdit" OnClientClicking="btnVoidClick" ></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnPrint" EnableViewState="false" CausesValidation="False" Text="Print" ValidationGroup="AddEdit" OnClientClicked="btnPrint_Click" ></telerik:RadButton>
            <script>
                function test() {

                }


                function btnVoidClick() {
                    $find("<%= wndVoidReason.ClientID %>").show();
                }

                function ModalPopUpShow() {
                    $find("<%= txtVoidReason.ClientID %>").focus();
                        }

                function btnPrint_Click() {
                    var letterNo = $telerik.findControl(document.forms[0], '<%= txtDocumentNo.ClientID %>');
                    showSimplePopUp('ReportPreview.aspx?ReportName=SlipPurchaseOrder&DocumentNo=' + letterNo.get_value());
                    return false;
                }

                $(function () {
                    $("#toggleDetailArea").click(function () {
                        $("#detail").toggle("slow");
                    });

                    
                });
            </script>
            
             <telerik:RadWindow runat="server" ID="wndVoidReason" Title="Enter Void Reason" Width="400px" Height="200px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close" OnClientShow="ModalPopUpShow" ReloadOnShow="True">
                <ContentTemplate>
                    <br/><br/>
                    <telerik:RadTextBox runat="server" ID="txtVoidReason" TextMode="MultiLine" Width="300px" Rows="3"></telerik:RadTextBox>
                    <br />                    
                    <br/>
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
