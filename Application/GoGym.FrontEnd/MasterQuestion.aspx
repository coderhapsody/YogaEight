<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterQuestion.aspx.cs" Inherits="GoGym.FrontEnd.MasterQuestion" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Questions
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
                                <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Seq" HeaderText="Seq" SortExpression="Seq" HeaderStyle-HorizontalAlign="Left" />
                                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" HeaderStyle-HorizontalAlign="Left" />
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
                            SelectCommand="proc_GetAllQuestions" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2" style="width: 90px">Question</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Width="300px" MaxLength="200" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvDescription" EnableClientScript="true" runat="server" ControlToValidate="txtDescription" EnableViewState="false" ErrorMessage="&lt;b&gt;Question&lt;/b&gt; must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="width: 90px">Sequence</td>
                    <td class="style3">:</td>
                    <td><telerik:RadNumericTextBox runat="server" ID="txtSeq" ShowSpinButtons="true" Value="1" NumberFormat-DecimalDigits="0" MaxValue="99" MinValue="1" Width="80px"></telerik:RadNumericTextBox> 
                        <asp:RequiredFieldValidator ID="rqvDescription0" runat="server" ControlToValidate="txtSeq" CssClass="errorMessage" EnableClientScript="true" EnableViewState="false" ErrorMessage="&lt;b&gt;Sequence&lt;/b&gt; must be specified" SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="width: 90px">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td><asp:CheckBox runat="server" id="chkIsActive" Text="Active" /> </td>
                </tr> 
                <tr>
                    <td class="style2" style="width: 90px">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2" style="width: 90px">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" EnableViewState="false" OnClick="btnSave_Click" SingleClick="true" Text="Save" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" CausesValidation="false" EnableViewState="false" OnClick="btnCancel_Click" Text="Cancel" ValidationGroup="AddEdit" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    
</asp:Content>
