<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExmUacCd.aspx.cs" Inherits="Exames_ExmUacCd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 align="center">
        Consulta de Exames UAC e CD</h1>
    <div>
        <table>
            <tr>
                <td colspan="2">
                    Registro Hospitalar:
                    <asp:TextBox ID="txbRh" runat="server"></asp:TextBox>
                    <asp:Button ID="btPesq" runat="server" Text="Pesquisar" OnClick="btPesq_Click" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txbRh" ErrorMessage="Apenas números!" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txbRh" ErrorMessage="Campo obrigatório!"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Registro Funcional:
                    <asp:Label ID="lbRF" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Nome:
                    <asp:Label ID="lbNome" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    Idade:
                    <asp:Label ID="lbIdade" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Endereço:
                    <asp:Label ID="lblograd" runat="server"></asp:Label>
                    &nbsp;nº
                    <asp:Label ID="lbnumero" runat="server"></asp:Label>
                    &nbsp;-
                    <asp:Label ID="lbbairro" runat="server"></asp:Label>
                    &nbsp;/
                    <asp:Label ID="lbProcedencia" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Complemento:
                    <asp:Label ID="lbcomplem" runat="server"></asp:Label>
                    &nbsp; -&nbsp; Telefone:
                    <asp:Label ID="lbTel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
     <br />
    
        <asp:Panel ID="Panel3" runat="server" GroupingText="Exames Uac">
            Exames cadastrados nos últimos 6 meses<br />
            <asp:GridView ID="GridView4" runat="server" CellPadding="4" GridLines="Vertical"
                Font-Size="Small" Width="850px" BackColor="White" BorderColor="#336666" BorderStyle="Double"
                BorderWidth="1px">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </asp:Panel>
        <br />
        
        <asp:Panel ID="Panel4" runat="server" GroupingText="Exames CD">
            Exames cadastrados nos últimos 6 meses
            <asp:GridView ID="GridView5" runat="server" CellPadding="4" GridLines="Vertical"
                Font-Size="Small" Width="850px" BackColor="White" BorderColor="#336666" BorderStyle="Double"
                BorderWidth="1px" OnRowDataBound="cor1GridView_RowDataBound">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </asp:Panel>
            
</asp:Content>

