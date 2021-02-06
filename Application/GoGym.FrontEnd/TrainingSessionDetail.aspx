<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPrompt.Master" AutoEventWireup="true" CodeBehind="TrainingSessionDetail.aspx.cs" Inherits="GoGym.FrontEnd.TrainingSessionDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <base target="_self">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="server">
    Training Session Detail for <asp:Label ID="lblCustomer" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="server">
    <telerik:RadGrid runat="server" id="grdSessionDetail" AllowPaging="True" AllowSorting="True" DataSourceID="sdsQuotaDetail" >
        <ClientSettings AllowDragToGroup="True">
        </ClientSettings>
        <MasterTableView AutoGenerateColumns="False" DataSourceID="sdsQuotaDetail">
            <Columns>
                <telerik:GridBoundColumn DataField="RefNo" FilterControlAltText="Filter RefNo column" HeaderText="RefNo" ReadOnly="True" SortExpression="RefNo" UniqueName="RefNo">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column" HeaderText="ItemName" SortExpression="ItemName" UniqueName="ItemName">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Name" SortExpression="Name" UniqueName="Name">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="When" DataType="System.DateTime" FilterControlAltText="Filter When column" HeaderText="When" SortExpression="When" UniqueName="When">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Usage" DataType="System.Int16" FilterControlAltText="Filter Usage column" HeaderText="Usage" SortExpression="Usage" UniqueName="Usage">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ClerkName" FilterControlAltText="Filter ClerkName column" HeaderText="ClerkName" ReadOnly="True" SortExpression="ClerkName" UniqueName="ClerkName">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TrainerName" FilterControlAltText="Filter TrainerName column" HeaderText="TrainerName" ReadOnly="True" SortExpression="TrainerName" UniqueName="TrainerName">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Notes" FilterControlAltText="Filter Notes column" HeaderText="Notes" SortExpression="Notes" UniqueName="Notes">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column" HeaderText="Type" ReadOnly="True" SortExpression="Type" UniqueName="Type">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
        
    </telerik:RadGrid>
    <br/>
    <button id="btnClose">Close This Window</button>
    <asp:SqlDataSource ID="sdsQuotaDetail" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetSessionQuotaDetail" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="QuotaID" QueryStringField="QuotaID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <script>
        $(function () {            
            $("#btnClose").click(function() {
                window.close();
            });
        });
    </script>
</asp:Content>
