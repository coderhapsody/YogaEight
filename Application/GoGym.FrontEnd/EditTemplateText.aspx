<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="EditTemplateText.aspx.cs" Inherits="GoGym.FrontEnd.EditTemplateText" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .ui-tabs {
            font-size: 100%;
            width: 1050px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Edit Template Text
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1">
        <Tabs>
            <telerik:RadTab runat="server" Text="Presigning Contract" />                            
            <telerik:RadTab runat="server" Text="Terms & Conditions" />
            <telerik:RadTab runat="server" Text="Sales Receipt Footer" />            
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
        <telerik:RadPageView runat="server" ID="RadPageView1">
            <telerik:RadEditor ID="txtPresigningNotice" runat="server" ToolsFile="" Width="100%" />
        </telerik:RadPageView>
        <telerik:RadPageView runat="server" ID="RadPageView2">
            <telerik:RadEditor ID="txtTermsConditions" runat="server" Width="100%" />    
        </telerik:RadPageView>
        <telerik:RadPageView runat="server" ID="RadPageView3">
            <telerik:RadEditor ID="txtReceiptFooterText" runat="server" Width="100%" />
        </telerik:RadPageView>
    </telerik:RadMultiPage>    
    <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
    <br />
    <telerik:RadButton ID="btnSave" runat="server" Text="Save" EnableViewState="false" OnClick="btnSave_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%--    <input type="reset" value="Undo" class="button" />--%>

    <%--<script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#txtPresigningNotice").cleditor({ width: 1000, height: 400 });
            $("#txtTermsConditions").cleditor({ width: 1000, height: 400 });
            $("#txtReceiptFooterText").cleditor({ width: 1000, height: 400 });
        });
    </script>--%>
</asp:Content>

