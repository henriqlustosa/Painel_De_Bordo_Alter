<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cadDataExameExterno.aspx.cs" Inherits="Exames_cadDataExameExterno" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exames Externos</title>
    <link rel="stylesheet" href="~/css/style.css" type="text/css" />
    <script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function() {
           
            //Input Mask for date of birth or date in general
            $(".campoData").mask("99/99/9999");
          
        });
        </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2 style="text-align: center">
            Agendamento de Exames Externos</h2>
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
                    Solicitante:
                </td>
                <td>
                    <asp:Label ID="lbSolicitante" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    
                </td>
            </tr>
                <tr>
                <td>
                    Grupo de Exame:
                </td>
                <td>
                    <asp:Label ID="lbGrupoExame" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td>
                    Exame solicitado:
                </td>
                <td>
                    <asp:Label ID="lbExame" runat="server"></asp:Label>
                </td>
                <td>
                    Fila:
                </td>
                
                <td>
                    
                    <asp:Label ID="lbfilaExm" runat="server"></asp:Label>
                    
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Agendamento" Height="158px" 
        Width="656px">
    <table>
    <tr>
    <td>Data de Liberação</td>
    <td>
        <asp:TextBox ID="txbDtLiberacao" class="campoData" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txbDtLiberacao" ErrorMessage="Campo obrigatório!"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
       <td valign="top">
                Observação:
       </td>
    <td>
      <asp:TextBox ID="txbObs" runat="server" Height="79px" TextMode="MultiLine" 
            Width="500px"></asp:TextBox>
    </td>       
   </tr>
 
    </table>
    </asp:Panel>
    
    <br />
    <asp:Button ID="btnGravarExamExterno" runat="server" 
        onclick="btnGravarExamExterno_Click" Text="Gravar" />
    <br />
    
    </form>
    
</body>
</html>
