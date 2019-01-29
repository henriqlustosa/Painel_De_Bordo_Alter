
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Perfil.aspx.cs" Inherits="Administracao_Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1 style="text-align: center">Perfil de Usuário</h1>
<div>
<table align="center">
                <tr>
                    <td style="width: 523px">
                        <h3 style="text-align: center">
                            CADASTRO DE PERMISSÕES DE USUÁRIOS<br />
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td style="width: 523px; text-align: center;">
                        Usuário&nbsp;
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 523px" align="center">
                        <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="centro">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 523px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 523px; text-align: center;">
                        <asp:Button ID="btnCad" runat="server" Text="Cadastrar" OnClick="btnCad_Click" />
                    </td>
                </tr>
            </table>
</div>
</asp:Content>

