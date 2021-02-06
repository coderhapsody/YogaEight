<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterBranch.aspx.cs" Inherits="GoGym.FrontEnd.MasterBranch" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Branch
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
                            Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster"
                            AllowPaging="True" AllowSorting="True" OnRowCreated="gvwMaster_RowCreated"
                            OnRowCommand="gvwMaster_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Address" HeaderText="Address" HeaderStyle-HorizontalAlign="Left"
                                    SortExpression="Address" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" HeaderStyle-HorizontalAlign="Left" />
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
                            SelectCommand="proc_GetAllBranches" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">Name</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="300px" MaxLength="50" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvName" runat="server" ControlToValidate="txtName" EnableViewState="false" ErrorMessage="<b>Name</b> must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Code</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtCode" runat="server" Width="80px" MaxLength="5" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvCode" runat="server" ControlToValidate="txtCode" EnableViewState="false" ErrorMessage="<b>Code</b> must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Address</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress" runat="server" Width="500px" MaxLength="100" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td class="style2">Email</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="200px" MaxLength="50" ValidationGroup="AddEdit" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail" CssClass="errorMessage" EnableViewState="False"
                            ErrorMessage="&lt;b&gt;Email&lt;/b&gt; address is invalid"
                            SetFocusOnError="True"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style2">Phone</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtPhone" runat="server" Width="100px" MaxLength="20" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td class="style2">Merchant Code</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtMerchantCode" runat="server" Width="100px" MaxLength="20" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This branch is active" ValidationGroup="AddEdit" />
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
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" SingleClick="True"
                            EnableViewState="false" OnClick="btnSave_Click" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel"
                            EnableViewState="false" ValidationGroup="AddEdit" CausesValidation="false"
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


