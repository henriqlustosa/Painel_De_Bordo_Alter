<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cadDataExame.aspx.cs" Inherits="Exames_cadDataExame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        $(".campoConsulta").mask("9999-9");
        });
        
        
  
 

   

     
</script>
</head>
<body>
    <form id="form1" runat="server"> 
   
    <div>
        <h2 style="text-align: center">
            Agendamento de Exames</h2>
        <table>
            <tr>
                <td>
                    Registro Hospitalar:
                </td>
                <td>
                    <asp:Label ID="lbRh" runat="server" Text=""></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Paciente:
                </td>
                <td>
                    <asp:Label ID="lbPaciente" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Exame solicitado:
                </td>
                <td>
                    <asp:Label ID="lbExame" runat="server"></asp:Label>
                </td>
                <td>
                    
                    <asp:Label ID="lbfilaExm" runat="server"></asp:Label>
                    
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Agendamento">
    <table>
    <tr>
    <td>Nº da Consulta:</td>
    <td>
        <asp:TextBox ID="txbNumCon" class="campoConsulta" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txbNumCon" ErrorMessage="Campo obrigatório!"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
    <td>Data:</td>
    <td>
        <asp:TextBox ID="txbDt" class="campoData"  runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="txbDt" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ControlToValidate="txbDt" ErrorMessage="* Erro no preenchimento da data!" 
            ValidationExpression="^([0-2][0-9]|3[0-1]){1}(/0[0-9]|/1[0-2]){1}(/20[0-9][0-9]){1}$"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
    <td>Hora:</td>
    <td>
        <asp:TextBox ID="txbHr" class="campoHora" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="txbHr" ErrorMessage="Campo obrigatório!"></asp:RequiredFieldValidator>
        
     
        
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="txbHr" 
            ErrorMessage="* Erro no preenchimento da data!" 
            ValidationExpression="^([0-1][0-9]|2[0-4]){1}(:[0-5][0-9]){1}$"></asp:RegularExpressionValidator>
        
     
        
        </td>
    </tr>
    <tr>
    <td>Executante:</td>
    <td>
        <asp:TextBox ID="txbExecutante" runat="server" Width="500px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            ControlToValidate="txbExecutante" ErrorMessage="Campo obrigatório!"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
    <td>
        <asp:CheckBox id="chbAtivo" runat="server" Text="Ativo 156" /></td>
    <td>
        <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" 
            onclick="btnCadastrar_Click" />
        </td>
    </tr>
    <tr>
    <td></td>
    <td></td>
    </tr>
    </table>
    </asp:Panel>
    
    </form>
</body>
</html>
