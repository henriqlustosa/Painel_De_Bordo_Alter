<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ExmAgendados.aspx.cs" Inherits="Exames_CadExm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            //Input Mask for landline phone number
            $(".campoTel").mask("(999) 99999-9999");
            //Input Mask for mobile phone number
            $(".campoTelFix").mask("(999) 9999-9999");
            //Input Mask for date of birth or date in general
            $(".campoData").mask("99/99/9999");
            //Input Mask for CNS (SUS)
            $(".campoSus").mask("999.9999.9999.9999");
            //Entrada Mask Consulta
            $(".campoConsulta").mask("9999-9");
            //Entrada Mask Hora
            $(".campoHora").mask("99:99");
        });


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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        Exames Agendados</h1>
    <div>
        <table>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lbUser" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
                <td colspan="3">
                    Registro Funcional:
                    <asp:Label ID="lbRF" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Nome:
                    <asp:Label ID="lbNome" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    Idade:
                    <asp:Label ID="lbIdade" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:Panel ID="Panel5" runat="server" GroupingText="Exames solicitados">
        <asp:GridView ID="GridView3" runat="server" CellPadding="4" GridLines="Vertical"
            AutoGenerateSelectButton="True" Font-Size="Small" Width="850px" BackColor="White"
            BorderColor="#336666" BorderStyle="Double" BorderWidth="1px" OnSelectedIndexChanged="GridView3_SelectedIndexChanged">
            <RowStyle BackColor="White" ForeColor="#333333" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel1" runat="server" GroupingText="Exames Marcados">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" GridLines="Vertical"
           Font-Size="Small" Width="850px" BackColor="White"
            BorderColor="#336666" BorderStyle="Double" BorderWidth="1px" >
            <RowStyle BackColor="White" ForeColor="#333333" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
</asp:Content>
