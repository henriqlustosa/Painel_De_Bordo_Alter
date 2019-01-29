<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rediscagem.aspx.cs" Inherits="Relatorio_Rediscagem" %>

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
    <h1 align="center">Relatório Rediscagem da Fila de Espera</h1>
    <br />
    <div class="alinha">
        <asp:FileUpload ID="fupArquivo" runat="server" Size="50" />
        <asp:Label ID="lblSaida" runat="server" Text=""></asp:Label>
        <asp:Button ID="btnLerExcel" runat="server" OnClick="btnLerExcel_Click" Text="Ler Excel" /><br />
        Obs.: Selecione um arquivo (.xls) Microsoft Office 2003.
        
            <br />
            <br />
        <br />
        <asp:Button ID="btnExportar" runat="server" onclick="btnExportar_Click" 
            Text="Exportar .txt" />
            <h5 align="center">
                <asp:Label ID="lbTituloArquivo" runat="server" Text=""></asp:Label></h5>
            <br />
        <asp:GridView ID="gvExcel" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None" Font-Size="X-Small">
            <RowStyle BackColor="#E3EAEB" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    
</asp:Content>

