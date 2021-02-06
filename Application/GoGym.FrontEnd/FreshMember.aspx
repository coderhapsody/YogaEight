<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="FreshMember.aspx.cs" Inherits="GoGym.FrontEnd.FreshMember" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Membership Invoicing
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="style1">
        <tr>
            <td class="style2">
                Branch
            </td>
            <td class="style3">
                :
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlBranch" runat="server" ValidationGroup="FreshMember" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"/>                
            </td>
        </tr>
        <tr>
            <td class="style2">
                Purchase Date
            </td>
            <td class="style3">
                :
            </td>
            <td>
                <telerik:RadDatePicker runat="server" id="calDate" />              
            </td>
        </tr>
        <tr>
            <td class="style2">
                Contract No.
            </td>
            <td class="style3">
                :
            </td>
            <td>
                <telerik:RadTextBox ID="txtContractNo" runat="server" Width="150px" ValidationGroup="FreshMember" ReadOnly="true"></telerik:RadTextBox>
                <asp:HyperLink ID="hypLookUpContract" NavigateUrl="#" runat="server">Look Up</asp:HyperLink>&nbsp;
                <asp:CustomValidator ID="cuvContractNo" runat="server" ControlToValidate="txtContractNo"
                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Contract No.&lt;/b&gt; is invalid"
                    OnServerValidate="cuvContractNo_ServerValidate" SetFocusOnError="True" ValidationGroup="FreshMember"
                    ValidateEmptyText="True"></asp:CustomValidator>
                &nbsp;<asp:Label ID="lblCustomerBarcode" runat="server"></asp:Label>
                &nbsp;
                <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4">
                Sales
            </td>
            <td class="style5">
                :
            </td>
            <td class="style6">
                <telerik:RadDropDownList ID="ddlSales" runat="server" ValidationGroup="FreshMember"/>                                
                <asp:RequiredFieldValidator ID="rqvSales" runat="server" ControlToValidate="ddlSales"
                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Sales&lt;/b&gt; must be specified"
                    SetFocusOnError="True" ValidationGroup="FreshMember"></asp:RequiredFieldValidator>
                <asp:Button ID="btnDummy" runat="server" OnClick="btnDummy_Click" Style="display: none;"
                    Text="Button" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;
            </td>
            <td class="style3">
                &nbsp;
            </td>
            <td>
                <asp:UpdatePanel ID="updPackage" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblPackage" runat="server"></asp:Label>
                        <asp:GridView ID="gvwPackage" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False"
                            Width="100%" OnRowCreated="gvwPackage_RowCreated" OnRowCommand="gvwPackage_RowCommand"
                            OnRowDataBound="gvwPackage_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UnitName" HeaderText="UnitName" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQuantity" runat="server" DbValue='<%# Bind("Quantity") %>' NumberFormat-DecimalDigits="0"
                                            Width="30px" />                                        
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                                                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" ReadOnly="true" DataFormatString="{0:###,##0.00}"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="UnitPrice" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("UnitPrice", "{0:###,##0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtUnitPrice" runat="server" DbValue='<%# Bind("UnitPrice") %>'
                                            Width="120px" />                                        
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("Discount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtDiscount" runat="server" DbValue='<%# Bind("Discount") %>'
                                            Width="50px" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsTaxable" runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkIsTaxable" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                 <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="true" DataFormatString="{0:###,##0.00}"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbEditPackage" runat="server" CommandName="EditPackage" CommandArgument='<%# Eval("ID") %>'
                                            Text="Edit" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnbDeletePackage" runat="server" CommandName="DeletePackage"
                                            CommandArgument='<%# Eval("ID") %>' Text="Delete" OnClientClick="return confirm('Are you sure want to delete this row?')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnbSavePackage" runat="server" CommandName="SavePackage" CommandArgument='<%# Eval("ID") %>'
                                            Text="Save" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnbCancelPackage" runat="server" CommandName="CancelPackage"
                                            CommandArgument='<%# Eval("ID") %>' Text="Cancel" OnClientClick="return confirm('Cancel current operation ?')" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                .: No Package Data :.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvwPackage" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnDummy" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Label ID="lblTotalAmountPackage" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="style2" valign="top">
                Notes
            </td>
            <td class="style3" valign="top">
                :&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtNotes" runat="server" CssClass="textbox" Width="400px" Rows="5"
                    TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <center>
        <h4>
            <asp:UpdatePanel ID="updTotalInvoice" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    Total Invoice:
                    <asp:Label ID="lblTotalInvoice" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvwPackage" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="btnDummy" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </h4>
    </center>
    <hr />
    <div>
        <a id="link_payment" href="#">Payment</a>
        <div id="payment">
            <asp:UpdatePanel ID="updPayment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="style1">
                        <tr style="font-weight: bold;">
                            <td class="style21">
                                Payment Type:
                            </td>
                            <td class="style26">
                                Amount:
                            </td>
                            <td class="style25">
                                Approval Code:
                            </td>
                            <td class="style25">
                                Notes</td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style24">
                                <telerik:RadDropDownList ID="ddlPaymentType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                </telerik:RadDropDownList>
<%--                                <AjaxToolkit:CascadingDropDown ID="ddlPaymentType_CascadingDropDown" PromptText="Select payment type"
                                    Category="PaymentType" ServicePath="AjaxWebService.asmx" ServiceMethod="GetPaymentTypes"
                                    runat="server" Enabled="True" TargetControlID="ddlPaymentType">
                                </AjaxToolkit:CascadingDropDown>--%>
                                <telerik:RadDropDownList ID="ddlCreditCardType" runat="server" />                                
<%--                                <AjaxToolkit:CascadingDropDown ID="ddlCreditCardType_CascadingDropDown" Category="CreditCardType"
                                    ParentControlID="ddlPaymentType" PromptText="" LoadingText="Loading, please wait"
                                    ServicePath="AjaxWebService.asmx" ServiceMethod="GetCreditCardTypesByPaymentType"
                                    runat="server" Enabled="True" TargetControlID="ddlCreditCardType">
                                </AjaxToolkit:CascadingDropDown>--%>
                            </td>
                            <td class="style26">
                                <telerik:RadNumericTextBox ID="txtPaymentAmount" runat="server" Width="100px" />                                    
                            </td>
                            <td class="style25">
                                <telerik:RadTextBox ID="txtApprovalCode" runat="server" Width="150px"></telerik:RadTextBox>
                            </td>
                            <td class="style25">
                                <telerik:RadTextBox ID="txtPaymentNotes" runat="server" MaxLength="300" Width="250px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadButton ID="btnAddPayment" runat="server" Text="Add Payment"
                                    OnClick="btnAddPayment_Click" />
                            </td>
                            <td>
                                &nbsp;
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
                            <asp:BoundField DataField="ApprovalCode" HeaderText="ApprovalCode" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Notes" HeaderText="Notes" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" />
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddPayment" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvwPayment" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPaymentType" EventName="SelectedIndexChanged" />                    
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <center>
        <h4>
            <asp:UpdatePanel ID="pnlTotalPayment" runat="server">
                <ContentTemplate>
                    Total Payment:
                    <asp:Label ID="lblTotalPayment" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddPayment" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvwPayment" EventName="RowCommand" />
                </Triggers>
            </asp:UpdatePanel>
        </h4>
    </center>
    <br />
    <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
    <br />
    <telerik:RadButton ID="btnSave" runat="server" EnableViewState="false" Text="Save" SingleClickText="Saving, please wait" SingleClick="True"
        ValidationGroup="FreshMember" OnClick="btnSave_Click" />
    <script type="text/javascript" language="javascript">
        $(document).ready(
            function () {
                $("#payment").hide();
            });

        $("#link_payment").click(
            function () {
                $("#payment").toggle("slow");
            });

        function callback() {
            //var contractNo = $get('<%= txtContractNo.ClientID %>').value;                
            __doPostBack('cphMainContent_btnDummy', '');

        }

    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 120px;
        }
        .style3
        {
            width: 1px;
        }
        .style4
        {
            width: 120px;
            height: 21px;
        }
        .style5
        {
            width: 1px;
            height: 21px;
        }
        .style6
        {
            height: 21px;
        }
        .style21
        {
            width: 332px;
            font-weight: bold;
        }
        .style24
        {
            width: 332px;
        }
        .style25
        {
            width: 189px;
        }
        .style26
        {
            width: 130px;
        }
    </style>
</asp:Content>
