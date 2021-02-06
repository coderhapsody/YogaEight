<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="ManageProspectFollowUp.aspx.cs" Inherits="GoGym.FrontEnd.ManageProspectFollowUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Prospect Follow-up <asp:Label ID="lblProspectName" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">       
    <table class="fullwidth">
        <tr>
            <td>Date</td>
            <td>Follow Up via</td>
            <td>Follow Up Result</td>
            <td>Follow Up Outcome</td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtpDate" />
            </td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlFollowUpVia" />
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="txtResult" />
            </td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlOutcome" />
            </td>
            <td>
                <telerik:RadButton runat="server" ID="btnAddFollowUp" Text="Add Follow Up" OnClick="btnAddFollowUp_Click"></telerik:RadButton>
            </td>
            <td></td>
        </tr>
        <tr>
            <td> </td>
            <td><asp:RequiredFieldValidator runat="server" ID="rqvFollowUpVia" ControlToValidate="ddlFollowUpVia" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="<b>Follow Up via</b> must be specified" Text="*"></asp:RequiredFieldValidator> </td>
            <td><asp:RequiredFieldValidator runat="server" ID="rqvResult" ControlToValidate="txtResult" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="<b>Result</b> must be specified" Text="*"></asp:RequiredFieldValidator> </td>
            <td><asp:RequiredFieldValidator runat="server" ID="rqvOutcome" ControlToValidate="ddlOutcome" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="<b>Outcome</b> must be specified" Text="*"></asp:RequiredFieldValidator> </td>
            <td></td>
            <td></td>
        </tr>
    </table>    
    <asp:Label ID="lblStatus" runat="server" EnableViewState="False" CssClass="errorMessage" />
    <br/>
    <telerik:RadGrid ID="grdMaster" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" DataSourceID="sdsMaster" GridLines="None" ShowGroupPanel="True" AutoGenerateColumns="False" GroupingSettings-CaseSensitive="false" OnItemCommand="grdMaster_ItemCommand">
        <GroupingSettings CaseSensitive="False" />
        <ClientSettings AllowDragToGroup="True" AllowColumnsReorder="True" EnableRowHoverStyle="true">
        </ClientSettings>
        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster">
            <Columns>
                <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="ID" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Date" DataType="System.DateTime" FilterControlAltText="Filter Date column" HeaderText="Date" SortExpression="Date" UniqueName="Date" DataFormatString="{0:dd-MMM-yyyy}">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="FollowUpVia" FilterControlAltText="Filter FollowUpVia column" HeaderText="FollowUpVia" SortExpression="FollowUpVia" UniqueName="FollowUpVia">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Result" FilterControlAltText="Filter Result column" HeaderText="Result" SortExpression="Result" UniqueName="Result">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Outcome" FilterControlAltText="Filter Outcome column" HeaderText="Outcome" SortExpression="Outcome" UniqueName="Outcome">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ChangedWhen" DataType="System.DateTime" FilterControlAltText="Filter ChangedWhen column" HeaderText="ChangedWhen" SortExpression="ChangedWhen" UniqueName="ChangedWhen" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn AllowFiltering="false" Groupable="false" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ImageUrl="~/Images/delete-item.png" runat="server" CausesValidation="False" OnClientClicking="DeleteConfirm" ID="btnDeleteRow" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetFollowUpsForProspect" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="" Name="ProspectID" QueryStringField="ProspectID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
