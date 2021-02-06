<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ManageSalesTarget.aspx.cs" Inherits="GoGym.FrontEnd.ManageSalesTarget"  StyleSheetTheme="Workspace"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 135px;
        }

        .auto-style2 {
            width: 2px;
        }

        .auto-style3 {
            width: 178px;
        }

        .auto-style4 {
            width: 4px;
        }

        .auto-style5 {
            width: 178px;
            height: 21px;
        }

        .auto-style6 {
            width: 4px;
            height: 21px;
        }

        .auto-style7 {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Sales Target
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View runat="server" ID="viwRead">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style1">Branch</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px"/>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Year</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlFindYear" runat="server" Width="100px"/>
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" /></td>
                </tr>
                
            </table>
            <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                    onclick="lnbAddNew_Click" SkinID="AddNewButton" Text="Add New" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                    onclick="lnbDelete_Click" 
                    OnClientClick="return confirm('Delete marked row(s) ?')" SkinID="DeleteButton" 
                    Text="Delete" />
            <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" AllowPaging="false" AllowSorting="true" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" OnRowCommand="gvwMaster_RowCommand" OnRowCreated="gvwMaster_RowCreated">

                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />                    
                    <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetSalesTarget" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="Year" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View runat="server" ID="viwAddEdit">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style3">Branch</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px"/>
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Month / Year</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadMonthYearPicker runat="server" ID="mypPeriode" />                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">Fresh Member Unit</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtFreshMemberUnit"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Renewal Unit</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtRenewalUnit"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Upgrade Unit</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtUpgradeUnit"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">Fresh Member Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtFreshMemberRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Renewal Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtRenewalRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Upgrade Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtUpgradeRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">Pilates Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtPilatesRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Vocal Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtVocalRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">EFT Collection Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtEFTCollectionRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style5">Drop Off Unit</td>
                    <td class="auto-style6">:</td>
                    <td class="auto-style7">
                        <telerik:RadNumericTextBox runat="server" ID="txtDropOffUnit"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Cancel Fees</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtCancelFees"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Freeze Unit</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtFreezeUnit"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Freeze Fees</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtFreezeFees"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">Other Revenue</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtOtherRevenue"></telerik:RadNumericTextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                        &nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" OnClientClicking="CancelConfirm" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

