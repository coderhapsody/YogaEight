<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterItem.aspx.cs" Inherits="GoGym.FrontEnd.MasterItem" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Item
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManagerProxy ID="scmScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/CheckInService.svc" />
        </Services>
    </asp:ScriptManagerProxy>
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="style4">Barcode</td>
                                <td class="style5">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindBarcode" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">Description</td>
                                <td class="style5">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindDescription" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">Item Type</td>
                                <td class="style5">:</td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFindItemType" runat="server" />

                                </td>
                            </tr>
                            <tr>
                                <td class="style4">&nbsp;</td>
                                <td class="style5">&nbsp;</td>
                                <td>
                                    <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="false" OnClick="btnRefresh_Click" Text="Refresh" ValidationGroup="AddEdit" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">&nbsp;</td>
                                <td class="style5">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false"
                            OnClick="lnbAddNew_Click" SkinID="AddNewButton" Text="Add New" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false"
                            OnClick="lnbDelete_Click"
                            OnClientClick="return confirm('Delete marked row(s) ?')" SkinID="DeleteButton"
                            Text="Delete" />
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
                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Account" HeaderText="Account" SortExpression="Account" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" SortExpression="UnitPrice" DataFormatString="{0:###,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:CheckBoxField DataField="IsTaxed" HeaderText="IsTaxed" SortExpression="IsTaxed" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
                            SelectCommand="proc_GetAllItems" SelectCommandType="StoredProcedure"
                            OnSelecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="Barcode" Type="String" />
                                <asp:Parameter Name="Description" Type="String" />
                                <asp:Parameter Name="ItemTypeID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">Barcode</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtBarcode" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="100px" />
                        <asp:RequiredFieldValidator ID="rqvBarcode" runat="server"
                            ControlToValidate="txtBarcode" CssClass="errorMessage" EnableViewState="false"
                            ErrorMessage="&lt;b&gt;Barcode&lt;/b&gt; must be specified"
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Description</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="300px" />
                        <asp:RequiredFieldValidator ID="rqvDescription" runat="server"
                            ControlToValidate="txtDescription" CssClass="errorMessage"
                            EnableViewState="false"
                            ErrorMessage="&lt;b&gt;Description&lt;/b&gt; must be specified"
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Account</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlAccount" runat="server" DropDownHeight="200px" Width="300px" />
                        <asp:RequiredFieldValidator ID="rqvAccount" runat="server"
                            ControlToValidate="ddlAccount" CssClass="errorMessage" EnableViewState="false"
                            ErrorMessage="&lt;b&gt;Account&lt;/b&gt; must be specified"
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Item Type</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlItemType" runat="server" OnClientSelectedIndexChanged="ddlItemTypeSelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rqvItemType" runat="server"
                            ControlToValidate="ddlItemType" CssClass="errorMessage" EnableViewState="false"
                            ErrorMessage="&lt;b&gt;Item Type&lt;/b&gt; must be specified"
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr id="sessionsRow">
                    <td class="style2"></td>
                    <td class="style3"></td>
                    <td><asp:CheckBox runat="server" ID="chkHasSessions" Text="Has Sessions"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span id="sessions">Session Balance: <telerik:RadNumericTextBox runat="server" ID="txtSessionBalance" MinValue="0" Width="100px" ShowSpinButtons="true" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox></span></td>
                </tr>
                <tr>
                    <td class="style2">Standard Unit Price</td>
                    <td class="style3">:</td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtUnitPrice" MaxLength="50" ValidationGroup="AddEdit"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="rqvUnitPrice" runat="server"
                            ControlToValidate="txtUnitPrice" CssClass="errorMessage"
                            EnableViewState="false"
                            ErrorMessage="&lt;b&gt;Standard Unit Price&lt;/b&gt; must be specified"
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                    <td class="style3">&nbsp;</td>
                    <td>
                        <table class="auto-style1">
                            <tr>
                                <td class="auto-style2">Unit Name 1 :</td>
                                <td class="auto-style7">
                                    <telerik:RadTextBox ID="txtUnitName1" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="60px" />
                                </td>
                                <td class="auto-style6">&nbsp;</td>
                                <td class="auto-style4">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Unit Name 2 :</td>
                                <td class="auto-style7">
                                    <telerik:RadTextBox ID="txtUnitName2" runat="server" MaxLength="10" ValidationGroup="AddEdit" Width="60px" />
                                </td>
                                <td class="auto-style6">&nbsp;</td>
                                <td class="auto-style4">Unit Factor 2 :</td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txtUnitFactor2" runat="server" NumberFormat-DecimalDigits="0" ValidationGroup="AddEdit" MinValue="1" ShowSpinButtons="True" Width="60px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Unit Name 3 :</td>
                                <td class="auto-style7">
                                    <telerik:RadTextBox ID="txtUnitName3" runat="server" MaxLength="10" ValidationGroup="AddEdit" Width="60px" />
                                </td>
                                <td class="auto-style6">&nbsp;</td>
                                <td class="auto-style4">Unit Factor 3 :</td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txtUnitFactor3" runat="server" NumberFormat-DecimalDigits="0" ValidationGroup="AddEdit" MinValue="1" ShowSpinButtons="True" Width="60px" />
                                </td>
                            </tr>
                        </table>
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
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This item is active" />
                        <br />
                        <asp:CheckBox ID="chkIsTaxed" runat="server" Text="Is Taxed" />
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
    
    <script>
        $(function() {
            ddlItemTypeSelectedIndexChanged(null, null);

            $("#<%=chkHasSessions.ClientID%>").change(function() {
                if ($("#<%=chkHasSessions.ClientID%>").is(":checked")) {
                    $("#sessions").show();
                } else {
                    $("#sessions").hide();
                }
            });
        });

        function ddlItemTypeSelectedIndexChanged(sender, args) {
            var checkInService = new AjaxService.CheckInService();
            var ddlItemType = $find("<%=ddlItemType.ClientID%>");
            if (ddlItemType != null) {
                var itemTypeID = ddlItemType.get_selectedItem().get_value();
                $("#sessionsRow").hide();
                $("#<%=chkHasSessions.ClientID%>").prop('checked', false);
                checkInService.ItemTypeIsService(itemTypeID, function (itemTypeIsService) {
                    if (itemTypeIsService) {
                        $("#sessionsRow").show();
                        if ($find("<%=txtSessionBalance.ClientID%>").get_value() > 0) {
                            $("#<%=chkHasSessions.ClientID%>").prop('checked', true);                            
                        }
                    }

                    if ($("#<%=chkHasSessions.ClientID%>").is(":checked")) {
                        $("#sessions").show();
                    } else {
                        $("#sessions").hide();
                    }
                });
            }
        }
    </script>
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

        .style4 {
            width: 110px;
        }

        .style5 {
            width: 2px;
        }

        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 95px;
        }

        .auto-style3 {
            width: 170px;
        }

        .auto-style4 {
            width: 90px;
        }

        .auto-style5 {
            width: 50px;
        }
        .auto-style6 {
            width: 20px;
        }
        .auto-style7 {
            width: 100px;
        }
    </style>
</asp:Content>

