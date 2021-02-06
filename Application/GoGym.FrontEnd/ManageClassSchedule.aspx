<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ManageClassSchedule.aspx.cs" Inherits="GoGym.FrontEnd.ManageClassSchedule" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 120px;
        }

        .auto-style2 {
            width: 2px;
        }

        .auto-style3 {
            width: 1px;
        }

        .auto-style4 {
            width: 130px;
        }

        .auto-style5 {
            width: 130px;
            height: 17px;
        }

        .auto-style6 {
            width: 2px;
            height: 17px;
        }

        .auto-style7 {
            height: 17px;
        }

        .auto-style10 {
            width: 119px;
        }

        .auto-style11 {
            width: 120px;
            font-weight: bold;
        }

        .auto-style12 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Class Schedule
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead1" runat="server">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style1">Branch</td>
                    <td class="auto-style3">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px"/>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Year</td>
                    <td class="auto-style3">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlYear" runat="server" Width="100px"/>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Month</td>
                    <td class="auto-style3">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlMonth" runat="server" Width="100px"/>
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnRefresh" runat="server" Text="Refresh" EnableViewState="False" OnClick="btnRefresh_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton EnableViewState="False" runat="server" id="btnToggleUpload" AutoPostBack="False" OnClientClicking="UploadToggleClick" Text="Upload via Excel"/>
                        <br />
                        <asp:Label ID="lblUploadError" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <div id="upload" style="display: none;">
                            <asp:FileUpload ID="fupFile" runat="server" />
                            <telerik:RadButton ID="btnUpload" runat="server" EnableViewState="False" Text="Upload" OnClick="btnUpload_Click" />
                        </div>
                    </td>
                </tr>
            </table>

        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="auto-style4">Branch</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Period</td>
                    <td class="auto-style6">:</td>
                    <td class="auto-style7">
                        <asp:Label ID="lblPeriod" runat="server"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnSelectOtherPeriod" runat="server" EnableViewState="False" OnClick="btnSelectOtherPeriod_Click" Text="Select Other Period" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Day Of Week</td>
                    <td class="auto-style6">:</td>
                    <td class="auto-style7">
                        <telerik:RadDropDownList ID="ddlDayOfWeek" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDayOfWeek_SelectedIndexChanged"/>
                        
                        &nbsp;
                        <telerik:RadButton ID="btnRefreshDayOfWeek" runat="server" EnableViewState="False" OnClick="btnRefreshDayOfWeek_Click" Text="Refresh" />
                        &nbsp;
                        <telerik:RadButton ID="btnCopyFromLastMonth" runat="server" EnableViewState="False" Text="Copy From Last Month" OnClick="btnCopyFromLastMonth_Click" OnClientClicking="CopyFromLastMonthClick"  />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <table style="width:100%;">
                            <tr>
                                <td class="auto-style11"><b>Class</b></td>
                                <td class="auto-style1"><b>Level</b></td>
                                <td class="auto-style1"><b>Room</b></td>
                                <td class="auto-style1"><b>Time Start</b></td>
                                <td class="auto-style10"><b>Instructor</b></td>
                                <td></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style1">
                                    <telerik:RadDropDownList ID="ddlClass" runat="server" />                                    
                                </td>
                                <td class="auto-style1">
                                    <telerik:RadDropDownList ID="ddlLevel" runat="server" >
                                        <Items>
                                            <telerik:DropDownListItem Selected="True" Text="Medium" Value="Medium"/>
                                            </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td class="auto-style1">
                                    <telerik:RadDropDownList ID="ddlClassRoom" runat="server" />
                                    
                                </td>
                                <td class="auto-style1">
                                    <telerik:RadDropDownList ID="ddlTimeStart" runat="server"  />
                                </td>
                                <td class="auto-style10">
                                    <telerik:RadDropDownList ID="ddlInstructor" runat="server" />
                                    
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnSave" runat="server" EnableViewState="False" OnClick="btnSave_Click" Text="Save" ValidationGroup="TimeStartEnd" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblStatusDelete" runat="server" EnableViewState="false" />
            <asp:GridView ID="gvwDetail" runat="server" SkinID="GridViewDefaultSkin" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" OnRowCreated="gvwDetail_RowCreated" OnRowCommand="gvwDetail_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ClassName" HeaderText="ClassName" SortExpression="ClassName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ClassRoomName" HeaderText="ClassRoomName" SortExpression="ClassRoomName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="InstructorName" HeaderText="InstructorName" SortExpression="InstructorName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="TimeStart" HeaderText="TimeStart" SortExpression="TimeStart" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="TimeEnd" HeaderText="TimeEnd" SortExpression="TimeEnd" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnbEdit" runat="server" Text="Edit" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnbDelete" runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Delete current row ?')" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetClassScheduleInfo" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="Year" Type="Int32" />
                    <asp:Parameter Name="Month" Type="Int32" />
                    <asp:Parameter Name="DayOfWeek" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>

        </asp:View>
        <asp:View ID="viwEdit" runat="server">

            <table class="auto-style12">
                <tr>
                    <td class="auto-style1">Branch</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblBranch0" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Period</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblPeriod0" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Day of Week</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblDayOfWeek0" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Class</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlClass0" runat="server" Width="200px" DropDownHeight="250px" />
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Level</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlLevel0" runat="server" >
                            <Items>
                                <telerik:DropDownListItem runat="server" Text="Medium" Value="Medium" Selected="True"/>
                            </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Room</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlClassRoom0" runat="server" />
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Time Start</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlTimeStart0" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Instructor</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlInstructor0" runat="server" DropDownHeight="250px" />
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnUpdate" runat="server" EnableViewState="False" Text="Update" ValidationGroup="Update" OnClick="btnUpdate_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancelUpdate" runat="server" EnableViewState="False" Text="Cancel" ValidationGroup="Update" OnClick="btnCancelUpdate_Click" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblStatusUpdate" runat="server" EnableViewState="false" />
        </asp:View>
    </asp:MultiView>

    <script>
        function UploadToggleClick(sender, args) {
            sender.set_autoPostBack(false);            
            $("#upload").toggle();
        }

        function CopyFromLastMonthClick(sender, args) {
            //sender.set_autoPostBack(false);
            args.set_cancel(!window.confirm('Copying schedule from last period will replace any schedule in current selected period. Are you sure ?'));            
        }
    </script>
</asp:Content>

