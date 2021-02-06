<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="PromptClassRunning.aspx.cs" Inherits="GoGym.FrontEnd.PromptClassRunning" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100px;
        }

        .auto-style2 {
            width: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Select Class Running        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
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
    <asp:GridView ID="gvwData" runat="server" AutoGenerateColumns="False" DataKeyNames="ClassRunningID" DataSourceID="sdsPrompt" OnRowCreated="gvwData_RowCreated" SkinID="GridViewDefaultSkin" Width="100%" OnRowDataBound="gvwData_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ClassRunningID" HeaderText="ClassRunningID" InsertVisible="False" ReadOnly="True" SortExpression="ClassRunningID" />
            <asp:BoundField DataField="ClassName" HeaderText="ClassName" SortExpression="ClassName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InstructorName" HeaderText="InstructorName" SortExpression="InstructorName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TimeStart" HeaderText="TimeStart" SortExpression="TimeStart" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TimeEnd" HeaderText="TimeEnd" SortExpression="TimeEnd" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypSelect" runat="server" Text="Select" NavigateUrl="#" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsPrompt" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_PromptClassRunning" SelectCommandType="StoredProcedure" OnSelecting="sdsPrompt_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="Date" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

