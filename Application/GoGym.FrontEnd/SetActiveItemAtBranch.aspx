<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="SetActiveItemAtBranch.aspx.cs" Inherits="GoGym.FrontEnd.SetActiveItemAtBranch"  StyleSheetTheme="Workspace"%>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Setting Active Items at Branch
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">    
    <table class="style1">
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">
                            Select Item</td>
                        <td class="style3">
                            :
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlItem" runat="server" AutoPostBack="True" DropDownHeight="250px" Width="250px"
                                OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">                    
                    <ContentTemplate>
                        <asp:CheckBoxList ID="cblBranches" runat="server" RepeatColumns="3" 
                            RepeatDirection="Horizontal">
                            
                        </asp:CheckBoxList>                        
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                        
                        <p>
                    <br />
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" 
                        EnableViewState="False" onclick="btnSave_Click" />
                </p>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlItem" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </td>
        </tr>
    </table>    
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 130px;
        }
        .style3
        {
            width: 1px;
        }
    </style>
</asp:Content>


