<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ExistingMember.aspx.cs" Inherits="GoGym.FrontEnd.ExistingMember" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Non-Membership Invoicing
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="style1">
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">Branch
                        </td>
                        <td class="style3">:
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlBranch" runat="server" Width="250px" ValidationGroup="ExistingMember" ClientIDMode="Static" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">Date
                        </td>
                        <td class="style3">:
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="calDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Customer</td>
                        <td class="style5">:
                        </td>
                        <td class="style6">
                            <telerik:RadTextBox ID="txtCustomerCode" runat="server" 
                                                Width="120px" ValidationGroup="ExistingMember"/>
                            <asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server">Look Up</asp:HyperLink>
                            &nbsp;<asp:RequiredFieldValidator ID="rqvCustomerCode" runat="server" ControlToValidate="txtCustomerCode"
                                CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Customer&lt;/b&gt; must be specified"
                                SetFocusOnError="True" ValidationGroup="ExistingMember"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Sales
                        </td>
                        <td class="style5">:
                        </td>
                        <td class="style6">
                            <telerik:RadDropDownList ID="ddlSales" runat="server" ValidationGroup="ExistingMember" />
                            <asp:RequiredFieldValidator ID="rqvSales" runat="server" ControlToValidate="ddlSales"
                                CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Sales&lt;/b&gt; must be specified"
                                SetFocusOnError="True" ValidationGroup="ExistingMember"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" valign="top">Notes
                        </td>
                        <td class="style3" valign="top">:&nbsp;
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNotes" runat="server" Width="400px" Rows="5"
                                TextMode="MultiLine" ValidationGroup="ExistingMember">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;&nbsp;                
            </td>
        </tr>
        <tr>
            <td>
                <div id="otherpurchase">
                    <asp:UpdatePanel ID="pnlOtherPurchase" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr style="font-weight: bold;">
                                    <td class="style23">Item Type:
                                    </td>
                                    <td class="style24">Item:
                                    </td>
                                    <td class="style21">Quantity:
                                    </td>
                                    <td class="style21">Unit:</td>
                                    <td class="style18">Unit Price:
                                    </td>
                                    <td class="style19">Discount:
                                    </td>
                                    <td class="style20">&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        <telerik:RadDropDownList ID="ddlItemType" runat="server" OnSelectedIndexChanged="ddlItemType_SelectedIndexChanged" AutoPostBack="True"  DropDownHeight="150px">
                                        </telerik:RadDropDownList>
                                        <%--<AjaxToolkit:CascadingDropDown ID="ddlItemType_CascadingDropDown" PromptText="Select item type"
                                            Category="ItemType" ServicePath="AjaxWebService.asmx" ServiceMethod="GetItemTypes"
                                            runat="server" Enabled="True" TargetControlID="ddlItemType">
                                        </AjaxToolkit:CascadingDropDown>--%>
                                    </td>
                                    <td class="style24">
                                        <telerik:RadDropDownList ID="ddlItem" runat="server" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged"  DropDownHeight="150px">
                                        </telerik:RadDropDownList>
                                        <%--<AjaxToolkit:CascadingDropDown ID="ddlItem_CascadingDropDown" Category="Item" ParentControlID="ddlItemType"
                                            PromptText="Select item type first" LoadingText="Loading, please wait" ServicePath="AjaxWebService.asmx"
                                            ServiceMethod="GetItemsByType" runat="server" Enabled="True" TargetControlID="ddlItem">
                                        </AjaxToolkit:CascadingDropDown>--%>
                                    </td>
                                    <td class="style21">
                                        <telerik:RadNumericTextBox ID="txtQuantity" runat="server" Width="50px" />
                                    </td>
                                    <td class="style21">
                                        <telerik:RadDropDownList ID="ddlUnit" runat="server" Width="80px">
                                        </telerik:RadDropDownList>
                                    </td>
                                    <td class="style18">
                                        <telerik:RadNumericTextBox ID="txtUnitPrice" runat="server" Width="120px" />
                                    </td>
                                    <td class="style19">
                                        <telerik:RadNumericTextBox ID="txtDiscount" runat="server" Width="50px" />
                                    </td>
                                    <td class="style20">
                                        <asp:CheckBox ID="chkIsTaxable" runat="server" Text="Taxable" Checked="True" />
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="btnAddDetail" runat="server" Text="Add Detail" OnClick="btnAddDetail_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvwOtherPurchase" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                                AutoGenerateColumns="False" OnRowCommand="gvwOtherPurchase_RowCommand" OnRowCreated="gvwOtherPurchase_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:###,##0}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="UnitName" HeaderText="UnitName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:###,##0}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:CheckBoxField DataField="IsTaxable" HeaderText="Taxable" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnbDeleteItem" runat="server" Text="Delete" CommandName="DeleteItem"
                                                CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <h4>Total Invoice:
                        <asp:Label ID="lblTotalInvoice" runat="server" />
                            </h4>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddDetail" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvwOtherPurchase" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="ddlItemType" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlItem" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </td>
        </tr>
    </table>
    <hr />
    <div>
        <asp:Panel ID="pnlPayment" runat="server">
            <a id="link_payment" href="#">Payment</a>

            <asp:UpdatePanel ID="updPayment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="payment">
                        <table>
                            <tr style="font-weight: bold;">
                                <td>Payment Type:
                                </td>
                                <td></td>
                                <td>Amount:
                                </td>
                                <td>Approval Code:
                                </td>
                                <td>Notes:</td>
                                <td>&nbsp;
                                </td>                                
                            </tr>
                            <tr>
                                <td >
                                    <telerik:RadDropDownList ID="ddlPaymentType" runat="server" AutoPostBack="True"
                                        ValidationGroup="Payment" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" />
                                    </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlCreditCardType" runat="server" ValidationGroup="Payment" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="txtPaymentAmount" runat="server" Width="100px" Value="0"
                                        ValidationGroup="Payment" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtApprovalCode" runat="server" 
                                        Width="150px" ValidationGroup="Payment"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtPaymentNotes" runat="server" 
                                        MaxLength="300" Width="250px"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnAddPayment" runat="server" Text="Add Payment"
                                        OnClick="btnAddPayment_Click" ValidationGroup="Payment" />
                                </td>                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="rqvBillingType" runat="server" ControlToValidate="ddlPaymentType"
                                        CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Payment Type&lt;/b&gt; must be specified"
                                        SetFocusOnError="True" ValidationGroup="Payment"></asp:RequiredFieldValidator>
                                </td>
                                <td></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rqvBillingType0" runat="server" ControlToValidate="txtPaymentAmount"
                                        CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Amount&lt;/b&gt; must be specified"
                                        SetFocusOnError="True" ValidationGroup="Payment"></asp:RequiredFieldValidator>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvwPayment" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                            AutoGenerateColumns="false" OnRowCreated="gvwPayment_RowCreated"
                            OnRowCommand="gvwPayment_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="PaymentType" HeaderText="PaymentType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreditCardType" HeaderText="CreditCardType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:###,##0.00}"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ApprovalCode" HeaderText="ApprovalCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Notes" HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbDeleteItem" runat="server" Text="Delete" CommandName="DeletePayment"
                                            CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure want to delete this row?')" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                .: No Payment Data :.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    <h4>Total Payment:
                        <asp:Label ID="lblTotalPayment" runat="server" />
                    </h4>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddPayment" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvwPayment" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPaymentType" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <br />
        <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
        <br />
        <telerik:RadButton ID="btnSave" runat="server" Text="Save" SingleClick="True" SingleClickText="Saving, please wait"
            EnableViewState="false" OnClick="btnSave_Click"
            ValidationGroup="ExistingMember" />
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(
        function () {
            $("#payment").hide();
        });

        $("#link_payment").click(
        function () {
            $("#payment").toggle("slow");
        });
    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 120px;
        }

        .style3 {
            width: 1px;
        }

        .style4 {
            width: 120px;
            height: 21px;
        }

        .style5 {
            width: 1px;
            height: 21px;
        }

        .style6 {
            height: 21px;
        }

        .style18 {
            width: 130px;
        }

        .style19 {
            width: 59px;
        }

        .style20 {
            width: 70px;
        }

        .style21 {
            width: 60px;
        }

        .style23 {
            width: 90px;
        }

        .style24 {
            width: 195px;
        }

        .style28 {
            width: 161px;
        }

        .style33 {
            width: 156px;
        }

        .style34 {
            width: 293px;
        }
    </style>
</asp:Content>
