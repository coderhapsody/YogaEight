<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="SetActiveClassRoomAtBranch.aspx.cs" Inherits="GoGym.FrontEnd.SetActiveClassRoomAtBranch" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Setting Active Rooms at Branch
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManagerProxy ID="scmScriptManager" runat="server" />
    <table class="style1">
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">Select Class</td>
                        <td class="style3">:
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
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DataList ID="dlsBranches" runat="server">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBranch" runat="server" Text='<%# Eval("Name") %>' Value='<%# Eval("ID") %>' />
                                <asp:TextBox ID="txtCapacity" runat="server" CssClass="textbox" Width="50px" />
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                        <p>
                            <br />
                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" EnableViewState="False" OnClick="btnSave_Click" />
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
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 130px;
        }

        .style3 {
            width: 1px;
        }
    </style>


</asp:Content>


