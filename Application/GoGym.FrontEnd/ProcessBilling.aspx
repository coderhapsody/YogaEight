<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ProcessBilling.aspx.cs" Inherits="GoGym.FrontEnd.ProcessBilling"  StyleSheetTheme="Workspace"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 200px;
        }

        .auto-style2 {
            width: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Process Billing                
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" />
                
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Billing Type</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList ID="ddlBillingType" runat="server"/>
                
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Membership Type</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:CheckBoxList ID="cblMembershipType" runat="server" RepeatColumns="5" RepeatLayout="Table">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Customer Status</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:CheckBoxList ID="cblCustomerStatus" runat="server" RepeatColumns="5" RepeatLayout="Table">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Next Due Date</td>
            <td class="auto-style2">:</td>
            <td>From
                <telerik:RadDatePicker runat="server" ID="calFindDateFrom"></telerik:RadDatePicker>                
                &nbsp;&nbsp;&nbsp;&nbsp; To
                <telerik:RadDatePicker runat="server" ID="calFindDateTo"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />&nbsp;&nbsp;&nbsp;&nbsp; 
                    <telerik:RadButton ID="btnProcessAll" runat="server" Text="Process All" OnClick="btnProcessAll_Click" /></td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <asp:Label ID="lblStatus" runat="server" EnableViewState="False" />
    <div>
        <asp:GridView ID="gvwBilling" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="false" Width="100%">
            <Columns>
                <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Name" HeaderText="CustomerName" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="BillingTypeDescription" HeaderText="BillingType" SortExpression="BillingTypeDescription" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                <asp:BoundField DataField="NextDuesDate" HeaderText="NextDuesDate" SortExpression="NextDuesDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                <asp:BoundField DataField="StatusMembership" HeaderText="StatusMembership" SortExpression="StatusMembership" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DuesAmount" HeaderText="DuesAmount" SortExpression="DuesAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,###0.00}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkProcess" CssClass="chkProcess" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

<%--    <script>
        $(document).ready(function () {
            $(".chkProcess").each(function () {
                $(this).attr("checked", "checked");
            });
        });
    </script>--%>
</asp:Content>

