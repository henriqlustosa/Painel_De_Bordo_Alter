<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="impressao.aspx.cs" Inherits="Relatorio_impressao" %>

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
<script type="text/javascript">
    function DisableButtons() {
        var x;
        x = document.getElementById("<%=txbDtRelatorio.ClientID %>").value;

        document.getElementById("<%=HiddenField1.ClientID %>").value = x;
        document.getElementById("<%=txbDtRelatorio.ClientID %>").disabled = true;
        
        setTimeout('document.getElementById ("<%=btnImpr.ClientID %>").disabled = true;', 500)
        return true;
        
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <h1 align="center">
        Relatório Retorno da Fila de Espera</h1>
    <div align="center">
        <asp:GridView ID="GridView1" runat="server" >
        </asp:GridView>
        <br />
Para acesso aos Mailings enviados acesse 
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="ftp://hspmcac544/Mailings_enviados/Mailing_retorno_call156/">Mailing Retorno 156</asp:HyperLink>
<br />
    </div>
     <table align ="center">
       <tr>
            <td>
                Informe a data das consultas:&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txbDtRelatorio" class="campoData" runat="server"></asp:TextBox>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txbDtRelatorio" ErrorMessage="Campo não preenchido!"></asp:RequiredFieldValidator>
            </td>
            
        </tr>
       
       <tr>
            <td>
                &nbsp;</td>
            <td>
            <asp:Button ID="btnImpr" class = "btn" runat="server" 
            Text="Exportar Arquivo .txt" onclick="btnImpr_Click1" OnClientClick ="return DisableButtons()" />
            </td>
            
        </tr>
       
      </table>
      <br />
    <asp:GridView ID="GridView2" align = "center" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="cod" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="cod" HeaderText="Código" InsertVisible="False" 
                ReadOnly="True" SortExpression="cod" />
            <asp:BoundField DataField="data" HeaderText="Data" SortExpression="data" />
            <asp:BoundField DataField="dt_consultas" HeaderText="Data Consultas" SortExpression="data" DataFormatString="{0:d}" />
            <asp:BoundField DataField="qtd_consultas" HeaderText="Quantidade de Consultas" SortExpression="data" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:SqlServices %>" 
        SelectCommand="SELECT TOP 5 [cod], [data], [dt_consultas],[qtd_consultas] FROM [Impressao] ORDER BY [cod] DESC"></asp:SqlDataSource>
    <br />
    <br />
     
    <hr />
    </asp:Content>

