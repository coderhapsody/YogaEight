﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterInstructor.aspx.cs" Inherits="GoGym.FrontEnd.MasterInstructor" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 111px;
        }

        .auto-style2 {
            width: 2px;
        }

        .auto-style3 {
            width: 120px;
        }

        .auto-style4 {
            width: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Instructor
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1">Name</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtFindName" runat="server" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="false" Text="Refresh" /></td>
                </tr>
            </table>
            <br />
            <asp:LinkButton ID="lnbAddNew" runat="server" Text="Add New" EnableViewState="false" OnClick="lnbAddNew_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnbDelete" runat="server" Text="Delete" EnableViewState="false" OnClick="lnbDelete_Click" />
            <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sqldsMaster" OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />                    
                    <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="HireDate" HeaderText="HireDate" SortExpression="HireDate" DataFormatString="{0:ddd, dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />                    
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="HomePhone" HeaderText="HomePhone" SortExpression="HomePhone" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CellPhone" HeaderText="CellPhone" SortExpression="CellPhone" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" ItemStyle-HorizontalAlign="Left"  HeaderStyle-HorizontalAlign="Left" />
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
            <asp:SqlDataSource ID="sqldsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetAllInstructors" SelectCommandType="StoredProcedure" OnSelecting="sqldsMaster_Selecting">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtFindName" ConvertEmptyStringToNull="False" Name="Name" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <asp:ValidationSummary ID="vsmSummary" runat="server" EnableViewState="false" CssClass="errorMessage" />
            <table class="ui-accordion">
                <tr>
                    <td class="auto-style3">Barcode</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtBarcode" runat="server" Width="100px" />
                        <asp:RequiredFieldValidator ID="rqvName0" runat="server" ControlToValidate="txtBarcode" CssClass="errorMessage" EnableViewState="false" ErrorMessage="&lt;b&gt;Barcode&lt;/b&gt; must be specified" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Name</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="200px" />
                        <asp:RequiredFieldValidator ID="rqvName" runat="server" ControlToValidate="txtName" CssClass="errorMessage" EnableViewState="false" ErrorMessage="&lt;b&gt;Name&lt;/b&gt; must be specified" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Hire Date</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="calDate"></telerik:RadDatePicker>
                        <asp:RequiredFieldValidator ID="rqvDate" runat="server" ControlToValidate="calDate" CssClass="errorMessage" EnableViewState="false" ErrorMessage="&lt;b&gt;Hire Date&lt;/b&gt; must be specified" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Status</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlStatus" runat="server">
                            <Items>
                                <telerik:DropDownListItem runat="server" Selected="True" />
                                <telerik:DropDownListItem runat="server" Value="C" Text="Contract" />
                                <telerik:DropDownListItem runat="server" Value="P" Text="Permanent" />
                            </Items>
                        </telerik:RadDropDownList>
                        <asp:RequiredFieldValidator ID="rqvStatus" runat="server" CssClass="errorMessage" ErrorMessage="<b>Status</b> must be specified" ControlToValidate="ddlStatus" SetFocusOnError="true"  EnableViewState="false" Text="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Email Address</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="150px" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" Text="*" ErrorMessage="<b>Email Address</b> is invalid" EnableViewState="false" SetFocusOnError="true" CssClass="errorMessage" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Home Phone</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtHomePhone" runat="server" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Cellular Phone</td>
                    <td class="auto-style4">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtCellPhone" runat="server" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This instructor is active" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td><asp:Label ID="lblMessage" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" EnableViewState="false" Text="Save" OnClick="btnSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" EnableViewState="false" Text="Cancel" OnClientClicking="CancelConfirm" OnClick="btnCancel_Click" />
                    </td>
                </tr>

            </table>

        </asp:View>
    </asp:MultiView>
</asp:Content>

