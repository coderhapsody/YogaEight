<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterTimeSlot.aspx.cs" Inherits="GoGym.FrontEnd.MasterTimeSlot" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 130px;
        }

        .auto-style2 {
            width: 2px;
        }

        .auto-style3 {
            width: 110px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Classes Time Slot
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
                    <td class="auto-style1">Day of Week</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlDayOfWeek" runat="server" />                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnRefresh" runat="server" Text="Submit" OnClick="btnRefresh_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>

        </asp:View>
        <asp:View ID="View2" runat="server">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style3">Start Time</td>
                    <td class="auto-style2">:</td>
                    <td>Time Start:                        
                        <telerik:RadMaskedTextBox runat="server" ID="txtTimeStart" Mask="##:##" ValidationGroup="AddEdit" Width="40px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Time End:
                        <telerik:RadMaskedTextBox runat="server" ID="txtTimeEnd" Mask="##:##" ValidationGroup="AddEdit"  Width="40px" />                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td style="font-weight: 700">
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" EnableViewState="false" OnClick="btnSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnDelete" runat="server" EnableViewState="false" Text="Delete" OnClick="btnDelete_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" CssClass="errorMessage" EnableViewState="False"/>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:GridView ID="gvwData" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsTimeSlot" OnRowCreated="gvwData_RowCreated" AllowSorting="True">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                <asp:BoundField DataField="StartTime" HeaderText="Time" SortExpression="StartTime" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsTimeSlot" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetTimeSlot" SelectCommandType="StoredProcedure" OnSelecting="sdsTimeSlot_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="BranchID" Type="Int32" />
                                <asp:Parameter Name="DayOfWeek" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>

        </asp:View>
    </asp:MultiView>
</asp:Content>

