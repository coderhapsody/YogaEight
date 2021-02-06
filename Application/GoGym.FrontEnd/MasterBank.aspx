﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterBank.aspx.cs" Inherits="GoGym.FrontEnd.MasterBank" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Bank
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                            Text="Add New" SkinID="AddNewButton" onclick="lnbAddNew_Click"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                            Text="Delete" OnClientClick="return confirm('Delete marked row(s) ?')" 
                            SkinID="DeleteButton" onclick="lnbDelete_Click"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" 
                            Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster" 
                            AllowPaging="True" AllowSorting="True" onrowcreated="gvwMaster_RowCreated" 
                            onrowcommand="gvwMaster_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
                            SelectCommand="proc_GetAllBanks" SelectCommandType="StoredProcedure">
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">
                        Name</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <telerik:RadTextBox id="txtName" runat="server" Width="300px" MaxLength="50" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvName" runat="server" ControlToValidate="txtName" EnableViewState="false" ErrorMessage="<b>Name</b> must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                     </td>
                </tr>           
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This bank is active" ValidationGroup="AddEdit" />
                    </td>
                </tr>     
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" 
                            EnableViewState="false" onclick="btnSave_Click" ValidationGroup="AddEdit"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" 
                            EnableViewState="false"  ValidationGroup="AddEdit" CausesValidation="false"                            
                            onclick="btnCancel_Click" />
                        </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>


<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
    .style1
    {
        width: 100%;
    }
        .style2
        {
            width: 140px;
        }
        .style3
        {
            width: 1px;
        }
    </style>
</asp:Content>