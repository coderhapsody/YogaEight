<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ApprovalPurchaseOrder.aspx.cs" Inherits="GoGym.FrontEnd.ApprovalPurchaseOrder"  StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Approval Purchase Order &nbsp;<asp:Label ID="lblPONo" runat="server" />
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
                                <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="sdsMaster" runat="server" SelectCommand="proc_GetPurchaseOrdersForApproval" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>">
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
                                        <asp:Label ID="txtDocumentNo" runat="server" Width="150px">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Document Date</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Expected Date</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <asp:Label ID="lblExpectedDate" runat="server">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Supplier</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <asp:Label ID="lblSupplierName" runat="server"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Terms</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <asp:Label ID="lblTerms" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Notes</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNotes" runat="server" Columns="50" Rows="3" Enabled="false" TextMode="MultiLine" Width="400px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6">Discount Value</td>
                                    <td class="auto-style7">:</td>
                                    <td>
                                        <asp:Label ID="lblDiscValue" runat="server" Width="150px">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 50%; text-align: left; vertical-align: top;">
                            <asp:UpdatePanel runat="server" ID="uppSupplierInformation" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <gogym:SupplierInformation ID="SupplierInformation" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right;">
                <a href="#" id="toggleDetailArea">Toggle Detail Area</a>
            </div>
            <asp:UpdatePanel ID="uppDetail" runat="server">
                <ContentTemplate>

                    <telerik:RadGrid runat="server" ID="grdDetail" CellSpacing="0" GridLines="None">
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
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:Label ID="lblStatusAddEdit" runat="server" EnableViewState="False" />
                    <asp:ValidationSummary runat="server" ID="vsmSummaryAddDetail" ShowMessageBox="true" ShowSummary="False" DisplayMode="List" ValidationGroup="AddDetail" />
                    <asp:ValidationSummary runat="server" ID="vsmSummaryAddEdit" ShowMessageBox="true" ShowSummary="False" DisplayMode="List" ValidationGroup="AddEdit" />
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
            <telerik:RadButton runat="server" ID="btnApprove" AutoPostBack="false" EnableViewState="false" CausesValidation="False" Text="Approve" OnClientClicking="btnApproveClick"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
                   <telerik:RadButton runat="server" ID="btnVoid" AutoPostBack="false" EnableViewState="false" CausesValidation="False" Text="Not Approve" OnClientClicking="btnVoidClick" OnClick="btnVoid_Click"></telerik:RadButton>
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnCancel" AutoPostBack="false" EnableViewState="false" CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click" ></telerik:RadButton>

            <script>

                function btnApproveClick() {
                    $find("<%= wndApproveReason.ClientID %>").show();
                }

                function ModalPopUpShow() {
                    $find("<%= txtApproveReason.ClientID %>").focus();
                }

                function btnVoidClick() {
                    $find("<%= wndVoidReason.ClientID %>").show();
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
            <telerik:RadWindow runat="server" ID="wndApproveReason" Title="Enter Approval Reason" Width="400px" Height="200px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close" OnClientShow="ModalPopUpShow" ReloadOnShow="True">
                <ContentTemplate>
                    <br />
                    <br />
                    <telerik:RadTextBox runat="server" ID="txtApproveReason" TextMode="MultiLine" Width="300px" Rows="3"></telerik:RadTextBox>
                    <br />
                    <br />
                    <div style="text-align: center;">
                        <telerik:RadButton runat="server" ID="btnProcessApprove" Text="Process Approve" OnClick="btnProcessApprove_Click"></telerik:RadButton>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="wndVoidReason" Title="Enter Approval Reason" Width="400px" Height="200px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close" OnClientShow="ModalPopUpShow" ReloadOnShow="True">
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
