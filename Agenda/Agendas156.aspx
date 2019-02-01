<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Agendas156.aspx.cs" Inherits="Agenda_Agendas156"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">
        .header
        {
            background-color: #3E3E3E;
            font-weight: bold;
            font-family: Calibri;
            color: White;
            text-align: center;
        }
        .header2
        {
            background-color: #336666;
            font-weight: bold;
            font-style: inherit;
            font-family: Courier New;
            color: White;
            text-align: center;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <div style="text-align: center">
            <h1>
                Agenda Futura de Consultas Ambulatoriais</h1>
        </div>
        <div>
       
       <p>O envio da planilha para a Central 156 será necessário criar a agenda no sistema Hospub e bloquear, a agenda deve estar com todas as consultas vagas</p>
       <p>Para exportar as agendas para Excel selecione a especialidade e escolha o profissional.</p>
       <p>Após exportar e enviar para a Central 156 debloquei a agenda no Hospub.</p>
        </div>
        <div align="center">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Especialidade/Sub:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEspecialidades" runat="server" Font-Names="Courier New">
                        </asp:DropDownList>
                        <asp:Button ID="btPesquisarProfissional" runat="server" OnClick="btPesquisarProfissional_Click"
                            Text="Pesquisar" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="lbcodescala" runat="server" Visible="false"></asp:Label>
        <br />
        <table style="width: 100%;">
            <tr>
                <td valign="top" width="500px" height="100px">
                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" Font-Size="Small" GridLines="Vertical"
                        Width="800px" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="1px"
                        AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                        Font-Names="Courier New" HorizontalAlign="Center">
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" ForeColor="White" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    
                </td>
            </tr>
           
            <tr>
                <td valign="top">
                
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Vagas Futuras (am11)" HorizontalAlign="Center">
                        <asp:Button ID="btnExportarExcel" runat="server" Text="Exporta Excel" OnClick="btnExportarExcel_Click" />
                        <br />
                        <asp:GridView ID="GridView3" runat="server" CellPadding="4" Font-Size="Small" GridLines="Horizontal"
                            Width="800px" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="1px"
                            Font-Names="Courier New" HorizontalAlign="Center">
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

