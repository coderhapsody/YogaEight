<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterWorkspace.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoGym.FrontEnd.Default" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="tableContainer">
        <div class="tableRow">
            <div class="tableCol" style="width: 280px; text-align: left; overflow: auto;">
                <table class="fullwidth">
                    <tr>
                        <td class="fieldHeader">Client Browser</td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="lblBrowser" runat="server" EnableViewState="false" /></td>
                    </tr>
                    <tr>
                        <td class="style2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="fieldHeader">Database Server</td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="lblDatabaseServer" runat="server" EnableViewState="False" /></td>
                    </tr>
                    <tr>
                        <td class="style2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="fieldHeader">User <%= Page.User.Identity.Name %> is active at branch:</td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:BulletedList ID="bulBranch" runat="server" EnableViewState="false"
                                BulletStyle="CustomImage" BulletImageUrl="~/images/user_green.png" />
                        </td>
                    </tr>

                    <tr>
                        <td class="style2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="fieldHeader">User <%= Page.User.Identity.Name %> is allowed at branches:</td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:BulletedList ID="bulAllowedBranch" runat="server" EnableViewState="false"
                                BulletStyle="CustomImage" BulletImageUrl="~/images/user_green.png" />
                        </td>
                    </tr>
                </table>


            </div>
            <div class="tableCol">
                <asp:HyperLink runat="server" ID="hypAlerts" Text="Go to Alerts" NavigateUrl="ManageAlerts.aspx"></asp:HyperLink>
                <asp:UpdatePanel runat="server" ID="updScheduler">
                    <ContentTemplate>
                        <telerik:RadScheduler ID="RadScheduler1" runat="server" DataEndField="EndDate" DataKeyField="ID"
                            DataStartField="StartDate" DataSubjectField="Subject" SelectedView="MonthView" ReadOnly="True"
                            DataDescriptionField="Description" FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" Width="100%"
                            AdvancedForm-EnableCustomAttributeEditing="true" OnClientAppointmentClick="AppointmentClick"
                            OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
                            EnableCustomAttributeEditing="True">
                            <AdvancedForm Modal="true" EnableCustomAttributeEditing="True"></AdvancedForm>
                            <MonthView UserSelectable="true"></MonthView>
                            <AppointmentContextMenuSettings EnableDefault="False" />
                            <TimeSlotContextMenuSettings EnableDefault="True" />
                            <AppointmentTemplate>
                                <div class="rsAptSubject">
                                    <%# Eval("Subject") %>
                                </div>
                                <br />
                                <%# Eval("Description") %>
                            </AppointmentTemplate>
                        </telerik:RadScheduler>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 97px;
        }

        .style2 {
            width: 192px;
        }

        .RadScheduler .rsAptSubject {
            text-align: left;
            padding: 4px 0 1px;
            margin: 0 0 3px;
            border-bottom: 1px solid #99DEFD;
            width: 100%;
        }
    </style>

    <script>
        function AppointmentClick(sender, eventArgs) {
            var apt = eventArgs.get_appointment();
            var s = apt.get_subject() + '\n\n' + apt.get_description();
            alert(s);
        }

    </script>
</asp:Content>


