<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ApprovalChangeStatusDocument.aspx.cs" Inherits="GoGym.FrontEnd.ApprovalChangeStatusDocument" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Approval Change Status
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">

    <asp:MultiView ID="mvwForm" ActiveViewIndex="0" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td>  
                        <table class="ui-accordion">
                            <tr>
                                <td class="style3">Branch</td>
                                <td class="style4">:</td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Document Type</td>
                                <td class="style4">:</td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlFindDocumentType" runat="server" Width="200px"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Period</td>
                                <td class="style4">:</td>
                                <td>From Date
                                    <telerik:RadDatePicker runat="server" ID="calFindFromDate"></telerik:RadDatePicker>
                                    &nbsp;&nbsp;&nbsp; To Date
                                    <telerik:RadDatePicker runat="server" ID="calFindToDate"></telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Customer Barcode</td>
                                <td class="style4">:</td>
                                <td>
                                    <telerik:RadTextBox ID="txtFindCustomerCode" runat="server" MaxLength="50" ValidationGroup="AddEdit" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td>
                                    <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False"
                DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%"
                OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand"
                AllowPaging="True" AllowSorting="True"
                OnRowDataBound="gvwMaster_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DocumentNo" HeaderText="DocumentNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="DocumentNo" />
                    <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="CustomerCode" />
                    <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="CustomerName" />
                    <asp:BoundField DataField="EmployeeCode" HeaderText="Issued By" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="EmployeeName" />
                    <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="DocumentType" />
                    <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="DocumentStatus" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" DataFormatString="{0:dddd, dd MMMM yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="StartDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate" DataFormatString="{0:dddd, dd MMMM yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="EndDate" />
                    <asp:BoundField DataField="ApprovedDate" HeaderText="ApprovedDate"
                        SortExpression="ApprovedDate" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server"
                ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                SelectCommand="proc_GetAllChangeStatusDocuments"
                SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="DocumentTypeID" Type="Int32" />
                    <asp:Parameter Name="DateFrom" Type="String" />
                    <asp:Parameter Name="DateTo" Type="String" />
                    <asp:Parameter Name="CustomerCode" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="style1">Branch</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Document No.</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:Label ID="lblDocumentNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Document Type</td>
                    <td class="style2">:</td>
                    <td>
                        <telerik:RadDropDownList ID="ddlDocumentType" runat="server" Width="120px"/>                        
                    </td>
                </tr>
                <tr>
                    <td class="style1">Start Date</td>
                    <td class="style2">:</td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="calStartDate"></telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="style1">End Date</td>
                    <td class="style2">:</td>
                    <td>
                        <span id="enddate">
                            <telerik:RadDatePicker runat="server" ID="calEndDate"></telerik:RadDatePicker>
                        </span>
                        <asp:CheckBox ID="chkEndDate" runat="server" Text="Specify End Date" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">Customer</td>
                    <td class="style2">:</td>
                    <td>
                        <telerik:RAdTextBox ID="txtCustomerCode" runat="server" MaxLength="50"
                            ValidationGroup="AddEdit" Width="150px" />&nbsp;<asp:HyperLink
                                ID="hypPromptCustomer" NavigateUrl="#" runat="server">Look Up</asp:HyperLink>
                        &nbsp;
                        <asp:CustomValidator ID="cuvCustomerCode" runat="server"
                            ClientValidationFunction="ValidateCustomerCode"
                            ControlToValidate="txtCustomerCode" CssClass="errorMessage"
                            EnableViewState="False"
                            ErrorMessage="&lt;b&gt;Customer&lt;/b&gt; must be specified with valid customer code"
                            OnServerValidate="cuvCustomerCode_ServerValidate" SetFocusOnError="True"
                            ValidateEmptyText="True" ValidationGroup="AddEdit"></asp:CustomValidator>
                    </td>
                </tr>

                <tr>
                    <td class="style1" valign="top">Notes</td>
                    <td class="style2" valign="top">:</td>
                    <td>
                        <asp:TextBox ID="txtNotes" runat="server" Columns="80" CssClass="textbox"
                            MaxLength="50" Rows="8" TextMode="MultiLine" ValidationGroup="AddEdit" />
                        <br />
                        <asp:RequiredFieldValidator ID="rqvNotes" runat="server"
                            ControlToValidate="txtNotes" CssClass="errorMessage" EnableViewState="False"
                            ErrorMessage="&lt;b&gt;Notes&lt;/b&gt; must be specified"
                            SetFocusOnError="True" ValidationGroup="AddEdit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Approval</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:Label ID="lblApprovalStatus" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">&nbsp;</td>
                    <td class="style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">&nbsp;</td>
                    <td class="style2">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">&nbsp;</td>
                    <td class="style2">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" EnableViewState="false"
                            OnClick="btnSave_Click" Text="Save" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" CausesValidation="false"
                            EnableViewState="false" OnClick="btnCancel_Click"
                            OnClientClicking="CancelConfirm"
                            Text="Cancel" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnApprove" runat="server" 
                            EnableViewState="false" OnClick="btnApprove_Click" Text="Approve"
                            OnClientClicking="ApproveConfirm"
                            ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                        <telerik:RadButton ID="btnVoid" runat="server" CausesValidation="false" 
                            EnableViewState="false" OnClick="btnVoid_Click"
                            OnClientClick="VoidConfirm" Text="Void"
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

    <script language="javascript" type="text/javascript">
        function ValidateCustomerCode(source, arg) {
            arg.IsValid = $('#<%= txtCustomerCode.ClientID %>').get(0).value != "";
        }

        $(document).ready(function () {
            if ($("#<%= chkEndDate.ClientID %>").is(":checked")) {
                $("#enddate").show();
            }
            else {
                $("#enddate").hide();
            }
        });

        $("#<%= chkEndDate.ClientID %>").click(
            function () {
                $("#enddate").toggle();
            });

    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 140px;
        }

        .style2 {
            width: 2px;
        }

        .style3 {
            width: 130px;
        }

        .style4 {
            width: 1px;
        }
    </style>
</asp:Content>

