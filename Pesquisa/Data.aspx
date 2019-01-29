<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Data.aspx.cs" Inherits="Pesquisa_Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            //Input Mask for landline phone number
            $(".campoTel").mask("(999) 99999-9999");
            //Input Mask for mobile phone number
            $(".campoTelFix").mask("(999) 9999-9999");
            //Input Mask for date of birth or date in general
            $(".campoData").mask("99/99/9999");
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1 align="center">
        Pesquisa de Solicitações por Data</h1>
    <div align="center">
        Data:
        <asp:TextBox ID="txbData" class="campoData" runat="server"></asp:TextBox>
        <asp:Button ID="btnPesq" runat="server" Text="Pesquisar" 
            onclick="btnPesq_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnExport" runat="server" 
            Text="Exportar" onclick="btnExport_Click" />
&nbsp;</div>
    <hr />
    <asp:GridView ID="GridView2" runat="server" BackColor="White"
        Style="font-size: x-small" Width="850px"
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
        GridLines="Horizontal">
        <RowStyle BackColor="White" ForeColor="#333333" />
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
</asp:Content>
