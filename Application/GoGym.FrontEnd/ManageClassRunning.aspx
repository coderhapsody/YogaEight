<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ManageClassRunning.aspx.cs" Inherits="GoGym.FrontEnd.ManageClassRunning" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 122px;
        }

        .auto-style2 {
            width: 10px;
        }

        .auto-style3 {
            width: 120px;
        }

        .auto-style4 {
            width: 3px;
        }

        .auto-style5 {
            width: 130px;
        }

        .auto-style6 {
            width: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Class Running
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="View1" runat="server">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style1">Branch</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Date</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="calDate"></telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
                    </td>
                </tr>
            </table>

        </asp:View>
        <asp:View ID="View2" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="auto-style3">Branch</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Period</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <asp:Label ID="lblPeriode" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSelectAnotherBranchDate" runat="server" EnableViewState="False" OnClick="btnSelectAnotherBranchDate_Click" Text="Select Another Branch or Date" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvwSchedule" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsSchedule" SkinID="GridViewDefaultSkin" Width="100%" OnRowCreated="gvwSchedule_RowCreated" OnRowDataBound="gvwSchedule_RowDataBound" OnRowCommand="gvwSchedule_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="InstructorName" HeaderText="InstructorName" SortExpression="InstructorName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="DayOfWeek" HeaderText="DayOfWeek" SortExpression="DayOfWeek" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />--%>
                    <asp:BoundField DataField="TimeStart" HeaderText="TimeStart" SortExpression="TimeStart" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TimeEnd" HeaderText="TimeEnd" SortExpression="TimeEnd" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ClassRoom" HeaderText="ClassRoom" SortExpression="ClassRoom" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RunningStartWhen" HeaderText="RunningStartWhen" SortExpression="RunningStartWhen" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <%--                    <asp:BoundField DataField="RunningEndWhen" HeaderText="RunningEndWhen" SortExpression="RunningEndWhen" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="RunningInstructor" HeaderText="RunningInstructor" SortExpression="RunningInstructor" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RunningAssistant" HeaderText="RunningAssistant" SortExpression="RunningAssistant" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnParticipants" runat="server" Text="Participants" CommandName="Participants" CommandArgument='<%# Eval("ID")  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnStartStop" runat="server" Text="Start" CommandName="StartStop" CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsSchedule" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetRunningClassesInfo" SelectCommandType="StoredProcedure" OnSelecting="sdsSchedule_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="Date" Type="DateTime" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:UpdatePanel runat="server" ID="updParticipants" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="ui-accordion">
                        <tr>
                            <td class="auto-style5">Branch</td>
                            <td class="auto-style6">:</td>
                            <td>
                                <asp:Label ID="lblBranchName3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5">Period</td>
                            <td class="auto-style6">:</td>
                            <td>
                                <asp:Label ID="lblPeriod3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5">Class Name</td>
                            <td class="auto-style6">:</td>
                            <td>
                                <asp:Label ID="lblClassName3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5">Instructor</td>
                            <td class="auto-style6">:</td>
                            <td>
                                <asp:Label ID="lblInstructor3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5">Customer Barcode</td>
                            <td class="auto-style6">:</td>
                            <td>
                                <asp:HiddenField ID="hidClassRunningID" runat="server" />
                                <telerik:RadTextBox ID="txtBarcode" runat="server" />
                                &nbsp;<asp:HyperLink ID="hypLookUpCustomer" runat="server" NavigateUrl="#">Look Up</asp:HyperLink>
                                &nbsp;<telerik:RadButton ID="btnAdd" runat="server" EnableViewState="False" OnClick="btnAdd_Click" Text="Add" />
                                &nbsp;<telerik:RadButton ID="btnCopy" runat="server" EnableViewState="False" OnClick="btnCopy_Click" Text="Copy from" />
                                <asp:Button ID="btnProcessCopy" runat="server" OnClick="btnProcessCopy_Click" Text="Process Copy" Style="display: none;" />                                
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5">&nbsp;</td>
                            <td class="auto-style6">&nbsp;</td>
                            <td>
                                <asp:Label ID="lblMessage3" runat="server" EnableViewState="false" /></td>
                        </tr>
                    </table>
                    <asp:Label ID="lblTotalParticipants" runat="server" />
                    <asp:GridView ID="gvwData" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="sdsAttendance" OnRowCreated="gvwData_RowCreated" SkinID="GridViewDefaultSkin" Width="50%" OnRowCommand="gvwData_RowCommand">

                        <Columns>
                            <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" ReadOnly="True" SortExpression="CustomerName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:CheckBoxField DataField="IsAttend" HeaderText="IsAttend" SortExpression="IsAttend" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button OnClientClick="return confirm('Are you sure want to delete this participant ?')" ID="btnDeleteParticipant" runat="server" Text="Delete" CssClass="button" CommandName="DeleteParticipant" CommandArgument='<%# Eval("CustomerID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                    <asp:SqlDataSource ID="sdsAttendance" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" OnSelecting="sdsAttendance_Selecting" SelectCommand="proc_GetClassParticipants" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="ClassRunningID" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </ContentTemplate>
                <Triggers>

                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnCopy" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnProcessCopy" EventName="Click" />

                </Triggers>
            </asp:UpdatePanel>
            <br/>
            <telerik:RadButton ID="btnBack3" runat="server" EnableViewState="False" OnClick="btnBack3_Click" Text="Back to Schedule" />
        </asp:View>
        <asp:View ID="View4" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="auto-style5">Branch</td>
                    <td class="auto-style6">:</td>
                    <td>
                        <asp:Label ID="lblBranch4" runat="server"></asp:Label>
                        &nbsp;-
                        <asp:Label ID="lblPeriod4" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Class Name</td>
                    <td class="auto-style6">:</td>
                    <td>
                        <asp:Label ID="lblClassName4" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Scheduled Instructor</td>
                    <td class="auto-style6">:</td>
                    <td>
                        <asp:Label ID="lblInstructor4" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Running Instructor</td>
                    <td class="auto-style6">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlRunningInstructor" runat="server" DropDownHeight="250px" />                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Assistant</td>
                    <td class="auto-style6">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlRunningAssistant" runat="server" DropDownHeight="250px" />                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Notes</td>
                    <td class="auto-style6">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtNotes" runat="server" CssClass="textbox" Rows="4" TextMode="MultiLine" Width="400px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style6">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style6">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" EnableViewState="False" Text="Save Attendances" OnClick="btnSave_Click" />
                        &nbsp;<telerik:RadButton ID="btnBack4" runat="server" EnableViewState="False" OnClick="btnBack3_Click" Text="Back to Schedule" />
                    </td>
                </tr>
            </table>
            <asp:CheckBoxList ID="cblAttendances" runat="server" DataSourceID="sdsAttendance" DataTextField="CustomerBarcodeAndName" DataValueField="CustomerID" CellPadding="5" CellSpacing="0" RepeatColumns="3">
            </asp:CheckBoxList>
        </asp:View>
    </asp:MultiView>
</asp:Content>

