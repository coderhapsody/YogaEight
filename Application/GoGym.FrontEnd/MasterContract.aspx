﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterContract.aspx.cs" Inherits="GoGym.FrontEnd.MasterContract" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Contract
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1" width="100%">
                <tr>
                    <td>
                        <table class="ui-accordion">
                            <tr>
                                <td class="auto-style2">Branch
                                </td>
                                <td class="style5">:
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Contract No.</td>
                                <td class="style5">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindContractNo" runat="server" MaxLength="30" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Customer Name</td>
                                <td class="style5">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindCustomerName" runat="server" MaxLength="50" Width="150px" /></td>
                            </tr>
                            <tr>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td class="style5">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="False"
                                        Text="Refresh" OnClick="btnRefresh_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td class="style5">&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" OnClick="lnbAddNew_Click"
                            SkinID="AddNewButton" Text="Add New" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                            AutoGenerateColumns="False" DataSourceID="sdsMaster" AllowPaging="True" AllowSorting="True"
                            OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand" OnRowDataBound="gvwMaster_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Name" SortExpression="Name"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Date" HeaderText="Contract Date" SortExpression="Date" HeaderStyle-HorizontalAlign="Left"
                                    DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                                <asp:BoundField DataField="ActiveDate" HeaderText="Activation Date" SortExpression="ActiveDate"
                                    DataFormatString="{0:ddd, dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" SortExpression="EffectiveDate"
                                    DataFormatString="{0:ddd, dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ExpiredDate" HeaderText="Expire Date" SortExpression="ExpiredDate"
                                    DataFormatString="{0:ddd, dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="BillingType" HeaderText="BillingType" SortExpression="BillingType"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Status" HeaderText="Contract Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="StatusMembership" HeaderText="Status Membersip" SortExpression="StatusMembership" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypPrint" runat="server" ImageUrl="~/Images/PrintHS.png" NavigateUrl="#" ToolTip="Print" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypSalesPoints" runat="server" Visible="False" ImageUrl="~/Images/HashSign.png" NavigateUrl="#" ToolTip="Sales Points" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow"
                                            CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            SelectCommand="proc_GetAllContracts" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="BranchID" Type="Int32" />
                                <asp:Parameter Name="ContractNo" Type="String" />
                                <asp:Parameter Name="CustomerName" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <asp:ValidationSummary runat="server" ID="vsmAddEdit" ValidationGroup="AddEdit" EnableViewState="False" CssClass="errorMessage" />
            <div id="tabs">
                <ul>
                    <li><a href="#tab1">General</a></li>
                    <li><a href="#tab2">Billing</a></li>
                    <li><a href="#tab3">Customer Info</a></li>
                    <li><a href="#tab4">Contract Detail</a></li>
                </ul>
                <div id="tab1">
                    <table class="style1">
                        <tr>
                            <td class="style7">Branch
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">Contract No.
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <asp:Label ID="lblContractNo" runat="server"></asp:Label>
                                &nbsp;&nbsp;
                        <span id="transfer">
                            <asp:CheckBox ID="chkIsTransfer" runat="server" Text="This is transfer contract" /></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">Date
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadDatePicker runat="server" ID="calDate"></telerik:RadDatePicker>
                                &nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Effective Date :
                        <telerik:RadDatePicker runat="server" ID="calEffectiveDate"></telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Customer
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:CheckBox ID="chkGenerateNewBarcodeCustomer" runat="server" Text="Generate New Barcode" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        <span id="newcustomer">First Name (*):
                            <asp:TextBox ID="txtCustomerFirstName" runat="server" CssClass="textbox" MaxLength="50"
                                ValidationGroup="AddEdit" Width="114px" />
                            &nbsp;&nbsp;&nbsp; Last Name (*):&nbsp;<asp:TextBox ID="txtCustomerLastName" runat="server"
                                CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit" Width="114px" />
                            &nbsp;&nbsp;&nbsp; Date of Birth:<telerik:RadDatePicker runat="server" ID="calDateOfBirth"></telerik:RadDatePicker>
                            &nbsp;&nbsp;<asp:CustomValidator ID="cuvNewCustomer" runat="server" ControlToValidate="txtCustomerFirstName"
                                CssClass="errorMessage" EnableClientScript="False" EnableViewState="False" ErrorMessage="&lt;b&gt;Customer Name&lt;/b&gt; must be specified"
                                OnServerValidate="cuvNewCustomer_ServerValidate" SetFocusOnError="True" ValidateEmptyText="True"
                                ValidationGroup="AddEdit"></asp:CustomValidator>
                        </span>&nbsp;<span id="customer">Customer Code:
                            <telerik:RadTextBox ID="txtCustomerBarcode" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="114px" />
                            &nbsp;<asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server" onclick="showPromptPopUp('PromptCustomer.aspx?', this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                            &nbsp;<asp:CustomValidator ID="cuvExistingCustomer" runat="server" ControlToValidate="txtCustomerBarcode"
                                CssClass="errorMessage" EnableClientScript="False" EnableViewState="False" ErrorMessage="&lt;b&gt;Customer Code&lt;/b&gt; is invalid"
                                OnServerValidate="cuvExistingCustomer_ServerValidate" SetFocusOnError="True"
                                ValidateEmptyText="True" ValidationGroup="AddEdit"></asp:CustomValidator>
                            &nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlRenewalOrUpgrade" runat="server" CssClass="dropdown">
                                <asp:ListItem Text="Renewal" Value="R" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Upgrade" Value="U"></asp:ListItem>
                            </asp:DropDownList>
                        </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Package (*)</td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPackage" runat="server" CssClass="dropdown" ValidationGroup="AddEdit"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rqvPackage" runat="server" ControlToValidate="ddlPackage"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Package &lt;/b&gt;must be specified"
                                    SetFocusOnError="True" ValidationGroup="AddEdit"></asp:RequiredFieldValidator>
                                <asp:UpdatePanel ID="updPackage" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvwPackage" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False"
                                            Width="400px" OnRowCreated="gvwPackage_RowCreated">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" />
                                                <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" CssClass="textbox" runat="server" Text='<%# Bind("Quantity") %>'
                                                            Width="30px" />
                                                        <%--<AjaxToolkit:FilteredTextBoxExtender ID="fltQuantity" runat="server" FilterType="Numbers"
                                                    TargetControlID="txtQuantity" />--%>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                .: No Package Data :.
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <asp:Label ID="lblBillingCycle" runat="server" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlPackage" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab2">
                    <table class="style1">
                        <tr>
                            <td class="style7" valign="top">Billing Type (*)</td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBillingType" runat="server" CssClass="dropdown" ValidationGroup="AddEdit">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rqvBillingType" runat="server" ControlToValidate="ddlBillingType"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Billing Type&lt;/b&gt; must be specified"
                                    SetFocusOnError="True" ValidationGroup="AddEdit"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="billing">
                            <td class="style7" valign="top">&nbsp;</td>
                            <td class="style3" valign="top">&nbsp;</td>
                            <td>
                                <table class="ui-accordion">
                                    <tr>
                                        <td class="style8">Card Type</td>
                                        <td class="style3">:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlBillingCardType" runat="server" CssClass="dropdown"
                                                ValidationGroup="AddEdit">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8">Issued by Bank</td>
                                        <td class="style3">:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlBillingBank" runat="server" CssClass="dropdown"
                                                ValidationGroup="AddEdit">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8">Card No.</td>
                                        <td class="style3">:</td>
                                        <td>
                                            <asp:TextBox ID="txtBillingCardNo" runat="server" CssClass="textbox"
                                                MaxLength="16" ValidationGroup="AddEdit" Width="150px" onkeydown="return NumbersOnly(event);" />
                                            <asp:CustomValidator ID="cuvCreditCardNo" runat="server"
                                                ControlToValidate="txtBillingCardNo" CssClass="errorMessage"
                                                EnableClientScript="False" EnableViewState="False"
                                                ErrorMessage="&lt;b&gt;Credit Card Number&lt;/b&gt; is invalid"
                                                OnServerValidate="cuvCreditCardNo_ServerValidate" SetFocusOnError="True"
                                                ToolTip="Credit Card Number is invalid" ValidateEmptyText="True"
                                                ValidationGroup="AddEdit2"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8">Card Holder Name</td>
                                        <td class="style3">:</td>
                                        <td>
                                            <asp:TextBox ID="txtBillingCardHolderName" runat="server" CssClass="textbox"
                                                MaxLength="50" ValidationGroup="AddEdit" Width="300px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8">Card Holder ID No.</td>
                                        <td class="style3">:</td>
                                        <td>
                                            <asp:TextBox ID="txtBillingCardHolderID" runat="server" CssClass="textbox"
                                                MaxLength="20" ValidationGroup="AddEdit" Width="150px" onkeydown="return NumbersOnly(event);" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8">Card Expired Date</td>
                                        <td class="style3">:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlCardExpiredMonth" runat="server" CssClass="dropdown" />&nbsp;
                                    <asp:DropDownList ID="ddlCardExpiredYear" runat="server" CssClass="dropdown" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style8">&nbsp;</td>
                                        <td class="style3">&nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblStatusBilling" runat="server" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab3">
                    <table class="style1" id="customerInfo">
                        <tr>
                            <td class="style7" valign="top">ID Card No.</td>
                            <td class="style3" valign="top">:</td>
                            <td>
                                <telerik:RadTextBox ID="txtIDCardNo" runat="server" MaxLength="20" ValidationGroup="AddEdit" Width="200px" /></td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Occupation</td>
                            <td class="style3" valign="top">:</td>
                            <td>
                            <telerik:RadDropDownList runat="server" id="ddlOccupation"/>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Mailing Address </td>
                            <td class="style3" valign="top">: </td>
                            <td>
                                <telerik:RadTextBox ID="txtMailingAddress" runat="server" MaxLength="500" ValidationGroup="AddEdit" Width="500px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Mailing Zip Code
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMailingZipCode" runat="server" MaxLength="5"
                                    ValidationGroup="AddEdit" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Address
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAddress" runat="server" MaxLength="500"
                                    ValidationGroup="AddEdit" Width="500px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Zip Code
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtZipCode" runat="server" MaxLength="5"
                                    ValidationGroup="AddEdit" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Home Phone No.
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtHomePhone" runat="server" MaxLength="20" ValidationGroup="AddEdit" Width="150px" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Cell Phone:
                                <telerik:RadTextBox ID="txtCellPhone" runat="server" MaxLength="20" ValidationGroup="AddEdit" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Work Phone</td>
                            <td class="style3" valign="top">:</td>
                            <td>
                                <telerik:RadTextBox ID="txtWorkPhone" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Area</td>
                            <td class="style3" valign="top">:</td>
                            <td>
                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="dropdown"
                                    ValidationGroup="AddEdit">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td class="style7" valign="top">Notes
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" runat="server" CssClass="textbox" MaxLength="1500" Rows="7"
                                    TextMode="MultiLine" ValidationGroup="AddEdit" Width="400px" />
                            </td>
                        </tr>

                    </table>
                </div>
                <div id="tab4">
                    <table class="style1">
                        <tr>
                            <td class="style7" valign="top">Active Date
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:Label ID="lblActiveDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Expired Date</td>
                            <td class="style3" valign="top">:</td>
                            <td>
                                <telerik:RadDatePicker runat="server" ID="calExpiredDate"></telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Closed Date
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:Label ID="lblClosedDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Void Date
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:Label ID="lblVoidDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" valign="top">Status
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">&nbsp;
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">Monthly Dues Item</td>
                            <td class="style3">:</td>
                            <td>
                                <asp:DropDownList ID="ddlMonthlyDuesItem" runat="server" CssClass="dropdown" ValidationGroup="AddEdit">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">Reg. Monthly Dues</td>
                            <td class="style3">:</td>
                            <td>
                                <telerik:RadNumericTextBox runat="server" ID="txtDuesAmount" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">Locker Charge</td>
                            <td class="style3">:</td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtAdditionalDuesAmount" runat="server" Width="100px" />

                            </td>
                        </tr>
                        <tr>
                            <td class="style7">Next Dues Date</td>
                            <td class="style3">:</td>
                            <td>
                                <telerik:RadDatePicker runat="server" ID="calNextDuesDate"></telerik:RadDatePicker>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>



            <table style="width: 100%; display: none;" id="parentsInfo">
                <tr>
                    <td style="width: 50%;">
                        <asp:CheckBox ID="chkFather" runat="server" Text="Father" Visible="False" />
                    </td>
                    <td style="width: 50%;">
                        <asp:CheckBox ID="chkMother" runat="server" Text="Mother" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%;">
                        <div id="father" style="border: 1px solid black;">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style6" valign="top">Father
                                    </td>
                                    <td class="style3" valign="top">&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Name
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="textbox" MaxLength="50"
                                            ValidationGroup="AddEdit" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Birth Date
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker runat="server" ID="calBirthDateFather"></telerik:RadDatePicker>
                                        <asp:CheckBox ID="chkFatherBirthDateUnknown" runat="server" Text="Unknown" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">ID Card No.
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIDCardNoFather" runat="server" CssClass="textbox" MaxLength="20"
                                            ValidationGroup="AddEdit" Width="200px" onkeydown="return NumbersOnly(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Cellular Phone No.
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFatherPhone" runat="server" CssClass="textbox" MaxLength="20"
                                            ValidationGroup="AddEdit" Width="150px" onkeydown="return NumbersOnly(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Email</td>
                                    <td class="style3" valign="top">:</td>
                                    <td>
                                        <asp:TextBox ID="txtFatherEmail" runat="server" CssClass="textbox"
                                            MaxLength="50" ValidationGroup="AddEdit"
                                            Width="150px" />
                                        <asp:RegularExpressionValidator ID="rqvFatherEmail" runat="server"
                                            ControlToValidate="txtFatherEmail" CssClass="errorMessage"
                                            EnableViewState="False" ErrorMessage="Email address is invalid"
                                            SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td style="width: 50%;">
                        <div id="mother" style="border: 1px solid black;">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style6" valign="top">Mother
                                    </td>
                                    <td class="style3" valign="top">&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Name
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMotherName" runat="server" CssClass="textbox" MaxLength="50"
                                            ValidationGroup="AddEdit" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Birth Date
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker runat="server" ID="calBirthDateMother"></telerik:RadDatePicker>
                                        <asp:CheckBox ID="chkMotherBirthDateUnknown" runat="server" Text="Unknown" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">ID Card No.
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIDCardNoMother" runat="server" CssClass="textbox" MaxLength="20"
                                            ValidationGroup="AddEdit" Width="200px" onkeydown="return NumbersOnly(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Cellular Phone No.
                                    </td>
                                    <td class="style3" valign="top">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMotherPhone" runat="server" CssClass="textbox" MaxLength="20" onkeydown="return NumbersOnly(event)"
                                            ValidationGroup="AddEdit" Width="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" valign="top">Email</td>
                                    <td class="style3" valign="top">:</td>
                                    <td>
                                        <asp:TextBox ID="txtMotherEmail" runat="server" CssClass="textbox"
                                            MaxLength="50" ValidationGroup="AddEdit"
                                            Width="150px" />
                                        <asp:RegularExpressionValidator ID="rqvMotherEmail" runat="server"
                                            ControlToValidate="txtMotherEmail" CssClass="errorMessage"
                                            EnableViewState="False" ErrorMessage="Email address is invalid"
                                            SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblStatusParent" runat="server" EnableViewState="false" />
            <div>
                <table style="width: 100%;">
                    <tr>
                        <td class="style2">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="style2">&nbsp;
                        </td>
                        <td class="style3">&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblMessageAddEdit" runat="server" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">&nbsp;
                        </td>
                        <td class="style3">&nbsp;
                        </td>
                        <td>
                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" EnableViewState="false" SingleClick="True" SingleClickText="Saving..."
                                OnClick="btnSave_Click" ValidationGroup="AddEdit" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            
                            <telerik:RadButton ID="btnPrint" runat="server" Text="Print Agreement" OnClientClicking="btnPrintClick" EnableViewState="false" ValidationGroup="AddEdit" />                            
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <telerik:RadButton ID="btnVoid" runat="server" CausesValidation="false"
                                EnableViewState="false" OnClick="btnVoid_Click" OnClientClicking="VoidConfirm"
                                Text="Void Contract" ValidationGroup="AddEdit" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadButton ID="btnCloseContract" runat="server" CausesValidation="false" EnableViewState="false" Text="Close Contract" ValidationGroup="AddEdit" OnClick="btnCloseContract_Click" />
                            &nbsp;&nbsp;
                            <telerik:RadButton ID="btnCancel" runat="server" CausesValidation="false" EnableViewState="false"
                                OnClick="btnCancel_Click" OnClientClicking="CancelConfirm"
                                Text="Cancel" ValidationGroup="AddEdit" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
    </asp:MultiView>
    <script>
        function btnPrintClick(sender, arg) {
            arg.set_cancel(true);
        }

        $(document).ready(function () {
            $("#tabs").tabs();

            if ($("#<%= chkGenerateNewBarcodeCustomer.ClientID %>").is(":checked")) {
                $("#customer").hide();
                $("#newcustomer").show();
                $("#parentsInfo").show();
            }
            else {
                $("#customer").show();
                $("#newcustomer").hide();
                $("#parentsInfo").hide();
            }

            if ($("#<%= ddlBillingType.ClientID %>").get(0) != null) {
                var ddlBillingType = $("#<%= ddlBillingType.ClientID %>").get(0);
                if (ddlBillingType.item(ddlBillingType.selectedIndex).text != "Manual Payment") {
                    $("#billing").show();
                    $("#<%= ddlMonthlyDuesItem.ClientID %>").parent().parent().show();
                    $("#<%= txtDuesAmount.ClientID %>").parent().parent().show();
                }
                else {
                    $("#billing").hide();
                    $("#<%= ddlMonthlyDuesItem.ClientID %>").parent().parent().hide();
                    $("#<%= txtDuesAmount.ClientID %>").parent().parent().hide();
                }
            }


            if ($("#<%= chkFatherBirthDateUnknown.ClientID %>").is(":checked"))
                $("#<%=calBirthDateFather.ClientID %>").hide();
            else
                $("#<%=calBirthDateFather.ClientID %>").show();


            if ($("#<%= chkMotherBirthDateUnknown.ClientID %>").is(":checked"))
                $("#<%=calBirthDateMother.ClientID %>").hide();
            else
                $("#<%=calBirthDateMother.ClientID %>").show();

            if ($("#<%= chkFather.ClientID %>").is(":checked"))
                $("#father").show();
            else
                $("#father").hide();

            if ($("#<%= chkMother.ClientID %>").is(":checked"))
                $("#mother").show();
            else
                $("#mother").hide();

        });

        $("#<%= ddlBillingType.ClientID %>").change(function () {
            var ddlBillingType = $("#<%= ddlBillingType.ClientID %>").get(0);
            if (ddlBillingType.item(ddlBillingType.selectedIndex).text != "Manual Payment") {
                $("#billing").show();
                $("#<%= ddlMonthlyDuesItem.ClientID %>").parent().parent().show();
                $("#<%= txtDuesAmount.ClientID %>").parent().parent().show();
            }
            else {
                $("#billing").hide();
                $("#<%= ddlMonthlyDuesItem.ClientID %>").parent().parent().hide();
                $("#<%= txtDuesAmount.ClientID %>").parent().parent().hide();
            }
        });


        $("#<%= chkFather.ClientID %>").click(
            function () {
                $("#father").toggle("fast");
            });

        $("#<%= chkMother.ClientID %>").click(
            function () {
                $("#mother").toggle("fast");
            });

        $("#<%= chkGenerateNewBarcodeCustomer.ClientID %>").click(
            function () {
                if (this.checked) {
                    $("#customer").hide();
                    $("#newcustomer").show();
                    $("#parentsInfo").show();
                    $("#transfer").show();
                }
                else {
                    $("#customer").show();
                    $("#newcustomer").hide();
                    $("#parentsInfo").hide();
                    $("#transfer").hide();

                }
            });

        $("#<%= chkFatherBirthDateUnknown.ClientID %>").click(
            function () {
                $("#<%=calBirthDateFather.ClientID %>").toggle();
            });

        $("#<%= chkMotherBirthDateUnknown.ClientID %>").click(
            function () {
                $("#<%=calBirthDateMother.ClientID %>").toggle();
            });
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
            width: 90px;
        }

        .style5 {
            width: 2px;
        }

        .style6 {
            width: 140px;
            font-weight: bold;
        }

        .style7 {
            width: 115px;
        }

        .style8 {
            width: 120px;
        }

        .auto-style2 {
            width: 143px;
        }
    </style>
</asp:Content>
