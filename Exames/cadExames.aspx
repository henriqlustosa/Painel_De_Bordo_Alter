<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cadExames.aspx.cs" Inherits="Exames_cadExamest"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="stylesheet" href="~/css/style.css" type="text/css" />
<script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>

    <script type="text/javascript">
    
    $(document).ready(function() {

            var $remaining = $('.remaining');


            $('.campoObs').keyup(function() {
                var chars = this.value.length;

                if (chars == '199') {
                    $remaining.text(' 0 caracteres restantes');
                } else if (chars == '198') {
                    remaining = 199 - (chars % 199);

                    $remaining.text(remaining + ' caracterer restante');
                }
                else {
                    remaining = 199 - (chars % 199);

                    $remaining.text(remaining + ' caracteres restantes');
                }
            });
        });
               function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
        
      </script>
    <title></title>
      
    <style type="text/css">
        .auto-style1 {
            width: 207px;
        }
        .auto-style2 {
            height: 59px;
        }
        .auto-style3 {
            width: 207px;
            height: 59px;
        }
    </style>
      
</head>
<body>
 
    <form id="form1" runat="server">
    <div>
    <h2 style="text-align: center">Cadastro de Exames</h2>
        <table>
        <tr>
           
                <td class="auto-style2">
                    Registro Hospitalar:
                </td>
                <td class="auto-style2">
                    <asp:Label ID="lbRh" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="lbSituacao" runat="server" Text="Situação: "></asp:Label>
                </td>
                <td class="auto-style2">
                         <asp:DropDownList ID="ddlSituacao" TabIndex="5" runat="server" DataSourceID="SqlDataSource1" DataTextField="Descricao" DataValueField="Cod_Situacao">
                         
                        </asp:DropDownList>
                         <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SqlServices %>" SelectCommand="SELECT [Cod_Situacao], [Descricao] FROM [Situacao_Exame]"></asp:SqlDataSource>
                </td>
                
            </tr>
            <tr>
                <td>
                    Paciente:
                </td>
                <td>
                    <asp:Label ID="lbPaciente" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:Label ID="lbDtSolicitacao" runat="server" Text="Data da Solicitação:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txbDtSolicitacao" class="campoData1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Solicitante:</td>
                <td>
                    <asp:Label ID="lbSolicitante" runat="server" Text=""></asp:Label>
                </td>
              
            </tr>
            <tr>
                <td>
                    Especialidade:</td>
                <td>
                    <asp:Label ID="lbEspecialidade" runat="server" Text=""></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td>
                    Grupo de Exames:
                </td>
                <td>
                    <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlGrupo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
               <td class="auto-style1">
                    <asp:Label ID="lbDtAgendamento" runat="server" Text="Data do Agendamento :"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txbDtAgendamento" class="campoData2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Exame:
                </td>
                <td>
                    <asp:DropDownList ID="ddlExame" runat="server" Width="500px">
                    </asp:DropDownList>
                </td>
                  <td class="auto-style1">
                    <asp:Label ID="lbFaltou" runat="server" Text="Paciente faltou no exame :"></asp:Label>
                </td>
                <td>
                        <asp:CheckBox ID="chbFaltou" runat="server" />
                </td>
               
            </tr>
           <tr>
            <td valign="top">
                Observação:
            </td>
            <td >
                <asp:TextBox ID="txbObs" Text='<%# Bind("Text") %>' runat="server" class="campoObs"
                    MaxLength="199" onkeyDown="checkTextAreaMaxLength(this,event,'199');" TabIndex="10"
                    Height="79px" TextMode="MultiLine" Width="500px"></asp:TextBox>
            </td>
            <td valign="top" class="auto-style1">
                <p >
                    <span id="remaining" class="remaining">199 caracteres restantes</span>
                </p>
            </td>
        </tr>
               
           
            <tr>
                <td>
                    &nbsp;</td>
                <td colspan="2">
                    <asp:Button ID="btnCadastrar" runat="server" onclick="btnCadastrar_Click" 
                        Text="Cadastrar" />
                  &nbsp;
                     <asp:Button ID="btnAtualizar" runat="server" 
                        Text="Atualizar" OnClick="btnAtualizar_Click" />
                </td>
                
            </tr>
            </table>
    </div>
    <div>
    </div>
   
   
        <asp:Panel ID="Panel5" runat="server" GroupingText="Exames Solicitados">
        <asp:GridView ID="grvExamesSolicitados" runat="server" CellPadding="4" GridLines="Vertical"
             Font-Size="Small" Width="850px" BackColor="White"
            BorderColor="#336666" BorderStyle="Double" BorderWidth="1px" OnSelectedIndexChanged="grvExamesSolicitados_SelectedIndexChanged" AutoGenerateSelectButton="True" OnRowDataBound="grvExamesSolicitados_RowDataBound" >
            <RowStyle BackColor="White" ForeColor="#333333" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel1" runat="server" GroupingText="Exames Agendados">
        <asp:GridView ID="grvExamesMarcados" runat="server" CellPadding="4" GridLines="Vertical"
           Font-Size="Small" Width="850px" BackColor="White"
            BorderColor="#336666" BorderStyle="Double" BorderWidth="1px" OnSelectedIndexChanged="grvExamesMarcados_SelectedIndexChanged" AutoGenerateSelectButton="True" OnRowDataBound="grvExamesMarcados_RowDataBound" >
            <RowStyle BackColor="White" ForeColor="#333333" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
     </form>
</body>
</html>
