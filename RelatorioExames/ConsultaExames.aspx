<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultaExames.aspx.cs" Inherits="RelatorioExames_ConsultaExames" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <link rel="stylesheet" href="~/css/style.css" type="text/css" />
    <script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>

    <script type="text/javascript">
        function CheckDate(pObj) {
            var expReg = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/(19|20)?\d{2}$/;
            var aRet = true;
            if ((pObj) && (pObj.value.match(expReg)) && (pObj.value != '')) {
                var dia = pObj.value.substring(0, 2);
                var mes = pObj.value.substring(3, 5);
                var ano = pObj.value.substring(6, 10);
                if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30)
                    aRet = false;
                else
                    if ((ano % 4) != 0 && mes == 2 && dia > 28)
                        aRet = false;
                    else
                        if ((year % 400 == 0 || (year % 100 != 0 && ano % 4 == 0)) && mes == 2 && dia > 29)
                            aRet = false;
            } else
                aRet = false;

            if (!aRet) {
                alert('Atenção! Data inexistente, por favor edite novamente.');
                pObj.value = "";

            }
            return aRet;
        }
         function CheckDate2(pObj) {
            var expReg = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/(19|20)?\d{2}\s([0-1]\d|[2][0-3]):[0-5][0-9]$/;
            var aRet = true;
            if ((pObj) && (pObj.value.match(expReg)) && (pObj.value != '')) {
                var dia = pObj.value.substring(0, 2);
                var mes = pObj.value.substring(3, 5);
                var ano = pObj.value.substring(6, 10);
                if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30)
                    aRet = false;
                else
                    if ((ano % 4) != 0 && mes == 2 && dia > 28)
                        aRet = false;
                    else
                        if ((year % 400 == 0 || (year % 100 != 0 && ano % 4 == 0)) && mes == 2 && dia > 29)
                            aRet = false;
            } else
                aRet = false;

            if (!aRet) {
                alert('Atenção! Data ou hora inexistente, por favor edite novamente.');
                pObj.value = "";

            }
            return aRet;
        }
        $(document).ready(function () {

            $('.campoData1').mask('99/99/9999');

            $('.campoData2').mask('99/99/9999 99:99');

            var $remaining = $('.remaining');


            $('.campoObs').keyup(function () {
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    


   
        <div>
            <h2 style="text-align: center">Consulta de Exames</h2>
            <table>
                <tr>

                    <td class="auto-style2">Registro Hospitalar:
                    </td>
                    <td class="auto-style2">
                        <asp:TextBox ID="txtRh" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lbSituacao" runat="server" Text="Situação: "></asp:Label>
                    </td>
                    <td class="auto-style2">
                        <asp:DropDownList ID="ddlSituacao" TabIndex="5" runat="server" >
                        </asp:DropDownList>
                       
                    </td>

                </tr>
                <tr>
                  
                    <td class="auto-style1">
                        <asp:Label ID="lbDtSolicitacaoInicio" runat="server" Text="Data da Solicitação Inicio:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbDtSolicitacaoInicio" class="campoData1" runat="server" onchange="CheckDate(this);"></asp:TextBox>
                    </td>
                     <td class="auto-style1">
                        <asp:Label ID="lbDtSolicitacaoFim" runat="server" Text="Data da Solicitação Fim:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbDtSolicitacaoFim" class="campoData1" runat="server" onchange="CheckDate(this);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td class="auto-style1">
                        <asp:Label ID="lbDtAgendamentoInicio" runat="server" Text="Data do Agendamento Inicio:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbDtAgendamentoInicio" class="campoData2" runat="server" onchange="CheckDate2(this);"></asp:TextBox>
                    </td>
              
                    <td class="auto-style1">
                        <asp:Label ID="lbDtAgendamentoFim" runat="server" Text="Data do Agendamento Fim:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbDtAgendamentoFim" class="campoData2" runat="server" onchange="CheckDate2(this);"></asp:TextBox>
                    </td>
                </tr>


                <tr>
                    <td>Solicitante:</td>
                    <td>
                        <asp:DropDownList ID="ddlSolicitante" runat="server">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td>Especialidade:</td>
                    <td>
                        <asp:DropDownList ID="ddlEspecialidade" runat="server">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td>Grupo de Exames:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                   
                </tr>
                <tr>
                    <td>Exame:
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
                    <td valign="top" class="auto-style1">
                        <p>
                            &nbsp;</p>
                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">
                        <asp:Button ID="btnConsulta" runat="server" OnClick="btnCadastrar_Click"
                            Text="Consulta" />
                        &nbsp;
                     </td>

                </tr>
            </table>
        </div>
        <div>
        </div>


        <asp:Panel ID="Panel5" runat="server" GroupingText="Exames">
            <asp:GridView ID="grvExamesPesquisados" runat="server" CellPadding="4" GridLines="Vertical"
                Font-Size="Small" Width="850px" BackColor="White"
                BorderColor="#336666" BorderStyle="Double" BorderWidth="1px" OnSelectedIndexChanged="grvExamesPesquisados_SelectedIndexChanged" AutoGenerateSelectButton="True" OnRowDataBound="grvExamesPesquisados_RowDataBound">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </asp:Panel>
        <br />
      


</asp:Content>

