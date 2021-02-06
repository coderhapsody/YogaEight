<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterItemType.aspx.cs" Inherits="GoGym.FrontEnd.MasterItemType" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Item Type
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="False">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvwMaster">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="mvwForm" />
                    <telerik:AjaxUpdatedControl ControlID="gvwMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">

            <table class="style1">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false"
                            Text="Add New" SkinID="AddNewButton" OnClick="lnbAddNew_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false"
                            Text="Delete" OnClientClick="return confirm('Delete marked row(s) ?')"
                            SkinID="DeleteButton" OnClick="lnbDelete_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin"
                            Width="450px" AutoGenerateColumns="False" DataSourceID="sdsMaster"
                            AllowPaging="True" AllowSorting="True" OnRowCreated="gvwMaster_RowCreated"
                            OnRowCommand="gvwMaster_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-HorizontalAlign="Left" />
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
                            SelectCommand="proc_GetAllItemTypes" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">Description</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Width="300px" MaxLength="50" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvDescription" runat="server" ControlToValidate="txtDescription" EnableViewState="false" ErrorMessage="<b>Description</b> must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Type</td>
                    <td class="style3">:
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlType" ValidationGroup="AddEdit">                            
                            <Items>
                                <telerik:DropDownListItem runat="server" Text="" />
                                <telerik:DropDownListItem runat="server" Text="Material" Value="M" />
                                <telerik:DropDownListItem runat="server" Text="Service" Value="S" />
                            </Items>
                        </telerik:RadDropDownList>
                        <asp:RequiredFieldValidator ID="rqvType" runat="server" ControlToValidate="ddlType" EnableViewState="false" ErrorMessage="<b>Type</b> must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save"
                            EnableViewState="false" OnClick="btnSave_Click" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel"
                            EnableViewState="false" ValidationGroup="AddEdit" CausesValidation="false"
                            OnClientClicking="CancelConfirm"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>        
</asp:Content>


<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 140px;
        }

        .style3 {
            width: 1px;
        }
    </style>

</asp:Content>


