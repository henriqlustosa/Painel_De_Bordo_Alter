<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgendamentoRetorno.aspx.cs"
    Inherits="Consulta_AgendamentoRetorno" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Agendamento Retorno</title>
    <link rel="stylesheet" href="~/css/style.css" type="text/css" />

    <script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function() {

            //Input Mask for date of birth or date in general
            $(".campoData").mask("99/99/9999");
            //Input Mask for date of birth or date in general
            $(".campoHora").mask("99:99");
            //Input Mask for number consult
            $(".NumConsulta").mask("9999-9");
        });
    </script>

</head>
<body>
    <form id="form2" runat="server">
    <div>
        <h2 style="text-align: center">
            Agendamento de Consultas </h2>
        <table>
            <tr>
                <td>
                    Registro Hospitalar:
                </td>
                <td>
                    <asp:Label ID="lbRh" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Paciente:
                </td>
                <td>
                    <asp:Label ID="lbPaciente" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Especialidade:
                </td>
                <td>
                    <asp:Label ID="lbEspecialidade" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Solicitante:
                </td>
                <td>
                    <asp:Label ID="lbSolicitante" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Agendamento" Height="215px" 
        Width="677px">
        <table>
            <tr>
                <td>
                    Nº da Consulta:
                </td>
                <td>
                    <asp:TextBox ID="txbNumConsulta" class="NumConsulta" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txbNumConsulta" ErrorMessage="RequiredFieldValidator">*Numero da consulta não foi preenchido</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Data:
                </td>
                <td>
                    <asp:TextBox ID="txbData" class="campoData" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Hora:
                </td>
                <td>
                    <asp:TextBox ID="txbHora" class="campoHora" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Marcada:
                </td>
                <td>z
                    <asp:DropDownList ID="ddlMarcada" runat="server">
                        <asp:ListItem>Sim</asp:ListItem>
                        <asp:ListItem Selected="True">Não</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rdOpcao" runat="server">
                    <asp:ListItem Text ="Ativo HSPM" Value = 1 ></asp:ListItem>  
                    <asp:ListItem Text ="Cancelado" Value = 2 ></asp:ListItem>  
                    </asp:RadioButtonList>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" OnClick="btnCadastrar_Click" />
                </td>
                <td>
                    <asp:Button ID="btnAtualizar" runat="server" Text="Atualizar" 
                        onclick="btnAtualizar_Click"  />
                </td>
            </tr>
        </table>
    </asp:Panel>
   <br />
    </form>
</body>
</html>
