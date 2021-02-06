<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Questionaire.ascx.cs" Inherits="GoGym.FrontEnd.UserControls.Questionaire" %>
<asp:GridView runat="server" id="gvwQuestionaire" AutoGenerateColumns="False" Width="600px" OnRowCreated="gvwQuestionaire_RowCreated">
    <Columns>
        
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="Question" HeaderText="Question" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkAnswer" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        
    </Columns>
</asp:GridView>