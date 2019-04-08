<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultaExames.aspx.cs" Inherits="RelatorioExames_ConsultaExames" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	<link rel="stylesheet" href="~/css/style.css" type="text/css" />
	<script src="../js/jquery-1.4.1.min.js" type="text/javascript"></script>

	<script src="../js/maskedinput-1.2.2.js" type="text/javascript"></script>

	<link rel="stylesheet" type="text/css" href="./css/form-estilo.css" />

	
	<script src="./javascript/Form-JavaScript.js" type="text/javascript"></script>



	<title></title>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


	<!--<form class="form-cadastro">-->
	<div class="form-cadastro">
		
			<h1>Consulta de Exames</h1>
			<div class="container-inputs">
				<asp:Label for="irh"  ID="lbRegistroHospitalar" class="label" runat="server">Registro Hospitalar:<br />
					<asp:TextBox type="text" ID="txtRh" name="nrh" class="input-rh" runat="server"></asp:TextBox>
				</asp:Label>

				<asp:Label ID="lbSituacao" for="isituacao" class="label" runat="server">Situação:<br />
					<asp:DropDownList ID="ddlSituacao" type="text" name="nsituacao" class="input-situacao" runat="server" />
				</asp:Label>

				<asp:Label ID="lbDtSolicitacaoInicio" for="idsi" runat="server" class="label" onchange="CheckDate(this)">Data da Solicitação Início:<br />
					<asp:TextBox ID="txbDtSolicitacaoInicio" type="date" name="ndsi" onchange="CheckDate(this)" class="input-dsi" runat="server"></asp:TextBox>
				</asp:Label>

				<asp:Label ID="lbDtSolicitacaoFim" for="idsf" runat="server" class="label">Data da Solicitação Fim:<br />
					<asp:TextBox ID="txbDtSolicitacaoFim" type="date" name="ndsf" onchange="CheckDate(this)" class="input-dsf" runat="server"></asp:TextBox>
				</asp:Label>

				<asp:Label ID="lbDtAgendamentoInicio" for="idai" runat="server" class="label">Data de Agendamento Início:<br />
					<asp:TextBox ID="txbDtAgendamentoInicio" type="date" name="ndai" onchange="CheckDate2(this)" class="input-dai" runat="server"></asp:TextBox>
				</asp:Label>

				<asp:Label ID="lbDtAgendamentoFim" for="idaf" runat="server" class="label">Data de Agendamento Fim:<br />
					<asp:TextBox type="date" ID="txbDtAgendamentoFim" name="ndaf" onchange="CheckDate(this)" class="input-daf" runat="server"></asp:TextBox>
				</asp:Label>

				<asp:Label for="isolicitante" runat="server" class="label">Solicitante:<br />
					<asp:DropDownList ID="ddlSolicitante" type="text" name="nsolicitante" class="input-solicitante" runat="server" />
				</asp:Label>

				<asp:Label for="iespecialidade" runat="server" class="label">Especialidade:<br />
					<asp:DropDownList ID="ddlEspecialidade" type="text" name="nespecialidade" class="input-especialidade" runat="server" />
				</asp:Label>
				<asp:Label for="igrupoexames" runat="server" class="label">Grupo de Exames:<br />
					<asp:DropDownList ID="ddlGrupo" type="text" name="ngrupoexames" AutoPostBack="True"
						OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged" class="input-grupoexames" runat="server" />
				</asp:Label>

				<asp:Label for="iexames" runat="server" class="label">Exames:<br />
					<asp:DropDownList ID="ddlExame" type="text" name="nexames" class="input-exames" runat="server" />
				</asp:Label>

				<asp:Label ID="lbFaltou" class="label-check" runat="server">Paciente faltou no exame:
				 <asp:CheckBox ID="chbFaltou" type="checkbox" name="nfalta" class="input-check" runat="server" />
				</asp:Label>

			
		</div>

		<div class="btn">
			<asp:Button ID="btnConsulta" name="ncconsulta" class="btn-cons" runat="server" OnClick="btnCadastrar_Click" Text="Consultar" />
		</div>
	</div>
	<!--</form>-->


	<asp:Panel ID="Panel5" runat="server" GroupingText="Exames">
		<asp:GridView ID="grvExamesPesquisados" runat="server" CellPadding="4" GridLines="Vertical"
			Font-Size="Small" Width="850px" BackColor="White"
			BorderColor="#336666" BorderStyle="Double" BorderWidth="1px" OnSelectedIndexChanged="grvExamesPesquisados_SelectedIndexChanged" AutoGenerateSelectButton="True" OnRowDataBound="grvExamesPesquisados_RowDataBound">
			<RowStyle BackColor="White" ForeColor="#333333" />
			<FooterStyle BackColor="White" ForeColor="#333333" />
			<PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
			<SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
			<HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
		</asp:GridView>
	</asp:Panel>


</asp:Content>

