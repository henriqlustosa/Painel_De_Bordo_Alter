<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rh.aspx.cs" Inherits="Pesquisa_Rh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .Hide
        {
            display: none;
        }
        .style2
        {
            text-decoration: underline;
            font-weight: bold;
        }
        .style3
        {
            height: 29px;
        }
        .style1
        {
            height: 23px;
        }
        .style4
        {
            width: 33px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 align="center">Pesquisa de Solicitações por Registro Hospitalar</h1>
    <div>
        
        
        
        <br />
    
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbUser" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Registro Hospitalar:
                    <asp:TextBox ID="txbRh" runat="server" ></asp:TextBox>
                    <asp:Button ID="btPesq" runat="server" Text="Pesquisar" OnClick="btPesq_Click" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txbRh"
                        ErrorMessage="Apenas números!" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txbRh"
                        ErrorMessage="Campo Obrigatório!"></asp:RequiredFieldValidator>
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
            </table>
        
        
        
        <br />
        <br />
        <table>
            <tr>
                <td>
                    Data da Solicitação:
                </td>
                <td colspan="3" class="style4">
                    <asp:TextBox ID="txbdtSol" class="campoData" runat="server"></asp:TextBox>
                </td>
                <td>
                    Código:
                </td>
                <td>
                    <asp:Label ID="lbcod" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Especialidade:
                </td>
                <td colspan="3" class="style4">
                    <asp:DropDownList ID="ddlEspec" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    Solicitante:
                </td>
                <td>
                    <asp:DropDownList ID="ddlSol" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <span class="style2">Procedimentos Solicitados</span>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:CheckBox ID="chbRetorno" runat="server" Text="Encaminhamento/Retorno" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:CheckBox ID="chbRegul" runat="server" Text="Regulação" />
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Quantidade
                </td>
                <td class="style3">
                    <asp:TextBox ID="txbqtd" runat="server" Width="50px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txbqtd" 
                        ErrorMessage="O campo [Quantidade] não foi preenchido." 
                        ValidationGroup="cadastro">*</asp:RequiredFieldValidator>
                </td>
                <td class="style3">
                    Situação:<asp:DropDownList ID="ddlSituacao" runat="server">
                        <asp:ListItem Selected="True">Enc. UAC</asp:ListItem>
                        <asp:ListItem>Recebido UAC</asp:ListItem>
                        <asp:ListItem>Enc. GTA</asp:ListItem>
                        <asp:ListItem>Recebido GTA</asp:ListItem>
                        <asp:ListItem>Cancelada</asp:ListItem>
						<asp:ListItem>Ativo HSPM</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style3" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Marcada:
                </td>
                <td>
                    <asp:DropDownList ID="ddlMarcada" runat="server">
                        <asp:ListItem Selected="True" Value="Não">Não</asp:ListItem>
                        <asp:ListItem Value="Sim">Sim</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="4">
                    Consulta:<asp:TextBox ID="txbConsulta" runat="server" class="campoConsulta" Width="50px"></asp:TextBox>
                    &nbsp;Data:<asp:TextBox ID="txbDtCon" runat="server" class="campoData" Width="80px"></asp:TextBox>
                    &nbsp;Hora:<asp:TextBox ID="txbHoraCon" runat="server" class="campoHora" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Observação:
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txbObs" runat="server" Height="79px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                </td>
            </tr>
                <table>
        <tr>
            <td width="77">
                Telefone 1:
            </td>
            <td>
                <asp:TextBox ID="txbTel1" class="campoTel" TabIndex="11" runat="server"></asp:TextBox>
            </td>
            <td width="77">
                Telefone 2:
            </td>
            <td>
                <asp:TextBox ID="txbTel2" class="campoTel" TabIndex="11" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td width="77">
                Telefone 3:
            </td>
            <td>
                <asp:TextBox ID="txbTel3" class="campoTel" TabIndex="11" runat="server"></asp:TextBox>
            </td>
            <td width="77">
                Telefone 4:
            </td>
            <td>
                <asp:TextBox ID="txbTel4" class="campoTel" TabIndex="11" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
              
               
              
                <td>
                    <asp:Button ID="Button2" runat="server" Text="Atualizar" OnClick="Button2_Click" />
                </td>
                  <td>
                    
                      &nbsp;</td>
            </tr>
        </table>
        
        
        
        <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False" BackColor="White"
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                DataSourceID="SqlDataSource1" GridLines="Vertical" Style="font-size: x-small"
                Width="850px" OnSelectedIndexChanged="GridView6_SelectedIndexChanged" AutoGenerateSelectButton="True">
                <rowstyle backcolor="White" forecolor="#333333" />
                <columns>
                            <asp:BoundField DataField="cod" HeaderText="cod" InsertVisible="False" ReadOnly="True"
                                SortExpression="cod"  ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                            <asp:BoundField DataField="data" HeaderText="data" SortExpression="data" />
                            <asp:BoundField DataField="especialidade" HeaderText="especialidade" SortExpression="especialidade" />
                            <asp:BoundField DataField="solicitante" HeaderText="solicitante" SortExpression="solicitante" />
                            <asp:BoundField DataField="retorno" HeaderText="retorno" SortExpression="retorno" />
                            <asp:BoundField DataField="regulacao" HeaderText="regulacao" SortExpression="regulacao" />
                            <asp:BoundField DataField="qtdExames" HeaderText="qtdExames" SortExpression="qtdExames" />
                            <asp:BoundField DataField="situacao" HeaderText="situacao" SortExpression="situacao" />
                            <asp:BoundField DataField="marcada" HeaderText="marcada" SortExpression="marcada" />
                            <asp:BoundField DataField="consulta" HeaderText="consulta" SortExpression="consulta" />
                            <asp:BoundField DataField="dtCon" HeaderText="Data" SortExpression="dtCon" />
                            <asp:BoundField DataField="hrCon" HeaderText="Hora" SortExpression="hrCon" />
                            <asp:BoundField DataField="telefone" HeaderText="telefone" SortExpression="telefone" />
                            <asp:BoundField DataField="obs" HeaderText="obs" SortExpression="obs" />
                            <asp:BoundField DataField="usuario" HeaderText="usuario" SortExpression="usuario" />
                        </columns>
                <footerstyle backcolor="White" forecolor="#333333" />
                <pagerstyle backcolor="#336666" forecolor="White" horizontalalign="Center" />
                <selectedrowstyle backcolor="#339966" font-bold="True" forecolor="White" />
                <headerstyle backcolor="#336666" font-bold="True" forecolor="White" />
            </asp:GridView>
        
        
        
        <hr />
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SqlServices %>"
                SelectCommand="SELECT [cod],CONVERT(varchar,data, 103) as data, [especialidade], [solicitante], [retorno],[regulacao],[qtdExames], [situacao], [marcada], [consulta],CONVERT(varchar,dtCon,103) as dtCon,[hrCon], [telefone], [obs], [usuario] FROM [Geral_Treina].[dbo].[Fila] WHERE ([rh] = @rh) ORDER BY [cod] desc">
                <selectparameters>
                            <asp:ControlParameter ControlID="txbRh" Name="rh" PropertyName="Text" Type="String" />
                        </selectparameters>
            </asp:SqlDataSource>
        
    </div>
</asp:Content>

