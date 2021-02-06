<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ManageAlerts.aspx.cs" Inherits="GoGym.FrontEnd.ManageAlerts" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Alerts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 180px">Filter by</td>
                                <td style="width: 1px">:
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFilter" runat="server" CssClass="dropdown">
                                        <Items>
                                            <telerik:DropDownListItem runat="server" Value="0" Text="Show All Alerts" />
                                            <telerik:DropDownListItem runat="server" Value="1" Text="Show Only Active Alerts" />
                                            <telerik:DropDownListItem runat="server" Value="2" Text="Show Only Inactive Alerts" />
                                        </Items>                                                               
                                    </telerik:RadDropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px">&nbsp;
                                </td>
                                <td style="width: 1px">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnRefresh" EnableViewState="false"
                                        runat="server" Text="Refresh" ToolTip="Show data with specified criteria"
                                        ValidationGroup="View" OnClick="btnRefresh_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" CommandArgument="AddNew" CommandName="FormCommand"
                            EnableViewState="false" SkinID="AddNewButton" OnClick="lnbAddNew_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" CommandArgument="Delete" CommandName="FormCommand" OnClientClick="return confirm('Delete marked row(s) ?');"
                            EnableViewState="false" SkinID="DeleteButton" OnClick="lnbDelete_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:HyperLink runat="server" ID="hypCalendar" Text="View Calendar" NavigateUrl="Default.aspx?FromAlert=1"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True" DataSourceID="sdsMaster" ShowGroupPanel="True" AutoGenerateColumns="False" OnItemCommand="RadGrid1_ItemCommand" GroupingSettings-CaseSensitive="false" AllowCustomPaging="True" OnPageIndexChanged="RadGrid1_PageIndexChanged" OnItemCreated="RadGrid1_ItemCreated">
                            <GroupingSettings CaseSensitive="False" />
                            <ClientSettings AllowDragToGroup="True" AllowColumnsReorder="True" EnableRowHoverStyle="true">
                            </ClientSettings>
                            <MasterTableView DataKeyNames="ID" DataSourceID="sdsMaster">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="ID" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Subject" FilterControlAltText="Filter Subject column" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>                                    
                                     <telerik:GridBoundColumn DataField="StartDate" DataType="System.DateTime" FilterControlAltText="Filter StartDate column" HeaderText="Start Date" SortExpression="StartDate" UniqueName="StartDate" DataFormatString="{0:ddd, dd-MMM-yyyy}">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                        <FilterTemplate>
                                            <telerik:RadDatePicker ID="StartDateFilterDatePicker" runat="server" Width="100px"
                                                ClientEvents-OnDateSelected="StartDateSelected" DbSelectedDate='<%# SetStartDate(Container) %>' />
                                            <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                                <script type="text/javascript">
                                                    function StartDateSelected(sender, args) {
                                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                        var date = FormatSelectedDate(sender);
                                                        tableView.filter("StartDate", date, "GreaterThanOrEqualTo");
                                                    }

                                                    function FormatSelectedDate(picker) {
                                                        var date = picker.get_selectedDate();
                                                        var dateInput = picker.get_dateInput();
                                                        var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
                                                        return formattedDate;

                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="EndDate" DataType="System.DateTime" FilterControlAltText="Filter EndDate column" HeaderText="End Date" SortExpression="EndDate" UniqueName="EndDate" DataFormatString="{0:ddd, dd-MMM-yyyy}">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                        <FilterTemplate>
                                            <telerik:RadDatePicker ID="EndDateFilterDatePicker" runat="server" Width="100px"
                                                ClientEvents-OnDateSelected="StartDateSelected" DbSelectedDate='<%# SetEndDate(Container) %>' />
                                            <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                                <script type="text/javascript">
                                                    function StartDateSelected(sender, args) {
                                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                        var date = FormatSelectedDate(sender);
                                                        tableView.filter("EndDate", date, "LessThanOrEqualTo");
                                                    }
                                                </script>
                                            </telerik:RadScriptBlock>
                                        </FilterTemplate>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridCheckBoxColumn DataField="Active" DataType="System.Boolean" FilterControlAltText="Filter Active column" HeaderText="Active" SortExpression="Active" UniqueName="Active">
                                    </telerik:GridCheckBoxColumn>
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
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            OnSelecting="sdsMaster_Selecting" SelectCommand="proc_GetAllAlert_Paged"
                            SelectCommandType="StoredProcedure" SortParameterName="OrderByClause">
                            <SelectParameters>
                                <asp:Parameter Name="PageIndex" Type="Int32" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Direction="InputOutput" Name="RecordCount" Type="Int32" />
                                <asp:Parameter Name="ShowOnlyActiveAlerts" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <asp:ValidationSummary ID="vsSummary" runat="server" EnableViewState="false" ValidationGroup="AddEdit"
                CssClass="errorMessage" ToolTip="Validation error" />
            <table style="width: 100%">
                <tr>
                    <td style="width: 160px">Subject
                    </td>
                    <td style="width: 1px">:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" runat="server" ValidationGroup="AddEdit"
                            Width="400px"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvSubject" runat="server" CssClass="errorMessage"
                            ControlToValidate="txtSubject" ErrorMessage="<strong>Subject</strong> must be specified"
                            ToolTip="Subject must be specified" Text="*" SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">Description
                    </td>
                    <td style="width: 1px">:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" ValidationGroup="AddEdit"
                            Width="400px" Columns="60" Rows="8" TextMode="MultiLine" MaxLength="2000"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Start Date
                    </td>
                    <td class="style2">:
                    </td>
                    <td class="style3">
                        <telerik:RadDatePicker runat="server" ID="CalendarPopup1" Width="100px" ShowPopupOnFocus="True"   />
                    </td>
                </tr>
                <tr id="enddate">
                    <td style="width: 160px">End Date
                    </td>
                    <td style="width: 1px">:
                    </td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="CalendarPopup2" Width="100px" ShowPopupOnFocus="True"/>                            
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">Background Color</td>
                    <td style="width: 1px">:</td>
                    <td>
                        <telerik:RadColorPicker ID="RadColorPicker1" Runat="server" RenderMode="Auto" ShowIcon="True" ShowRecentColors="True">
                        </telerik:RadColorPicker>
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">&nbsp;</td>
                    <td style="width: 1px">&nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkInfinite" runat="server" Text="Set as infinite" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">&nbsp;
                    </td>
                    <td style="width: 1px">&nbsp;
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Set as active alert" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">&nbsp;
                    </td>
                    <td style="width: 1px">&nbsp;
                    </td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" EnableViewState="false"
                            ValidationGroup="AddEdit" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" 
                            OnClientClicking="CancelConfirm" EnableViewState="false" CausesValidation="false" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    <script>
        $(document).ready(function () {
            if ($("#<%= chkInfinite.ClientID %>").is(":checked")) {
                $("#enddate").hide();
            }
            else {
                $("#enddate").show();
            }
            $("#<%= chkInfinite.ClientID %>").click(
                    function () {
                        chk = $("#<%= chkInfinite.ClientID %>");
                        if (chk.is(":checked")) {
                            $("#enddate").hide();
                        }
                        else {
                            $("#enddate").show();
                        }
                    });
        });
    </script>
</asp:Content>
