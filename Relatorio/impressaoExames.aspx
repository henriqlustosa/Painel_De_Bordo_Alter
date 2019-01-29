<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="impressaoExames.aspx.cs" Inherits="Relatorio_impressaoExames" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>
<script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function() {
           
            //Input Mask for date of birth or date in general
            $(".campoData").mask("99/99/9999");
         
        });

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <h1 align="center">
        Mailing de Exames da Fila de Espera</h1>
    <div align="center">
        
        <asp:GridView ID="GridView1" runat="server" >
        </asp:GridView>
        <br />

    </div>
     <table align ="center">
       <tr>
            <td>
                Informe a data do exames agendados:             </td>
            <td>
                <asp:TextBox ID="txbDtRelatorio" class="campoData" runat="server"></asp:TextBox>
            </td>
            
        </tr>
       <tr>
            <td>
                &nbsp;</td>
            <td>
            <asp:Button ID="btnImpr" class = "btn" runat="server" 
            Text="Exportar Arquivo .txt" onclick="btnImpr_Click1" />
            </td>
            
        </tr>
      </table>
      <br />
    <br />
    <br />
     
    <hr />
    </asp:Content>


