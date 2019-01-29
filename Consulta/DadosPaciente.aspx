<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DadosPaciente.aspx.cs" Inherits="Consulta_DadosPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
            //Input Mask for landline phone number
            $(".campoTel").mask("(99) 99999-9999");
            //Input Mask for mobile phone number
            $(".campoTelFix").mask("(999) 99999-9999");
            //Input Mask for date of birth or date in general
            $(".campoData").mask("99/99/9999");
            //Input Mask for CNS (SUS)
            $(".campoSus").mask("999.9999.9999.9999");
            //Entrada Mask Consulta
            $(".campoConsulta").mask("9999-9");
            //Entrada Mask Hora
            $.mask.definitions['H'] = "[0-2]";
            $.mask.definitions['h'] = "[0-9]";
            $.mask.definitions['M'] = "[0-5]";
            $.mask.definitions['m'] = "[0-9]";

            $(".campoHora").mask("Hh:Mm");

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

        function checagem(id) {


            if (id == 'ctl00_ContentPlaceHolder1_chbRetorno') {

                document.getElementById("ctl00_ContentPlaceHolder1_chbRetorno").checked = true;
                document.getElementById("ctl00_ContentPlaceHolder1_chbRegul").checked = false;
            }
            if (id == 'ctl00_ContentPlaceHolder1_chbRegul') {
                document.getElementById("ctl00_ContentPlaceHolder1_chbRegul").checked = true;
                document.getElementById("ctl00_ContentPlaceHolder1_chbRetorno").checked = false;
            }
        }
        function proximoCampo(atual, proximo) {

            if (atual.value.substring(atual.maxLength - 1) == '') {
                $(document).ready(function() {

                    $(".campoData").mask("99/99/9999");

                    //Entrada Mask Consulta
                    $(".campoConsulta").mask("9999-9");
                    //Entrada Mask Hora
                    $.mask.definitions['H'] = "[0-2]";
                    $.mask.definitions['h'] = "[0-9]";
                    $.mask.definitions['M'] = "[0-5]";
                    $.mask.definitions['m'] = "[0-9]";

                    $(".campoHora").mask("Hh:Mm");

                });
            }

            else if (atual.value.substring(atual.maxLength - 1) != '_') {

                $(proximo).focus();
                $(proximo).select();
            }
        }

        function teste(atual) {



            if (atual.value == '') {
                $(document).ready(function() {


                    $(".campoData").mask("99/99/9999");

                    //Entrada Mask Consulta
                    $(".campoConsulta").mask("9999-9");
                    //Entrada Mask Hora
                    $.mask.definitions['H'] = "[0-2]";
                    $.mask.definitions['h'] = "[0-9]";
                    $.mask.definitions['M'] = "[0-5]";
                    $.mask.definitions['m'] = "[0-9]";

                    $(".campoHora").mask("Hh:Mm");

                });
            }
        }


        function fnDispara(obj) {

            if (obj.value == "Sim") {


                $(".campoConsulta").focus();
                $(".campoConsulta").select();


            }




        }


        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Deseja incluir exames?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

    </script>

    <style type="text/css">
        .Hide
        {
            display: none;
        }
        .style13
        {
            height: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1 align="center">
        Solicitação (Fila de Espera)</h1>
    <table>
        <tr>
            <td colspan="2">
                <asp:Label ID="lbUser" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                Registro Hospitalar:
                <asp:TextBox ID="txbRh" runat="server" TabIndex="1"></asp:TextBox>
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
        <tr>
            <td colspan="2">
                Cartão SUS:
                <asp:TextBox ID="txbSus" class="campoSus" runat="server" Width="200px"></asp:TextBox>
                &nbsp;<asp:Button ID="btnAtuSUS" runat="server" OnClick="btnAtuSUS_Click" Text="Atualizar" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lbDateIni" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                Data da Solicitação:
            </td>
            <td colspan="3">
                <asp:TextBox ID="txbdtSol" class="campoData1" runat="server"></asp:TextBox>
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
            <td colspan="3">
                <asp:UpdatePanel ID="especialidade" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlEspec" runat="server" TabIndex="2">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                Solicitante:
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlSol" runat="server" TabIndex="3">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <span class="style2">Procedimentos Solicitados</span>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:CheckBox ID="chbRetorno" runat="server" Text="Encaminhamento/Retorno" onclick="checagem(this.id)" />
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:CheckBox ID="chbRegul" runat="server" Text="Regulação" onclick="checagem(this.id)" />
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:Button ID="Button3" runat="server" Text="Cadastrar Exames" OnClick="Button3_Click"
                    OnClientClick="Confirm()" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                Quantidade:
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txbqtd" runat="server" TabIndex="4" Width="45"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txbqtd"
                            ErrorMessage="O campo [Quantidade] não foi preenchido." ValidationGroup="cadastro">*</asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                Situação:
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlSituacao" TabIndex="5" runat="server">
                            <asp:ListItem Selected="True">Enc. UAC</asp:ListItem>
                            <asp:ListItem>Recebido UAC</asp:ListItem>
                            <asp:ListItem>Enc. GTA</asp:ListItem>
                            <asp:ListItem>Recebido GTA</asp:ListItem>
                            <asp:ListItem>Cancelada</asp:ListItem>
                            <asp:ListItem>Ativo HSPM</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td colspan="5">
            </td>
        </tr>
        <tr>
            <td>
                Marcada:
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlMarcada" onchange="fnDispara(this);return false" TabIndex="6"
                            runat="server">
                            <asp:ListItem Selected="True" Value="Não">Não</asp:ListItem>
                            <asp:ListItem Value="Sim">Sim</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                Consulta:
            </td>
            <td>
                <asp:UpdatePanel ID="consultaPanel" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txbConsulta" AutoPostBack="true" onclick="teste(this)" TabIndex="7"
                            MaxLength="6" runat="server" OnTextChanged="txbConsulta_TextChanged" class="campoConsulta"
                            Width="50px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                Data:
            </td>
            <td>
                <asp:UpdatePanel ID="dtConPanel" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txbDtCon" TabIndex="8" onclick="teste(this)" MaxLength="10" runat="server"
                            class="campoData" Onkeyup="proximoCampo(this, '.campoHora')" Width="80px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                Hora:
            </td>
            <td colspan="2">
                <asp:UpdatePanel ID="horaConPanel" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txbHoraCon" TabIndex="9" Width="80" onclick="teste(this)" MaxLength="5"
                            runat="server" class="campoHora" Onkeyup="proximoCampo(this, '.campoObs')"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                Observação:
            </td>
            <td colspan="7">
                <asp:TextBox ID="txbObs" Text='<%# Bind("Text") %>' runat="server" class="campoObs"
                    MaxLength="199" onkeyDown="checkTextAreaMaxLength(this,event,'199');" TabIndex="10"
                    Height="79px" TextMode="MultiLine" Width="500px"></asp:TextBox>
            </td>
            <td valign="top">
                <p>
                    <span id="remaining" class="remaining">199 caracteres restantes</span>
                </p>
            </td>
        </tr>
    </table>
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
    <table>
        <tr>
            <td class="style13">
                <asp:Button ID="Button1" runat="server" TabIndex="12" Text="Cadastrar" OnClick="Button1_Click"
                    ValidationGroup="cadastro" />
            </td>
            <td class="style13">
                <asp:Button ID="Button2" TabIndex="13" runat="server" Text="Atualizar" OnClick="Button2_Click" />
            </td>
        </tr>
    </table>
    <hr />
    <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False" BackColor="White"
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
        DataSourceID="SqlDataSource1" GridLines="Vertical" Style="font-size: x-small"
        Width="850px" OnSelectedIndexChanged="GridView6_SelectedIndexChanged" AutoGenerateSelectButton="True">
        <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="cod" HeaderText="cod" InsertVisible="False" ReadOnly="True"
                SortExpression="cod"  ItemStyle-CssClass="Hide" 
                HeaderStyle-CssClass="Hide" >
<HeaderStyle CssClass="Hide"></HeaderStyle>

<ItemStyle CssClass="Hide"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="data" HeaderText="Data" SortExpression="data" />
            <asp:BoundField DataField="especialidade" HeaderText="Especialidade" SortExpression="especialidade" />
            <asp:BoundField DataField="solicitante" HeaderText="Solicitante" SortExpression="solicitante" />
            <asp:BoundField DataField="retorno" HeaderText="Retorno" SortExpression="retorno" />
            <asp:BoundField DataField="regulacao" HeaderText="Regulacao" SortExpression="regulacao" />
            <asp:BoundField DataField="qtdExames" HeaderText="QtdExames" SortExpression="qtdExames" />
            <asp:BoundField DataField="situacao" HeaderText="Situacao" SortExpression="situacao" />
            <asp:BoundField DataField="marcada" HeaderText="Marcada" SortExpression="marcada" />
            <asp:BoundField DataField="consulta" HeaderText="Consulta" SortExpression="consulta" />
            <asp:BoundField DataField="dtCon" HeaderText="Data" SortExpression="dtCon" />
            <asp:BoundField DataField="hrCon" HeaderText="Hora" SortExpression="hrCon" />
            <asp:BoundField DataField="telefone" HeaderText="Telefone" SortExpression="telefone" />
            <asp:BoundField DataField="obs" HeaderText="Obs" SortExpression="obs" />
            <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />
                 <asp:TemplateField>
        <ItemTemplate>
            <asp:Button ID="Buttonid" runat="server" CommandName="NovoExame" Text="Exame" OnClick="Button_click_event"></asp:Button>    
        </ItemTemplate>
 </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SqlServices %>"
        SelectCommand="SELECT [cod],CONVERT(varchar,data, 103) as data, [especialidade], [solicitante], [retorno],[regulacao],[qtdExames], [situacao], [marcada], [consulta],CONVERT(varchar,dtCon,103) as dtCon,[hrCon], [telefone], [obs], [usuario] FROM [Geral_Treina].[dbo].[Fila] WHERE ([rh] = @rh) and marcada = 'Não' and situacao != 'Cancelada' ORDER BY [cod] desc">
        <SelectParameters>
            <asp:ControlParameter ControlID="txbRh" Name="rh" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Atenção"
        ShowMessageBox="True" ShowSummary="False" ValidationGroup="cadastro" />
</asp:Content>
