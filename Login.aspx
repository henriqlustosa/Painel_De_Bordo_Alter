<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Login"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Central Login</title>
    <link rel="stylesheet" href="css/style.css" type="text/css" />
</head>
<body>
    <div id="background">
        <div id="page" style="z-index: 1">
            <div id="header">
                <div id="logo">
                        <a href="">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/logo_hspm.jpg" 
                            Height="76px" Width="230px" /></a>
                </div><%--
                Div de implementação - teste
                <div id="navigation">
                </div>--%>
                
            </div>
            <div id="contents">
                <h4>
                    <span>Painel de Bordo</span></h4>
                <div id="faq">
                    <form id="form1" runat="server">
                    <div align="center">
        <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Principal.aspx" 
            LoginButtonText="Acessar" PasswordLabelText="Senha:" 
            PasswordRequiredErrorMessage="Senha obrigatória." TitleText="Acesso" 
            UserNameLabelText="Usuário:" 
            UserNameRequiredErrorMessage="Usuário obrigatório." FailureText="Sua tentativa de acesso ao sistema falhou. Por favor, tente outra vez." >
            <LayoutTemplate>
                <table border="0" cellpadding="1" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2">
                                        Acesso</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuário:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="Usuário obrigatório." 
                                            ToolTip="Usuário obrigatório." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Senha:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Senha obrigatória." 
                                            ToolTip="Senha obrigatória." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Acessar" 
                                            ValidationGroup="Login1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </div>
                    
                    </form>
                </div>
            </div>
            <div id="footer">
                <div class="background">
                    <p class="footnote">
                        &copy; Copyirght &copy; 2013. <a href="">Hospital do Servidor Público Municipal</a>.
                    </p>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
