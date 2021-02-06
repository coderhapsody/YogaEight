<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="UpdateAutoNumber.aspx.cs" Inherits="GoGym.FrontEnd.UpdateAutoNumber" StyleSheetTheme="Workspace" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Update Auto Number
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">    
    <table class="style1">
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">
                            Select Branch
                        </td>
                        <td class="style3">
                            :
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Width="250px"
                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
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
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="false"
                            Width="450px" OnRowCreated="gvwMaster_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="FormCode" HeaderText="FormCode" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="10px" />
                                <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="300px" />
                                    <asp:BoundField DataField="Year" HeaderText="Year" HeaderStyle-HorizontalAlign="Left"
                                     />
                                <asp:TemplateField HeaderText="Last Number" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLastNumber" runat="server" MaxLength="5" CssClass="textbox" Width="100px"
                                            Text='<%# Eval("LastNumber") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlBranch" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <p>
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" SkinID="SaveButton" 
                        EnableViewState="False" onclick="btnSave_Click" />
                </p>
            </td>
        </tr>
    </table>
    <script>
        Sys.Application.add_load(pageLoad);


        function pageLoad() {
            $get('<%= btnSave.ClientID %>').disabled = $get('<%= ddlBranch.ClientID %>').selectedIndex == 0;
        }

    </script>
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
