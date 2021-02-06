<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterWarehouse.aspx.cs" Inherits="GoGym.FrontEnd.MasterWarehouse" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Warehouse
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
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
            <table class="fullwidth">
                <tr>
                    <td>
                        <div class="tableContainer">
                            <div class="tableRow">
                                <div class="tableCol" style="width:200px;">Branch</div>
                                <div class="tableCol" style="width:10px;">:</div>
                                <div class="tableCol"><telerik:RadDropDownList runat="server" ID="ddlFindBranch" Width="250px"/> </div>
                            </div>
                            <div class="tableRow">
                                <div class="tableCol">&nbsp;</div>
                                <div class="tableCol">&nbsp;</div>
                                <div class="tableCol">&nbsp;</div>
                            </div>
                            <div class="tableRow">
                                <div class="tableCol"></div>
                                <div class="tableCol"></div>
                                <div class="tableCol"><telerik:RadButton runat="server" ID="btnRefresh" OnClick="btnRefresh_Click" Text="Refresh" /> </div>
                            </div>
                        </div>
                        

                    </td>
                </tr>
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
                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" HeaderStyle-HorizontalAlign="Left" />                                
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
                            SelectCommand="proc_GetAllWarehouses" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="BranchID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="fieldRowHeader">
                        Branch</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlBranch" Width="250px" />
                        <asp:RequiredFieldValidator ID="rqvBranch" runat="server" ControlToValidate="ddlBranch" CssClass="errorMessage" EnableViewState="false" ErrorMessage="&lt;b&gt;Branch&lt;/b&gt; must be specified" SetFocusOnError="true" ValidationGroup="AddEdit" />
                        </td>
                </tr>           
                <tr>
                    <td class="style2">Code</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtCode" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="100px" />
                        <asp:RequiredFieldValidator ID="rqvCode" runat="server" ControlToValidate="txtCode" CssClass="errorMessage" EnableViewState="false" ErrorMessage="&lt;b&gt;Code&lt;/b&gt; must be specified" SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Name</td>
                    <td class="style3">:</td>
                    <td>                                                
                        <telerik:RadTextBox ID="txtName" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="300px" />
                        <asp:RequiredFieldValidator ID="rqvName" runat="server" ControlToValidate="txtName" CssClass="errorMessage" EnableViewState="false" ErrorMessage="&lt;b&gt;Name&lt;/b&gt; must be specified" SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This class is active" ValidationGroup="AddEdit" />
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
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
