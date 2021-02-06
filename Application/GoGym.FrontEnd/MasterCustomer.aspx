<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="MasterCustomer.aspx.cs" Inherits="GoGym.FrontEnd.MasterCustomer" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Customer
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="auto-style2">Branch
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px" />

                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Barcode
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindBarcode" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Name
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindName" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td class="auto-style2">Parent Name</td>
                                <td style="width: 1px">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindParentName" runat="server" ValidationGroup="AddEdit" Width="250px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Phone No. (Customer/Parent) </td>
                                <td style="width: 1px">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindPhoneNo" runat="server" ValidationGroup="AddEdit" Width="250px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td class="style3">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="false" OnClick="btnRefresh_Click" Text="Refresh" ValidationGroup="AddEdit" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                            onclick="lnbAddNew_Click" SkinID="AddNewButton" Text="Add New" 
                            Enabled="False" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                            onclick="lnbDelete_Click" 
                            OnClientClick="return confirm('Delete marked row(s) ?')" SkinID="DeleteButton" 
                            Text="Delete" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                            AutoGenerateColumns="False" DataSourceID="sdsMaster" AllowPaging="True" AllowSorting="True"
                            OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand" OnRowDataBound="gvwMaster_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ContractStatus" HeaderText="ContractStatus" SortExpression="ContractStatus"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="StatusMembership" HeaderText="StatusMembership" SortExpression="StatusMembership"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                                <asp:BoundField DataField="NextDuesDate" HeaderText="NextDuesDate" SortExpression="NextDuesDate"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                                <asp:BoundField DataField="ActiveDate" HeaderText="ActiveDate" SortExpression="ActiveDate"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypChangeCC" runat="server" Text="Change Credit Card" NavigateUrl="#" ImageUrl="~/images/hand.png"
                                            ToolTip="View credit card change history" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypInvoiceHistory" runat="server" Text="Invoice History" NavigateUrl="#" ImageUrl="~/images/mail_16.png"
                                            ToolTip="View invoice history" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypCheckInHistory" runat="server" Text="Check-in History" NavigateUrl="#" ImageUrl="~/images/list_components.gif"
                                            ToolTip="View check-in history" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypNotes" runat="server" Text="Notes" NavigateUrl="#" ImageUrl="~/images/NewDocumentHS.png"
                                            ToolTip="View notes" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow"
                                            CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            SelectCommand="proc_GetAllCustomers" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="BranchID" Type="Int32" />
                                <asp:Parameter Name="Barcode" Type="String" />
                                <asp:Parameter Name="Name" Type="String" />
                                <asp:Parameter Name="ParentName" Type="String" />
                                <asp:Parameter Name="PhoneNo" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <asp:ValidationSummary runat="server" ID="vsmSummary" ValidationGroup="AddEdit" EnableViewState="False" CssClass="errorMessage" />
            <div id="tabs">
                <ul>
                    <li><a href="#tab1">Identity</a></li>
                    <li><a href="#tab2">Addresses</a></li>
                    <li><a href="#tab3">Contact</a></li>
                    <li><a href="#tab4">Billing</a></li>
                    <li><a href="#tab5">Other Info</a></li>
                </ul>
                <div id="tab1">
                    <table class="style1">
                        <tr>
                            <td class="style2">Home Branch
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <asp:Label ID="lblHomeBranch" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Barcode
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBarcode" runat="server" Width="120px" MaxLength="50" ValidationGroup="AddEdit" />
                                <asp:RequiredFieldValidator ID="rqvDescription" runat="server" ControlToValidate="txtBarcode"
                                    EnableViewState="false" ErrorMessage="<b>Barcode</b> must be specified" ValidationGroup="AddEdit"
                                    CssClass="errorMessage" SetFocusOnError="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">First Name
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFirstName" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Last Name
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Surname
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSurname" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Date of Birth
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadDatePicker runat="server" ID="calDate" MinDate="1900-01-01"></telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">ID Card No.
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtIDCardNo" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Occupation
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadDropDownList runat="server" id="ddlOccupation"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" valign="top">Active Contract
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:GridView ID="gvwActiveContracts" runat="server" AutoGenerateColumns="False"
                                    SkinID="GridViewDefaultSkin" Width="100%"
                                    OnRowCommand="gvwActiveContracts_RowCommand" OnRowDataBound="gvwActiveContracts_RowDataBound">
                                    <Columns>
                                        <asp:CommandField />
                                        <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" HeaderStyle-HorizontalAlign="Left"
                                            ReadOnly="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DuesAmount" HeaderText="DuesAmount" HeaderStyle-HorizontalAlign="Right"
                                            ReadOnly="true" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0}" />
                                        <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                                            ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                                            ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                                            ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="ExpiredDate" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%# Convert.ToDateTime(Eval("ExpiredDate")).ToString("ddd, dd-MMM-yyyy") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadDatePicker runat="server" ID="calExpiredDate" SelectedDate='<%# Eval("ExpiredDate") %>'></telerik:RadDatePicker>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NextDuesDate" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%# Convert.ToDateTime(Eval("NextDuesDate")).ToString("ddd, dd-MMM-yyyy") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadDatePicker runat="server" ID="calNextDuesDate" SelectedDate='<%# Eval("NextDuesDate") %>'></telerik:RadDatePicker>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnbEditContract" runat="server" CommandName="EditContract" CommandArgument='<%# Eval("ContractNo") %>' Enabled='<%# Convert.ToString(Eval("Status")).ToUpper() != "CLOSED" && Convert.ToString(Eval("Status")).ToUpper() != "VOID"  %>'
                                                    Text="Edit" />
                                                &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnbDeletePackage" runat="server" CommandName="DeleteContract" Visible="false"
                                            CommandArgument='<%# Eval("ContractNo") %>' Text="Delete" OnClientClick="return confirm('Are you sure want to delete this row?')" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnbSaveContract" runat="server" CommandName="SaveContract" CommandArgument='<%# Eval("ContractNo") %>'
                                                    Text="Save" />
                                                &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnbCancelPackage" runat="server" CommandName="CancelContract"
                                            CommandArgument='<%# Eval("ContractNo") %>' Text="Cancel" OnClientClick="return confirm('Cancel current operation ?')" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderText="Contract Status" HeaderStyle-HorizontalAlign="Left" ReadOnly="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Status
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Large" />
                                &nbsp;<asp:HyperLink ID="hypViewStatusHistory" runat="server" NavigateUrl="#">View Status History</asp:HyperLink>
                                <asp:HiddenField ID="hidStatus" runat="server" />
                                <br />
                                <asp:Label ID="lblStatusNotes" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab2">
                    <table class="style1">
                        <tr>
                            <td class="style2">Address
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAddress" runat="server" MaxLength="500" ValidationGroup="AddEdit" Width="500px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Zip Code
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtZipCode" runat="server" MaxLength="5" ValidationGroup="AddEdit" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">&nbsp;
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Mailing Address
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMailingAddress" runat="server" MaxLength="500" ValidationGroup="AddEdit" Width="500px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Zip Code
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMailingZipCode" runat="server" MaxLength="5" ValidationGroup="AddEdit" Width="80px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab3">
                    <table class="style1">
                        <tr>
                            <td class="style2">Email
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtEmail" runat="server" MaxLength="50" ValidationGroup="AddEdit"
                                    Width="200px" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Email Address&lt;/b&gt; is invalid"
                                    SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Phone
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPhone" runat="server" MaxLength="20" ValidationGroup="AddEdit"
                                    Width="150px" />
                                &nbsp;&nbsp;&nbsp;&nbsp; Cell Phone:
                                <telerik:RadTextBox ID="txtCellPhone" runat="server" MaxLength="20" ValidationGroup="AddEdit" Width="150px" />
                                <telerik:RadTextBox ID="txtCellPhone2" runat="server" MaxLength="20" ValidationGroup="AddEdit" Width="150px" />
                            </td>
                        </tr>
                         <tr>
                            <td class="style2">Work Phone
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtWorkPhone" runat="server" MaxLength="20" ValidationGroup="AddEdit" Width="200px" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="style2">Area
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlArea" runat="server"  DropDownHeight="200px" />
                                
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab4">
                    <table class="style1">
                        <tr>
                            <td class="style2">Billing Type
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlBillingType" runat="server" OnClientSelectedIndexChanged="BillingTypeSelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr id="billing">
                            <td class="style2">&nbsp;
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="style2">Card Number
                                        </td>
                                        <td class="style3">:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtCardNo" runat="server" MaxLength="20" ValidationGroup="AddEdit"
                                                Width="150px" />
                                            &nbsp;<asp:DropDownList ID="ddlCreditCardType" runat="server" CssClass="dropdown">
                                            </asp:DropDownList>
                                            &nbsp;
                                    <asp:CustomValidator ID="cuvCardNo" runat="server" ControlToValidate="txtCardNo"
                                        CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Card Number&lt;/b&gt; must be specified if Billing type is set to Auto Payment"
                                        OnServerValidate="cuvCardNo_ServerValidate" SetFocusOnError="True"
                                        ValidationGroup="AddEdit" Display="Dynamic"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cuvCreditCardNo" runat="server" ControlToValidate="txtCardNo" CssClass="errorMessage" EnableClientScript="False" EnableViewState="False" ErrorMessage="&lt;b&gt;Credit Card Number&lt;/b&gt; is invalid" OnServerValidate="cuvCreditCardNo_ServerValidate" SetFocusOnError="True" ToolTip="Credit Card Number is invalid" ValidationGroup="AddEdit"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">Card Holder Name
                                        </td>
                                        <td class="style3">:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCardHolderName" runat="server" CssClass="textbox" MaxLength="50"
                                                ValidationGroup="AddEdit" Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">Card Holder ID No.
                                        </td>
                                        <td class="style3">:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCardHolderID" runat="server" CssClass="textbox" MaxLength="20"
                                                onkeydown="return NumbersOnly(event)" ValidationGroup="AddEdit" Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2">Issued by Bank
                                        </td>
                                        <td class="style3">:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBank" runat="server" CssClass="dropdown">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp; Card Expired Date:
                                    <telerik:RadDatePicker runat="server" ID="calExpiredDate"></telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab5">
                    <table class="style1">
                        <tr>
                            <td class="style2" valign="top">Partner
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:HiddenField ID="hidPartner" runat="server" />
                                <telerik:RadTextBox ID="txtPartner" runat="server" MaxLength="20" ValidationGroup="AddEdit"
                                    Width="150px" />
                                <asp:HyperLink ID="hypLookUpPartner" NavigateUrl="#" runat="server">Look Up</asp:HyperLink>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CustomValidator ID="cuvPartner" runat="server" ControlToValidate="txtPartner"
                            CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Partner&lt;/b&gt; is invalid"
                            SetFocusOnError="True" ValidationGroup="AddEdit" OnServerValidate="cuvPartner_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" valign="top">Parents
                            </td>
                            <td class="style3" valign="top">:
                            </td>
                            <td>
                                <asp:HyperLink ID="hypParent" runat="server" EnableViewState="False" NavigateUrl="#">Click here to modify information of Parents / Guardians / Pick Up Persons</asp:HyperLink>
                                <asp:GridView ID="gvwParents" runat="server" AutoGenerateColumns="False" DataSourceID="sdsParents"
                                    SkinID="GridViewDefaultSkin" Width="700px">
                                    <Columns>
                                        <asp:BoundField DataField="Connection" HeaderText="Connection" ReadOnly="True" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="Connection" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BirthDate" HeaderText="BirthDate" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="BirthDate" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Phone1" HeaderText="Phone1" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="Phone1" />
                                        <asp:BoundField DataField="Phone2" HeaderText="Phone2" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="Phone2" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="sdsParents" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                                    SelectCommand="proc_GetCustomerParents" SelectCommandType="StoredProcedure" OnSelecting="sdsParents_Selecting">
                                    <SelectParameters>
                                        <asp:Parameter Name="CustomerBarcode" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Photo
                            </td>
                            <td class="style3">:
                            </td>
                            <td>
                                <asp:FileUpload ID="fupPhoto" runat="server" />
                                <asp:CheckBox ID="chkDeletePhoto" runat="server" Text="Delete current photo" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">&nbsp;
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td>
                                <asp:Image ID="imgPhoto" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <telerik:RadButton ID="btnSave" runat="server" EnableViewState="False" Text="Save" OnClick="btnSave_Click" ValidationGroup="AddEdit" CausesValidation="True" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadButton ID="btnCancel" runat="server" EnableViewState="false" Text="Cancel" ValidationGroup="AddEdit" CausesValidation="false" OnClientClicking="CancelConfirm" OnClick="btnCancel_Click" />
        </asp:View>
    </asp:MultiView>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();

            var ddlBillingType = $find("<%= ddlBillingType.ClientID %>");
            if (ddlBillingType != null) {
                if (ddlBillingType.get_selectedItem().get_text() != "Manual Payment") {
                    $("#billing").show();
                }
                else {
                    $("#billing").hide();
                }
            }
        });

        function BillingTypeSelectedIndexChanged(sender, args) {
            var ddlBillingType = $find("<%= ddlBillingType.ClientID %>");
            if (ddlBillingType != null) {
                if (ddlBillingType.get_selectedItem().get_text() != "Manual Payment")
                    $("#billing").show();
                else
                    $("#billing").hide();
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
            width: 130px;
        }

        .auto-style1 {
            width: 131px;
        }

        .auto-style2 {
            width: 181px;
        }
    </style>
</asp:Content>
