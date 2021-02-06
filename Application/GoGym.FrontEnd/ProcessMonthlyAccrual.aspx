<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ProcessMonthlyAccrual.aspx.cs" Inherits="GoGym.FrontEnd.ProcessMonthlyAccrual" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 150px;
        }

        .auto-style3 {
            width: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Monthly Accrual Process
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="auto-style1">
        <tr>
            <td class="auto-style2">Branch</td>
            <td class="auto-style3">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlBranch" Width="250px" /></td>
        </tr>
        <tr>
            <td class="auto-style2">Last Accrual Month / Year</td>
            <td class="auto-style3">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlMonth" Width="100px" />&nbsp;<telerik:RadDropDownList runat="server" ID="ddlYear" Width="100px" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <asp:CheckBox ID="chkExcludeFirstAccrual" runat="server" Text="Exclude first accrual" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <asp:CheckBox ID="chkExcludeFinishAccrual" runat="server" Text="Exclude last accrual" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                &nbsp;<telerik:RadButton ID="btnProcess" runat="server" Text="Process Accrual" OnClientClicking="ProcessConfirm" OnClick="btnProcess_Click" />
                &nbsp;&nbsp;&nbsp;
    <asp:Label runat="server" ID="lblStatus" EnableViewState="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvwMaster" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" OnRowCreated="gvwMaster_RowCreated" SkinID="GridViewDefaultSkin" OnRowDataBound="gvwMaster_RowDataBound" GridLines="Vertical" AllowSorting="True">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" SortExpression="InvoiceID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AccrualDate" HeaderText="AccrualDate" SortExpression="AccrualDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TotalAmount" HeaderText="TotalAmount" SortExpression="TotalAmount" DataFormatString="{0:###,##0.00}"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="TotalAccrualPeriod" HeaderText="TotalAccrualPeriod" SortExpression="TotalAccrualPeriod" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AccrualAmount" HeaderText="AccrualAmount" SortExpression="AccrualAmount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="SumAccrualPeriod" HeaderText="SumAccrualPeriod" SortExpression="SumAccrualPeriod" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="SumAccrualAmount" HeaderText="SumAccrualAmount" SortExpression="SumAccrualAmount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"  />
            <asp:BoundField DataField="CreatedWhen" HeaderText="CreatedWhen" SortExpression="CreatedWhen" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="CreatedWho" HeaderText="CreatedWho" SortExpression="CreatedWho" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hypAccrualHistory" Text="Accrual History" NavigateUrl="#"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:CheckBox ID="chkProcess" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetInvoiceAccrualSummary" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="AccrualMonth" Type="Int32" />
            <asp:Parameter Name="AccrualYear" Type="Int32" />
            <asp:Parameter Name="ExcludeFirstAccrual" Type="Boolean" />
            <asp:Parameter Name="ExcludeFinishAccrual" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <script>
        function ProcessConfirm(sender, args) {
            args.set_cancel(!window.confirm("Are you sure you want to start process?"));
        }
    </script>
</asp:Content>

