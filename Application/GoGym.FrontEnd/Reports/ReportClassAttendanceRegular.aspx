<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ReportClassAttendanceRegular.aspx.cs" Inherits="GoGym.FrontEnd.Reports.ReportClassAttendanceRegular"  StyleSheetTheme="Workspace"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 90px;
        }

        .auto-style2 {
            width: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Class Attendance Regular
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table >
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px"/>                
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
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    &nbsp;<asp:SqlDataSource ID="sdsSchedule" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetRunningClassesInfo" SelectCommandType="StoredProcedure" OnSelecting="sdsSchedule_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="Date" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="gvwSchedule" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsSchedule" SkinID="GridViewDefaultSkin" Width="100%" OnRowCreated="gvwSchedule_RowCreated" OnRowDataBound="gvwSchedule_RowDataBound">
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
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypViewReport" runat="server" Text="View Report" NavigateUrl="#" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

