<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="TrainingSessionAdjustment.aspx.cs" Inherits="GoGym.FrontEnd.TrainingSessionAdjustment" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Training Session
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <div style="width: 100%; text-align: center;">
        <asp:HiddenField runat="server" ID="hidBranchID" Value="" />
        <fieldset>
            <legend id="branchName"></legend>
            <asp:UpdatePanel runat="server" ID="updSession">
                <ContentTemplate>

                    <table class="fullwidth">
                        <tr>
                            <td class="centeredRow">
                                <telerik:RadTextBox ID="txtBarcode" runat="server" Font-Size="24pt" Height="60px" Width="300px" SelectionOnFocus="SelectAll" />
                            </td>
                        </tr>
                        <tr>
                            <td class="centeredRow">
                                <telerik:RadButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"></telerik:RadButton>
                            </td>
                        </tr>

                        <tr>
                            <td>&nbsp;</td>
                        </tr>

                        <tr>
                            <td class="centeredRow">
                                <asp:Label ID="lblCustomerInfo" runat="server" Font-Size="18pt" /></td>
                        </tr>
                        <tr>
                            <td class="centeredRow">
                                <asp:Label ID="lblResult" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                                <br/><br/>
                                <asp:GridView ID="gvwSession" runat="server" AutoGenerateColumns="False" SkinID="GridViewDefaultSkin" Width="100%" OnRowCreated="gvwSession_RowCreated" OnRowDataBound="gvwSession_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="QuotaID" HeaderText="QuotaID" />
                                        <asp:BoundField DataField="ItemID" HeaderText="ItemID" />
                                        <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" />
                                        <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" />
                                        <asp:BoundField DataField="Balance" HeaderText="Balance" />

                                        <asp:TemplateField HeaderText="Operation">
                                            <ItemTemplate>
                                                <telerik:RadDropDownList runat="server" ID="ddlType" Width="80px">
                                                    <Items>
                                                        <telerik:DropDownListItem runat="server" Text="Sub" Value="Sub" />
                                                        <telerik:DropDownListItem runat="server" Text="Add" Value="Add" />
                                                    </Items>
                                                </telerik:RadDropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Usage">
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox runat="server" ID="txtQtySession" Value="0" NumberFormat-DecimalDigits="0" MinValue="0" MaxValue="10" Width="50px" ShowSpinButtons="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Trainer">
                                            <ItemTemplate>
                                                <telerik:RadDropDownList runat="server" ID="ddlTrainer"></telerik:RadDropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        
                                        <asp:TemplateField HeaderText="Notes">
                                            <ItemTemplate>
                                                <telerik:RadTextBox runat="server" ID="txtNotes"></telerik:RadTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ID="hypQuotaDetail" NavigateUrl="#" Text="Detail"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="centeredRow">
                                <telerik:RadButton runat="server" ID="btnProcess" Text="Process" OnClick="btnProcess_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </fieldset>
    </div>


    <telerik:RadWindow runat="server" ID="wndSelectBranch" Title="Select Branch" Width="500px" Height="200px" Modal="true" VisibleStatusbar="False" Behaviors="Close" InitialBehaviors="Close" ReloadOnShow="True" OnClientBeforeClose="SelectBranchWindowBeforeClose">
        <ContentTemplate>
            <div class="tableContainer windowContent">
                <div class="tableRow">
                    <div class="tableCol">
                        <telerik:RadDropDownList runat="server" ID="ddlBranch" Width="250px"></telerik:RadDropDownList>
                    </div>
                </div>
                <div class="tableRow">
                    <div class="tableCol">&nbsp;</div>
                </div>
                <div class="tableRow">
                    <div class="tableCol">
                        <telerik:RadButton ID="btnSelect" runat="server" Text="Select" OnClientClicking="SelectBranchClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    <style>
        .centeredRow {
            text-align: center;
        }

        .windowContent {
            margin-top: 30px;
        }
    </style>

    <script>
        $(function () {
            var branchID = $get('<%=hidBranchID.ClientID%>').value;
            if (branchID === "") {
                var wndBranch = $find('<%=wndSelectBranch.ClientID%>');
                wndBranch.show();
            }
        });

        function SelectBranchClick(sender, args) {
            sender.set_autoPostBack(false);

            var ddlBranch = $find('<%=ddlBranch.ClientID%>');
            $get('<%=hidBranchID.ClientID%>').value = ddlBranch.get_selectedItem().get_value();
            $get('branchName').innerHTML = ddlBranch.get_selectedItem().get_text();

            var wndBranch = $find('<%=wndSelectBranch.ClientID%>', document.all);
            wndBranch.close("branchSelected");
        }

        function SelectBranchWindowBeforeClose(sender, args) {
            if (args.get_argument() != "branchSelected") {
                args.set_cancel(true);
            }
        }
    </script>
</asp:Content>
