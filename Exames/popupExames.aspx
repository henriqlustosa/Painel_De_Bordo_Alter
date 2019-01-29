<%@ Page Language="C#" AutoEventWireup="true" CodeFile="popupExames.aspx.cs" Inherits="Exames_popupExames" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="~/css/style.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lbrh" runat="server" Visible="false" Text=""></asp:Label>
    <asp:Label ID="lbpac" runat="server" Visible="false" Text=""></asp:Label>
    <asp:Label ID="lbfila" runat="server" Visible="false" Text=""></asp:Label>
    <asp:Label ID="lbexm" runat="server" Visible="false" Text=""></asp:Label>
    <asp:Label ID="lbsol" runat="server" Visible="false" Text=""></asp:Label>
    <div>
        <table style="width:100%;">
            <tr>
                <td colspan="2">
                    <h1 style="text-align: center">Agendamento</h1></td>
            </tr>
            <tr>
                <td style="text-align: center">
        <asp:Button ID="btnInterno" runat="server" Text="Interno" onclick="btnInterno_Click" />
                </td>
                <td style="text-align: center">
        <asp:Button ID="btnExterno" runat="server" Text="Externo" onclick="btnExterno_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
