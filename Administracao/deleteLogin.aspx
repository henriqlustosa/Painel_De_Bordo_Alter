<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="deleteLogin.aspx.cs" Inherits="Administracao_deleteLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>Excluir Conta de Login</h2>
<br />
<table>
<tr>
<td>Usuário:</td><td>
    <asp:TextBox ID="txbUser" runat="server" Enabled="False"></asp:TextBox></td>
</tr>
<tr>
<td>User Id:</td><td>
    <asp:TextBox ID="txbUserId" runat="server" Width="269px" Enabled="False"></asp:TextBox></td>
</tr>
<tr>
<td></td><td>
    <asp:Button ID="btDelete" runat="server" Text="Excluir" 
        onclick="btDelete_Click" /></td>
</tr>

</table>

<hr />
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" 
        AllowPaging="True" AllowSorting="True">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="UserId, UserName" DataNavigateUrlFormatString="deleteLogin.aspx?UserId={0}&UserName={1}"
                HeaderText="Detalhes" Text="Selecione" />
            <asp:BoundField DataField="UserName" HeaderText="Usuário" 
                SortExpression="UserName" />
            <asp:BoundField DataField="LastActivityDate" HeaderText="Última atividade" 
                SortExpression="LastActivityDate" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Bloqueado" 
                SortExpression="IsLockedOut" />
            <asp:BoundField DataField="CreateDate" HeaderText="Data de Criação" 
                SortExpression="CreateDate" />
            <asp:BoundField DataField="LastLoginDate" HeaderText="Último login" 
                SortExpression="LastLoginDate" />
                <asp:BoundField DataField="UserId" HeaderText="Id Usuário" 
                SortExpression="UserId" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SqlServices %>"
        SelectCommand="SELECT aspnet_Users.UserName, aspnet_Users.LastActivityDate, aspnet_Membership.Email, aspnet_Membership.IsLockedOut, aspnet_Membership.CreateDate, aspnet_Membership.LastLoginDate, aspnet_Users.UserId FROM aspnet_Membership INNER JOIN aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId order by aspnet_Users.UserName">
    </asp:SqlDataSource>
</asp:Content>

