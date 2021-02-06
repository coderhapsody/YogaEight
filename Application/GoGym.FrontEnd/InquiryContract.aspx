<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="InquiryContract.aspx.cs" Inherits="GoGym.FrontEnd.InquiryContract"  StylesheetTheme="Workspace" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Inquiry Contract
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">
                Branch
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlFindBranch" runat="server" Width="250px" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                Contract No.
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadTextBox ID="txtFindContractNo" runat="server" MaxLength="50" Width="120px" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                Date
            </td>
            <td class="style2">
                :
            </td>
            <td>
                From
                <telerik:RadDatePicker runat="server" ID="calFindDateFrom" />
                &nbsp;&nbsp;&nbsp;&nbsp; To
                <telerik:RadDatePicker runat="server" ID="calFindDateTo"/>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Customer Code
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadTextBox ID="txtFindBarcode" runat="server" MaxLength="50" Width="120px" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                Package
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlFindPackage" runat="server"  />
            </td>
        </tr>
        <tr>
            <td class="style1">
                Billing Type
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlFindBillingType" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                Status
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <telerik:RadDropDownList ID="ddlFindStatus" runat="server"  />
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                <telerik:RadButton ID="btnRefresh" runat="server" EnableViewState="false" OnClick="btnRefresh_Click" Text="Refresh" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin"  
        AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
        Width="100%" DataSourceID="sdsMaster" onrowcreated="gvwMaster_RowCreated" 
        onrowdatabound="gvwMaster_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" SortExpression="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" SortExpression="CustomerName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="PackageName" HeaderText="Package" SortExpression="PackageName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="BillingTypeDescription" HeaderText="BillingType" SortExpression="BillingTypeDescription" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate" SortExpression="PurchaseDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="ExpiredDate" HeaderText="ExpiredDate" SortExpression="ExpiredDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="NextDuesDate" HeaderText="NextDuesDate" SortExpression="NextDuesDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="ActiveDate" HeaderText="ActiveDate" SortExpression="ActiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />                        
            <asp:BoundField DataField="VoidDate" HeaderText="VoidDate" SortExpression="VoidDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />            
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypPrint" runat="server" Text="Print" NavigateUrl="#" ImageUrl="~/images/PrintHS.png"
                        ToolTip="Print" />                    
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" 
        ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
        onselecting="sdsMaster_Selecting" SelectCommand="proc_InquiryContract" 
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="ContractNo" Type="String" />
            <asp:Parameter Name="DateFrom" Type="String" />
            <asp:Parameter Name="DateTo" Type="String" />
            <asp:Parameter Name="CustomerCode" Type="String" />
            <asp:Parameter Name="PackageID" Type="Int32" />
            <asp:Parameter Name="BillingTypeID" Type="Int32" />
            <asp:Parameter Name="Status" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1
        {
            width: 130px;
        }
        .style2
        {
            width: 2px;
        }

        .RadPicker {            
            position: static; 
        }        

    </style>
</asp:Content>

