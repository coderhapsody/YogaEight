<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="PromptCustomer.aspx.cs" Inherits="GoGym.FrontEnd.PromptCustomer" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Look Up Customers
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">    
    <table style="width: 100%">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td class="auto-style1">
                            Branch</td>
                        <td style="width: 1px">
                            :</td>
                        <td>
                            <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px" />                            
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Barcode</td>
                        <td style="width: 1px">
                            :
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFindCustomerCode" runat="server" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Name</td>
                        <td style="width: 1px">
                            :
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFindCustomerName" runat="server" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Parent Name</td>
                        <td style="width: 1px">
                            :</td>
                        <td>
                            <telerik:RadTextBox ID="txtFindParentName" runat="server" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Phone No. (Customer/Parent) </td>
                        <td style="width: 1px">
                            :</td>
                        <td>
                            <telerik:RadTextBox ID="txtFindPhoneNo" runat="server" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            &nbsp;
                        </td>
                        <td style="width: 1px">
                            &nbsp;
                        </td>
                        <td>
                            <telerik:RadButton ID="btnRefresh" runat="server" CommandArgument="Refresh" CommandName="PromptEmployee"
                                EnableViewState="False" Text="Refresh" 
                                onclick="btnRefresh_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvwPrompt" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False"
                    Width="100%" AllowSorting="True"                    
                    AllowPaging="True" DataKeyNames="ID" DataSourceID="sdsPrompt" 
                    onrowcreated="gvwPrompt_RowCreated" onrowdatabound="gvwPrompt_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID"
                            HeaderStyle-HorizontalAlign="Left" InsertVisible="False" ReadOnly="True" />
                        <asp:BoundField DataField="Barcode" SortExpression="Barcode" HeaderText="Barcode"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name"
                            HeaderStyle-HorizontalAlign="Left" ReadOnly="True" />
                        <asp:BoundField DataField="Branch" SortExpression="Branch" HeaderText="Branch"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status" 
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Package" HeaderText="Package" 
                            SortExpression="Package" />
                                                    <asp:TemplateField>
                            <ItemStyle Width="10px" />
                            <ItemTemplate>
                                <asp:HyperLink ID="hypSelect" runat="server" NavigateUrl="#">Select</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        .: No Data :.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="sdsPrompt" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
                    SelectCommand="proc_GetAllCustomers" SelectCommandType="StoredProcedure" 
                    onselecting="sdsPrompt_Selecting">
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
</asp:Content>

<asp:Content ID="Content4" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .auto-style1 {
            width: 200px;
        }
    </style>
</asp:Content>