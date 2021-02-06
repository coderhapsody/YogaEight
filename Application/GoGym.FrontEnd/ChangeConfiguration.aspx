<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="ChangeConfiguration.aspx.cs" Inherits="GoGym.FrontEnd.ChangeConfiguration" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Change Configuration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <telerik:RadTabStrip ID="tabConfiguration" runat="server" MultiPageID="mpgConfiguration">
        <Tabs>
            <telerik:RadTab runat="server" Text="Check-In Alerts" />
            <telerik:RadTab runat="server" Text="Security" />
            <telerik:RadTab runat="server" Text="User Interface" />
            <telerik:RadTab runat="server" Text="Roles" />
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="mpgConfiguration" SelectedIndex="0">
        <telerik:RadPageView runat="server" ID="RadPageView1">
            <table class="fullwidth">
                <tr>
                    <td style="width: 125px">Birthday Alert</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkBirthdayAlert" /></td>
                </tr>
                <tr>
                    <td style="width: 125px">Contract Not Active</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkContractNotActive" /></td>
                </tr>
                <tr>
                    <td style="width: 125px">Contract Not Paid</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkContractNotPaid" /></td>
                </tr>
                <tr>
                    <td style="width: 125px">Credit Card Expired</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkCreditCardExpired" /></td>
                </tr>
                <tr>
                    <td style="width: 125px">Credit Card Expiring</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkCreditCardExpiring" /></td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server" ID="RadPageView2">
            <div class="tableContainer">
                <div class="tableRow">
                    <div class="tableCol" style="width: 200px;">Enable Log-in History</div>
                    <div class="tableCol" style="width: 5px;">:</div>
                    <div class="tableCol">
                        <asp:CheckBox runat="server" ID="chkLoginHistory" />
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server" ID="RadPageView3">
            <div class="tableContainer">
                <div class="tableRow">
                    <div class="tableCol" style="width:200px;">Grid roll over color</div>
                    <div class="tableCol" style="width:5px;">:</div>
                    <div class="tableCol"><telerik:RadColorPicker runat="server" ShowIcon="true" ID="colGridRollOver"></telerik:RadColorPicker> </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server" ID="RadPageView4">
            <div class="tableContainer">
                <div class="tableRow">
                    <div class="tableCol" style="width:200px;">Role for Trainer</div>
                    <div class="tableCol" style="width:5px;">:</div>
                    <div class="tableCol"><telerik:RadDropDownList runat="server" ID="ddlRoleTrainer"></telerik:RadDropDownList>  </div>
                </div>
                <div class="tableRow">
                    <div class="tableCol" style="width:200px;">Role for Sales</div>
                    <div class="tableCol" style="width:5px;">:</div>
                    <div class="tableCol"><telerik:RadDropDownList runat="server" ID="ddlRoleSales"></telerik:RadDropDownList>  </div>
                </div>
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    <br />
    <asp:Label ID="lblStatus" runat="server" EnableViewState="False" />
    <br />
    <br />
    <telerik:RadButton ID="btnSave" Text="Save" runat="server" EnableViewState="False" OnClick="btnSave_Click" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
