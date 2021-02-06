<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="PaymentManual.aspx.cs" Inherits="GoGym.FrontEnd.PaymentManual" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 130px;
        }

        .auto-style2 {
            width: 2px;
        }
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Payment
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div>
        <table style="width: 100%">
            <tr>
                <td class="auto-style1">Invoice No.</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style1">Employee</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblEmployee" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style1">Invoice Date</td>
                <td class="auto-style2">:</td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="calInvoiceDate"></telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">Payment Date</td>
                <td class="auto-style2">:</td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="calPaymentDate"></telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">Customer</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblCustomer" runat="server"></asp:Label>

                </td>
            </tr>
            <tr style="display: none;">
                <td class="auto-style1">Invoice Type</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblInvoiceType" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style1">Notes</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblNotes" runat="server"></asp:Label>

                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>Select Item to add:</td>
                <td>Quantity</td>
                <td>Unit Price</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadDropDownList ID="ddlItemType" runat="server" AutoPostBack="True" CausesValidation="False" OnSelectedIndexChanged="ddlItemType_SelectedIndexChanged" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadDropDownList ID="ddlItem" runat="server" />
                    <asp:RequiredFieldValidator ID="rqvItemType" runat="server" ErrorMessage="Item Type must be specified" ControlToValidate="ddlItemType" ValidationGroup="Charge" Display="Dynamic" CssClass="errorMessage" Text="*" /><br />
                    <asp:RequiredFieldValidator ID="rqvItem" runat="server" ErrorMessage="Item must be specified" ControlToValidate="ddlItem" ValidationGroup="Charge" Display="Dynamic" CssClass="errorMessage" Text="*" />
                    <asp:Label ID="lblStatusItem" runat="server" EnableViewState="false" />
                </td>
                <td>
                    <telerik:RadNumericTextBox runat="server" ID="txtQuantity" NumberFormat-DecimalDigits="0" />
                </td>
                <td>
                    <telerik:RadNumericTextBox runat="server" ID="txtUnitPrice" NumberFormat-DecimalDigits="0" />
                    <asp:RequiredFieldValidator ID="rqvUnitCharge" runat="server" ErrorMessage="Unit Price must be specified" ControlToValidate="txtUnitPrice" ValidationGroup="Charge" CssClass="errorMessage" Text="*" /></td>
                <td>
                    <telerik:RadButton ID="btnPopupOK" runat="server" CausesValidation="true" EnableViewState="False" Text="Add" OnClick="btnPopupOK_Click" ValidationGroup="Charge" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvwInvoice" runat="server" SkinID="GridViewDefaultSkin"
            Width="100%" AutoGenerateColumns="false"
            OnRowDataBound="gvwInvoice_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:###,##0.00}"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:###,##0.00}"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:###,##0.00}"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="Taxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsTaxable")) ? "Taxed" : "Not Taxed" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:###,##0.00}"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
            </Columns>
        </asp:GridView>
        <p>
            <strong>Total Before Tax:&nbsp;</strong><asp:Label ID="lblTotalBeforeTax" runat="server" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <strong>Total Tax:&nbsp;</strong><asp:Label ID="lblTotalTax" runat="server" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <strong>Total After Tax:&nbsp;</strong><asp:Label ID="lblTotalInvoice" runat="server" />
        </p>
    </div>
    <br />
    <div>
        <asp:UpdatePanel ID="updPayment" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="payment">
                    <table class="style1">
                        <tr style="font-weight: bold;">
                            <td class="style34">Payment Type:
                            </td>
                            <td class="style33">Amount:
                            </td>
                            <td class="style28">Approval Code:
                            </td>
                            <td class="style28">Notes:</td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style34">
                                <telerik:RadDropDownList ID="ddlPaymentType" runat="server" ValidationGroup="Payment" AutoPostBack="True" CausesValidation="False" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" />
                                <%--                                <AjaxToolkit:CascadingDropDown ID="ddlPaymentType_CascadingDropDown" PromptText="Select payment type"
                                    Category="PaymentType" ServicePath="AjaxWebService.asmx" ServiceMethod="GetPaymentTypes"
                                    runat="server" Enabled="True" TargetControlID="ddlPaymentType">
                                </AjaxToolkit:CascadingDropDown>--%>
                                <telerik:RadDropDownList ID="ddlCreditCardType" runat="server" ValidationGroup="Payment" />

                                <%--                                <AjaxToolkit:CascadingDropDown ID="ddlCreditCardType_CascadingDropDown" Category="CreditCardType"
                                    ParentControlID="ddlPaymentType" PromptText="" LoadingText="Loading, please wait"
                                    ServicePath="AjaxWebService.asmx" ServiceMethod="GetCreditCardTypesByPaymentType"
                                    runat="server" Enabled="True" TargetControlID="ddlCreditCardType">
                                </AjaxToolkit:CascadingDropDown>--%>
                            </td>
                            <td class="style33">
                                <telerik:RadNumericTextBox runat="server" ID="txtPaymentAmount"></telerik:RadNumericTextBox>
                            </td>
                            <td class="style28">
                                <telerik:RadTextBox ID="txtApprovalCode" runat="server" Width="150px" ValidationGroup="Payment" />
                            </td>
                            <td class="style28">
                                <telerik:RadTextBox ID="txtPaymentNotes" runat="server" MaxLength="300" Width="250px" />
                            </td>
                            <td>
                                <telerik:RadButton ID="btnAddPayment" runat="server" Text="Add Payment" OnClick="btnAddPayment_Click" ValidationGroup="Payment" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style34">
                                <asp:RequiredFieldValidator ID="rqvBillingType" runat="server" ControlToValidate="ddlPaymentType"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Payment Type&lt;/b&gt; must be specified"
                                    SetFocusOnError="True" ValidationGroup="Payment"></asp:RequiredFieldValidator>
                            </td>
                            <td class="style33">
                                <asp:RequiredFieldValidator ID="rqvBillingType0" runat="server" ControlToValidate="txtPaymentAmount"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Amount&lt;/b&gt; must be specified"
                                    SetFocusOnError="True" ValidationGroup="Payment"></asp:RequiredFieldValidator>
                            </td>
                            <td class="style28">&nbsp;
                            </td>
                            <td class="style28">&nbsp;</td>
                            <td>&nbsp;
                            </td>
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

        <br />
        <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
        <br />
        <telerik:RadButton ID="btnSave" runat="server" Text="Save" EnableViewState="false" OnClick="btnSave_Click" ValidationGroup="ExistingMember" />
    </div>
</asp:Content>

