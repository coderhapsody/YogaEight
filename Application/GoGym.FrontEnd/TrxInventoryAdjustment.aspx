<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="TrxInventoryAdjustment.aspx.cs" Inherits="GoGym.FrontEnd.TrxInventoryAdjustment" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Inventory Adjustment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:MultiView runat="server" ID="mvwForm">
        <asp:View runat="server" ID="viwRead">
            <table>
                <tr>
                    <td style="width: 150px">Branch</td>
                    <td style="width: 5px;">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlBranch" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Date From-To</td>
                    <td>:</td>
                    <td>
                        <telerik:RadDatePicker ID="ddlDateFrom" runat="server" Width="120px" />
                        <telerik:RadDatePicker ID="ddlDateTo" runat="server" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px"></td>
                    <td></td>
                    <td>
                        <telerik:RadButton runat="server" ID="btnSearch" EnableViewState="False" Text="Search" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:LinkButton runat="server" ID="lnbAddNew" Text="Add New" OnClick="lnbAddNew_Click" SkinID="AddNewButton" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton runat="server" ID="lnbDelete" Text="Delete" OnClick="lnbDelete_Click" SkinID="DeleteButton" OnClientClick="return confirm('Delete marked row(s) ?')" />
            <br />
            <telerik:RadGrid runat="server" ID="grdMaster" CellSpacing="-1" DataSourceID="sdsMaster" GridLines="Both" GroupPanelPosition="Top">
                <MasterTableView AutoGenerateColumns="False" DataSourceID="sdsMaster">
                    <Columns>
                        <telerik:GridBoundColumn DataField="DocumentNo" FilterControlAltText="Filter DocumentNo column" HeaderText="DocumentNo" SortExpression="DocumentNo" UniqueName="DocumentNo">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InOut" FilterControlAltText="Filter InOut column" HeaderText="In/Out" SortExpression="InOut" UniqueName="InOut">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Source" FilterControlAltText="Filter Source column" HeaderText="Source" ReadOnly="True" SortExpression="Source" UniqueName="Source">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Destination" FilterControlAltText="Filter Destination column" HeaderText="Destination" ReadOnly="True" SortExpression="Destination" UniqueName="Destination">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RefDocumentNo" FilterControlAltText="Filter RefDocumentNo column" HeaderText="Ref. Document No." SortExpression="RefDocumentNo" UniqueName="RefDocumentNo">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Void" DataType="System.DateTime" FilterControlAltText="Filter Void column" HeaderText="Void" SortExpression="Void" UniqueName="Void">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Posted" DataType="System.DateTime" FilterControlAltText="Filter Posted column" HeaderText="Posted" SortExpression="Posted" UniqueName="Posted">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetInventoryMutations" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="FromDate" Type="DateTime" />
                    <asp:Parameter Name="ToDate" Type="DateTime" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View runat="server" ID="viwAddEdit">
            <div id="tabs">
                <ul>
                    <li><a href="#tab1">Header</a></li>
                    <li><a href="#tab2">Detail</a></li>
                </ul>
                <div id="tab1">
                    <div class="tableContainer">
                        <div class="tableRow">
                            <div class="tableCol fieldRowHeader">Document No:</div>
                            <div class="tableCol">
                                <telerik:RadTextBox runat="server" ID="txtDocumentNo"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="tableRow">
                            <div class="tableCol">Document Date:</div>
                            <div class="tableCol">
                                <telerik:RadDatePicker runat="server" ID="dtpDocumentDate"></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator runat="server" ID="rqvDocumentDate" ControlToValidate="dtpDocumentDate" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Document Date must be specified" SetFocusOnError="True" />
                            </div>
                        </div>

                        <div class="tableRow">
                            <div class="tableCol">Direction</div>
                            <div class="tableCol">
                                <telerik:RadDropDownList runat="server" ID="ddlDirection">
                                    <Items>
                                        <telerik:DropDownListItem runat="server" Text="" />
                                        <telerik:DropDownListItem runat="server" Text="In" Value="I" />
                                        <telerik:DropDownListItem runat="server" Text="Out" Value="Out" />
                                    </Items>
                                </telerik:RadDropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rqvDirection" ControlToValidate="ddlDirection" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Direction must be specified" SetFocusOnError="True" />
                            </div>
                        </div>

                        <div class="tableRow">
                            <div class="tableCol">Source:</div>
                            <div class="tableCol">
                                <asp:UpdatePanel runat="server" ID="updHeader1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadDropDownList runat="server" ID="ddlSource"></telerik:RadDropDownList>
                                        <telerik:RadDropDownList runat="server" ID="ddlSourceRef"></telerik:RadDropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rqvSource" Display="Dynamic" ControlToValidate="ddlSource" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Source must be specified" SetFocusOnError="True" />
                                        <asp:RequiredFieldValidator runat="server" ID="rqvSourceRef" Display="Dynamic" ControlToValidate="ddlSourceRef" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Source Reference must be specified" SetFocusOnError="True" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlSource" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>


                        <div class="tableRow">
                            <div class="tableCol">Destination:</div>
                            <div class="tableCol">
                                <asp:UpdatePanel runat="server" ID="updHeader2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadDropDownList runat="server" ID="ddlDest"></telerik:RadDropDownList>
                                        <telerik:RadDropDownList runat="server" ID="ddlDestRef"></telerik:RadDropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rqvDest" Display="Dynamic" ControlToValidate="ddlDest" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Destination must be specified" SetFocusOnError="True" />
                                        <asp:RequiredFieldValidator runat="server" ID="rqvDestRef" Display="Dynamic" ControlToValidate="ddlDestRef" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Destination Reference must be specified" SetFocusOnError="True" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlDest" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </div>

                        <div class="tableRow">
                            <div class="tableCol">Delivery Order No.:</div>
                            <div class="tableCol">
                                <telerik:RadTextBox runat="server" ID="txtDeliveryOrderNo" />
                                <asp:RequiredFieldValidator runat="server" ID="rqvDeliveryOrderNo" ControlToValidate="txtDeliveryOrderNo" ValidationGroup="AddEdit" CssClass="errorMessage" ErrorMessage="Delivery Order Number must be specified" SetFocusOnError="True" />
                            </div>
                        </div>

                        <div class="tableRow">
                            <div class="tableCol">Notes:</div>
                            <div class="tableCol">
                                <telerik:RadTextBox runat="server" ID="txtNotes" TextMode="MultiLine" Rows="5" Columns="40" />
                            </div>
                        </div>
                    </div>
                </div>

                <div id="tab2">
                    <asp:UpdatePanel runat="server" ID="updDetail" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div class="tableContainer">
                                <div class="tableCol">
                                    <div class="tableRow">
                                        <asp:ValidationSummary runat="server" ID="vsmSummaryAddDetail" EnableViewState="False" ValidationGroup="AddDetail" CssClass="errorMessage" />
                                        <table class="fullwidth">
                                            <tr>
                                                <td>Item</td>
                                                <td>Unit</td>
                                                <td>Qty</td>
                                                <td>Notes</td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadDropDownList runat="server" ID="ddlItem" ValidationGroup="AddDetail" />
                                                </td>
                                                <td>
                                                    <telerik:RadDropDownList runat="server" ID="ddlUnit" ValidationGroup="AddDetail" />
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox runat="server" ID="ntbQty" NumberFormat-DecimalDigits="0" Width="80px" ValidationGroup="AddDetail" Value="0" />
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox runat="server" ID="txtNotesDetail" Width="300px" />
                                                </td>
                                                <td>
                                                    <telerik:RadButton runat="server" ID="btnAddDetail" EnableViewState="False" ValidationGroup="AddDetail" Text="Add Detail" OnClick="btnAddDetail_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rqvItem" EnableViewState="False" CssClass="errorMessage" ControlToValidate="ddlItem" SetFocusOnError="True" Text="*" ValidationGroup="AddDetail" ErrorMessage="Item must be specified"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator runat="server" ID="rqvUnit" EnableViewState="False" CssClass="errorMessage" ControlToValidate="ddlUnit" SetFocusOnError="True" Text="*" ValidationGroup="AddDetail" ErrorMessage="Unit must be specified"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:CustomValidator runat="server" ID="cuvQty" EnableViewState="False" SetFocusOnError="True" ControlToValidate="ntbQty" ValidationGroup="AddDetail" ClientValidationFunction="QtyValidate" CssClass="errorMessage" Text="*" ErrorMessage="Qty must be greater than zero."></asp:CustomValidator></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="tableRow">
                                        <telerik:RadGrid runat="server" ID="grdDetail">
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddDetail" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdDetail" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>
            <br />
            <asp:Label ID="lblStatus" runat="server" EnableViewState="False" />
            <br/>
            <telerik:RadButton runat="server" ID="btnSave" Text="Save" ValidationGroup="AddEdit" EnableViewState="False" OnClick="btnSave_Click" />
            &nbsp;&nbsp;&nbsp;
            <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="AddEdit" CausesValidation="False" EnableViewState="False" OnClick="btnCancel_Click" />
            <script>
                $(function () {
                    $("#tabs").tabs();
                });

                function QtyValidate(sender, args) {
                    args.IsValid = args.Value > 0;
                }
            </script>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
