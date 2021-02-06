<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="GoGym.FrontEnd.ManageUsers" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Manage Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
        <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="View1" runat="server">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 200px">
                                    Roles</td>
                                <td style="width: 1px">
                                    :</td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFindRoles" CssClass="dropdown" runat="server" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    &nbsp;</td>
                                <td style="width: 1px">
                                    &nbsp;</td>
                                <td>
                                    <telerik:RadButton ID="btnRefresh" runat="server" Text="Refresh" 
                                         ToolTip="Show data with specified criteria" OnClick="btnRefresh_Click"
                                        />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" CssClass="errorMessage" 
                            EnableViewState="False"></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" CausesValidation="False" 
                            EnableViewState="False" 
                            SkinID="AddNewButton" OnClick="lnbAddNew_Click"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Delete marked row(s) ?')"
                            EnableViewState="False" 
                            SkinID="DeleteButton" OnClick="lnbDelete_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" 
                            Width="100%"  AllowSorting="True"
                            AutoGenerateColumns="False" 
                            onrowcommand="gvwMaster_RowCommand" onrowcreated="gvwMaster_RowCreated" AllowPaging="True" DataSourceID="sdsMaster">
                            <Columns>
                                <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="UserName" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="RoleName" SortExpression="RoleName" HeaderText="RoleName" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreatedWhen" SortExpression="CreatedWhen" HeaderText="CreatedWhen" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="HomeBranch" SortExpression="HomeBranch" HeaderText="HomeBranch" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="IsActive" SortExpression="IsActive" HeaderText="IsActive" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbResetPassword" runat="server" CommandName="ResetPassword" CommandArgument='<%# Eval("UserName") %>' Text="Reset Password" ToolTip="Reset password for this user" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/ManageFormAccess.aspx?UserName={0}" DataNavigateUrlFields="UserName" Text="User Access" Visible="false" />
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("UserName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                .: No Data :.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" OnSelecting="sdsMaster_Selecting" SelectCommand="proc_GetUsersByRole" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="RoleID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 200px">
                        User Name</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadTextBox ID="txtUserName" runat="server" ValidationGroup="AddEdit"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvUserName" runat="server" 
                            CssClass="errorMessage" ControlToValidate="txtUserName" SetFocusOnError="true" 
                            EnableViewState="false" 
                            ErrorMessage="<strong>User Name</strong> is required" 
                            ToolTip="User Name is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>                
                <tr>
                    <td style="width: 200px">
                        Role Name</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlRole" runat="server"  ValidationGroup="AddEdit">
                        </telerik:RadDropDownList>
                        <asp:RequiredFieldValidator ID="rqvRole" runat="server" CssClass="errorMessage" 
                            ControlToValidate="ddlRole" SetFocusOnError="true" EnableViewState="false" 
                            ErrorMessage="<strong>Role</strong> is required" 
                            ToolTip="Role is required" Display="Dynamic" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Home Branch</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlHomeBranch" runat="server" Width="250px" 
                            ValidationGroup="AddEdit">
                        </telerik:RadDropDownList>
                        <asp:RequiredFieldValidator ID="rqvHomeBranch" runat="server" 
                            ControlToValidate="ddlHomeBranch" CssClass="errorMessage" Display="Dynamic" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;strong&gt;Home Branch&lt;/strong&gt; is required" 
                            SetFocusOnError="true" ToolTip="Home Branch is required" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Barcode</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadTextBox ID="txtBarcode" runat="server" 
                            ValidationGroup="AddEdit"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvBarcode" runat="server" 
                            ControlToValidate="txtBarcode" CssClass="errorMessage" Display="Dynamic" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;strong&gt;Barcode&lt;/strong&gt; is required" 
                            SetFocusOnError="true" ToolTip="Barcode is required" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Email</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail" runat="server" ValidationGroup="AddEdit" Width="250px"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvEmail" runat="server" 
                            CssClass="errorMessage" ControlToValidate="txtEmail" SetFocusOnError="true" 
                            EnableViewState="false" ErrorMessage="<strong>Email</strong> is required" 
                            ToolTip="Email is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                            ControlToValidate="txtEmail" CssClass="errorMessage" Display="Dynamic" 
                            EnableViewState="False" 
                            ErrorMessage="&lt;strong&gt;Email&lt;/strong&gt; is invalid" 
                            SetFocusOnError="True" ToolTip="Email is invalid" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Password</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadTextBox ID="txtPassword" runat="server" ValidationGroup="AddEdit" TextMode="Password"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvPassword" CssClass="errorMessage" 
                            runat="server" ControlToValidate="txtPassword" SetFocusOnError="true" 
                            EnableViewState="false" 
                            ErrorMessage="<strong>Password</strong> is required" 
                            ToolTip="Password is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                        <asp:CustomValidator ID="cuvPassword" runat="server" ControlToValidate="txtPassword"  ErrorMessage="Minimum <b>password</b> length is 6 characters" CssClass="errorMessage" OnServerValidate="cuvPassword_ServerValidate" ValidationGroup="AddEdit" ></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Confirm New Password</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <telerik:RadTextBox ID="txtConfirmPassword" runat="server" ValidationGroup="AddEdit" TextMode="Password"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rqvConfirmPassword" runat="server" 
                            CssClass="errorMessage" ControlToValidate="txtConfirmPassword" 
                            SetFocusOnError="true" EnableViewState="false" 
                            ErrorMessage="<strong>Confirm Password</strong> is required" 
                            ToolTip="Confirm Password is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="txtPassword"
                                                    ControlToValidate="txtConfirmPassword" CssClass="errorMessage" Display="Dynamic"
                                                    SetFocusOnError="true" ErrorMessage="The <strong>Confirm New Password</strong> must match the <strong>New Password</strong> entry."
                                                    ForeColor="Red" ValidationGroup="AddEdit"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">&nbsp;</td>
                    <td style="width: 1px">&nbsp;</td>
                    <td><asp:CheckBox runat="server" ID="chkIsActive" Text="Active"/></td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        &nbsp;</td>
                    <td style="width: 1px">
                        &nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" EnableViewState="false"  Text="Save"                             
                            ValidationGroup="AddEdit"  
                            OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server"  Text="Cancel"
                            OnClientClicking="CancelConfirm" 
                            EnableViewState="false" CausesValidation="false" OnClick="btnCancel_Click" 
                             />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        &nbsp;</td>
                    <td style="width: 1px">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View3" runat="server">

            <table style="width: 100%">
                <tr>
                    <td style="width: 200px">
                        <img alt="" src="images/Security_Warning.png" />
                    </td>
                    <td>
                        <h3>Warning</h3>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    This option will reset the password for user: <asp:Label ID="lblResetUserName" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    After password reset, user must log in with default password and change it to 
                                    desired one.</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RAdButton ID="btnDoReset" runat="server" 
                                        Text="Reset" EnableViewState="False" OnClick="btnDoReset_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <telerik:RadButton ID="btnCancelReset" runat="server" 
                                         Text="Cancel" EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNewPassword" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
