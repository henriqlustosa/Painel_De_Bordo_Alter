<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Consultas.aspx.cs" Inherits="Consulta_Consultas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1 align="center">
        Consulta do Paciente</h1>
    <div>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbUser" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Registro Hospitalar:
                    <asp:TextBox ID="txbRh" runat="server"></asp:TextBox>
                    <asp:Button ID="btPesq" runat="server" Text="Pesquisar" OnClick="btPesq_Click" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txbRh"
                        ErrorMessage="Apenas números!" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txbRh"
                        ErrorMessage="Campo Obrigatório!"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Registro Funcional:
                    <asp:Label ID="lbRF" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    Nome:
                    <asp:Label ID="lbNome" runat="server" Text=""></asp:Label>
                </td>
                <td class="style1">
                    Idade:
                    <asp:Label ID="lbIdade" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style1" colspan="2">
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
            <tr>
                <td colspan="2">
                    </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div>
        <asp:Panel ID="Panel7" runat="server" GroupingText="Histórico de Consultas">
            Últimos 6 meses<br />
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" Font-Size="Small" GridLines="Vertical"
                Width="850px" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="1px"
                AllowSorting="True">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </asp:Panel>
    </div>
    <br />
    <div>
        <asp:Panel ID="Panel1" runat="server" GroupingText="Consultas Marcadas">
            <asp:GridView ID="GridView2" runat="server" CellPadding="4" GridLines="Vertical"
                Font-Size="Small" Width="850px" BackColor="White" BorderColor="#336666" BorderStyle="Double"
                BorderWidth="1px">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </asp:Panel>
    </div>
    <br />
    <div>
        <asp:Panel ID="Panel5" runat="server" GroupingText="Agenda por Demanda">
            <asp:GridView ID="GridView3" runat="server" CellPadding="4" GridLines="Vertical"
                Font-Size="Small" Width="850px" BackColor="White" BorderColor="#336666" BorderStyle="Double"
                BorderWidth="1px">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>

