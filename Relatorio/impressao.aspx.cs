using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Globalization;
public partial class Relatorio_impressao : System.Web.UI.Page
{
	//Método que carrega a página
	protected void Page_Load(object sender, EventArgs e)
	{
	}

	// Método que faz o start para a exportação do arquivo, ao clicar
	// no botão "Exportar arquivo" busca no Banco de Dados:- os registros
	// marcados como sim, que foram marcados até a data atual.
	// Entrada:  vazio
	// Saída: Data Set contendo a consulta que retorna todas as consultas cadastradas até a data atual.
	public DataSet exportarArquivo()
	{
		string dtParaCons = HiddenField1.Value;
		string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";
		string marcada = "Sim";

		strQuery = "SELECT [rh],[especialidade],[solicitante]" +
",[marcada],[consulta],[telefone],[dtCon],[hrCon],[cod],[obs]" +
" FROM [Geral_Treina].[dbo].[Fila] " +
" where marcada = '" + marcada + "' AND dtCon = '" + dtParaCons +
"' And impr = 'false' and not((situacao='Cancelada')) and not((situacao='Ativo HSPM'))";

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataSet dataSet = new DataSet();
			SqlCommand cmd = new SqlCommand(strQuery, conn);

			try
			{
				cmd.CommandType = CommandType.Text;
				adapter.TableMappings.Add("Table", "ArquivoTxt");
				adapter.SelectCommand = cmd;
				adapter.Fill(dataSet);
			}

			catch (Exception ex)
			{
				string erro = ex.Message;
			}
			return dataSet;
		}
	}

	// Método para que retorne uma tabela com todos os campos que serão  utilizados para a exportação
	// do arquivo txt.
	// Entrada:
	//     conjunto1 - Data Set contendo a consulta que retorna todas as consultas CADASTRADAS até a data atual
	//     DateTime dt - data base que a partir dela sera verificada as datas de consultas AGENDADAS

	//public DataTable consultasAgendadas(DataSet conjunto1, string dt)
	public DataTable consultasAgendadas(DataSet conjunto1)
	{
		DataTable dt2 = new DataTable();
		
		try
		{
			// Adicionando as colunas que serão visualizadas no arquivo txt
			dt2.Columns.Add("Nome", System.Type.GetType("System.String"));    // Nome do paciente
			dt2.Columns.Add("Rh", System.Type.GetType("System.String"));      // Registro hospitar   
			dt2.Columns.Add("Especialidade", System.Type.GetType("System.String"));  // Especialidade médica
			dt2.Columns.Add("Dt_Consulta", System.Type.GetType("System.String"));     // Data agendada da consulta
			dt2.Columns.Add("Solicitante", System.Type.GetType("System.String"));    // Nome do médico
			dt2.Columns.Add("Consulta", System.Type.GetType("System.String"));       // Código da consulta
			dt2.Columns.Add("Observacao_Hospub", System.Type.GetType("System.String"));       // Telefone do paciente
			dt2.Columns.Add("Telefone1", System.Type.GetType("System.String"));       // Telefone do paciente
			dt2.Columns.Add("Telefone2", System.Type.GetType("System.String"));       // Telefone2 do paciente
			dt2.Columns.Add("Telefone3", System.Type.GetType("System.String"));       // Telefone3 do paciente
			dt2.Columns.Add("Telefone4", System.Type.GetType("System.String"));       // Telefone4 do paciente
			foreach (DataRow pRow in conjunto1.Tables["ArquivoTxt"].Rows)
			{
				string tel2 = "";
				string tel3 = "";
				string tel4 = "";
				//string datateste = pRow["dtCon"].ToString();
				if (pRow["dtCon"].ToString().Equals(""))
				{
					continue;
				}
				else
				{
					string marcada = pRow["marcada"].ToString();
					string rh = pRow["rh"].ToString();
					string nome = getNome(rh).Replace("'", " ").Replace("\"", " ");
					string espec = pRow["especialidade"].ToString();
					string data1 = pRow["dtCon"].ToString();
					string[] data2 = data1.Split('/');
					string data3 = data2[2] + "-" + data2[1] + "-" + data2[0];
					string hora = pRow["hrCon"].ToString() + ":00";// acrescentado os segundos na hora da consulta
					string sol = pRow["solicitante"].ToString().Replace("'", " ").Replace("\"", " ");
					string datConsult = data3 + " " + hora;

					// se o campo da data não estiver preenchido
					if (datConsult.Equals(""))
						datConsult = "00-00-0000 00:00:00";// acrescentado os segundos na hora da consulta

					string consulta = pRow["consulta"].ToString();
					// se o campo do código da consulta não estiver preenchido
					if (consulta.Equals(""))
						consulta = "sem preenchimento";
					AtualizacaoTabelaExames(consulta);
					   

					string tel = pRow["telefone"].ToString();
					// se o campo do telefone não estiver preenchido
					if (tel.Equals("") || tel.Equals("&nbsp;"))
					{
						using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
						{
							OdbcCommand cmm = cnn.CreateCommand();
							cmm.CommandText = "Select ib6telef from intb6 where ib6regist = " + rh;
							cnn.Open();
							OdbcDataReader dr = cmm.ExecuteReader();
							if (dr.Read())
							{
								char[] arr = new char[] { '0', ' ' };
								tel = dr.GetString(0);
								tel = tel.TrimStart(arr);
								if (tel != "")
								{
									if (tel.Length.Equals(8))
									{
										tel = tel.Substring(0, 4) + "-" + tel.Substring(4, 4);
										tel = "(11)  0" + tel;

									}
									else if (tel.Length.Equals(9))
									{
										tel = tel.Substring(0, 5) + "-" + tel.Substring(5, 4);
										tel = "(11) " + tel;
									}
									else if (tel.Length.Equals(10))
									{
										tel = "(" + tel.Substring(0, 2) + ") 0" + tel.Substring(2, 4) + "-" + tel.Substring(6, 4);

									}
									else if (tel.Length.Equals(11))
									{
										tel = "(" + tel.Substring(0, 2) + ") " + tel.Substring(2, 5) + "-" + tel.Substring(7, 4);

									}
								}

								else
								{
									tel = " ";
								}
							}
						}
					}
					string observ = HttpUtility.HtmlDecode(pRow["obs"].ToString()).Replace(';',' ');

				
					if (observ.Equals(""))
						observ = " ";

					using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
					{
						SqlCommand cmm = cnn.CreateCommand();
						cmm.CommandText = "SELECT telefone2,telefone3,telefone4 FROM Telefones_Adicionais WHERE rh='" + rh + "'";

						try
						{
							cnn.Open();
							SqlDataReader dr = cmm.ExecuteReader();
							if (dr.Read())
							{
								tel2 = dr.GetString(0);
								tel3 = dr.GetString(1);
								tel4 = dr.GetString(2);
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("{0} Exception caught.", ex);
						}
					}
					if (tel2.Equals("") || tel2.Equals("&nbsp;"))
					{
						tel2 = " ";
					}
					if (tel3.Equals("") || tel3.Equals("&nbsp;"))
					{
						tel3 = " ";
					}
					if (tel4.Equals("") || tel4.Equals("&nbsp;"))
					{
						tel4 = " ";
					}
				  


					dt2.Rows.Add(new String[] { nome, rh, espec, datConsult, sol, consulta, observ, tel,tel2,tel3,tel4 });

				}
			}
		} //try
		catch (Exception ex)
		{
			string erro = ex.Message;
		}
		return dt2;
	}

	

	// Método para buscar o código de todos os registros que se encontram 15 dias  
	// corrente do mês e marcá-los como impressos

	public void obtendoCodigos(DataSet conjunto1, string dt)
	{
		int totalDeRegistros = conjunto1.Tables["ArquivoTxt"].Rows.Count;
		DataTable dt3 = new DataTable();
		try
		{
			dt3.Columns.Add("Codigo", System.Type.GetType("System.String")); // Codigo do paciente

			foreach (DataRow pRow in conjunto1.Tables["ArquivoTxt"].Rows)
			{
				string datateste = pRow["dtCon"].ToString();
				if (pRow["dtCon"].ToString().Equals(""))
				{
					continue;
				}
				else
				{
					if (dt.Equals(datateste))
					{
						string codigo = pRow["cod"].ToString();
						dt3.Rows.Add(new String[] { codigo });
					}
				}
			} //foreach
		}//try
		catch (Exception ex)
		{
			string erro = ex.Message;
		}

		foreach (DataRow pRow2 in dt3.Rows)
		{

			int codigo_sozinho = Convert.ToInt32(pRow2["codigo"].ToString());
			marcarImpresso(codigo_sozinho);

		}

		int codigo_log = obterCodigoLog(fazerLog(totalDeRegistros));

		foreach (DataRow pRow2 in dt3.Rows)
		{
			int codigo_sozinho2 = Convert.ToInt32(pRow2["codigo"].ToString());
			mudarDeTabela(codigo_sozinho2, codigo_log);
			deletarTabelaFila(codigo_sozinho2);
		}
	}


	// Método para marcar toda as consultas agendadas como 'está impresso', 
	// registro por registrs
	// Entrada:
	//      int - codigo: primary key da tabela Fila indicando
	//                    o registro a ser marcado
	static void marcarImpresso(int codigo)
	{

		string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";

		strQuery = "UPDATE [Geral_Treina].[dbo].[Fila]" +
	   " SET [impr] = 'true'" +
	   " where [cod] = " + codigo;

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			try
			{

				SqlCommand cmd = new SqlCommand(strQuery, conn);
				conn.Open();
				int i = cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
		}
	}
	static void AtualizacaoTabelaExames(string codigo)
	{

		string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";


		strQuery = "UPDATE [Geral_Treina].[dbo].[Exames_Paciente]" +
	   " SET [impr] = 'true'" +
	   " where [cod_fila] = " + codigo;

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			try
			{

				SqlCommand cmd = new SqlCommand(strQuery, conn);
				conn.Open();
				int i = cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
		}
	}
	public void exportarToTxt()
	{
		DateTime dt = DateTime.Now.AddDays(1);//dia atual + 1 = dia seguinte;
		
		string dia = Convert.ToString(dt.Day);
		if (dia.Length == 1)
			dia = dia.PadLeft(2, '0');
		string mes = Convert.ToString(dt.Month);
		if (mes.Length == 1)
			mes = mes.PadLeft(2, '0');

		string data = Convert.ToString(dia) + Convert.ToString(mes) + Convert.ToString(dt.Year);

		string nomeArquivo = "mailing_HSPM_Retorno_" + data + ".txt";

		Response.ClearContent();
		Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", nomeArquivo));
		Response.ContentType = "application/text";

		StringBuilder str = new StringBuilder();
		str.Append("PACIENTE;RH;ESPECIALIDADE;DT_CONSULTA;PROFISSIONAL;COD_CONSULTA;OBSERVACAO_HOSPUB;TELEFONE1;TELEFONE2;TELEFONE3;TELEFONE4;");
		str.AppendLine();

		for (int j = 0; j < GridView1.Rows.Count; j++)
		{
			for (int k = 0; k < GridView1.Rows[j].Cells.Count; k++)
			{
				str.Append(HttpUtility.HtmlDecode( GridView1.Rows[j].Cells[k].Text + ';').Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));
			}
			str.AppendLine();
		}
		Response.Write(str.ToString());
		Response.End();
	}

	public static String getNome(string _rh)
	{
		string nome = "";

		using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
		{
			OdbcCommand cmm = cnn.CreateCommand();
			cmm.CommandText = "Select ib6pnome,ib6compos from intb6 where ib6regist = " + _rh;
			cnn.Open();
			OdbcDataReader dr = cmm.ExecuteReader();
			if (dr.Read())
			{
				nome = dr.GetString(0) + dr.GetString(1);
			}
		}
		return nome;
	}

	// Método  para preencher o GriedView
	public void gridCarregaGridView1()
	{
		string dataParaConsulta = HiddenField1.Value; //a data que contém o campo txbDtRelatorio
		GridView1.DataSource = consultasAgendadas(exportarArquivo());
		obtendoCodigos(exportarArquivo(), dataParaConsulta);
		GridView1.DataBind();
	}

	public string fazerLog(int ttal)
	{
		string total = ttal.ToString();
		DateTime dataDoSistema = DateTime.Now;
		string datasistem = recuperarDataParaString(dataDoSistema);
		string dataConsultas = HiddenField1.Value; //a data que contém o campo txbDtRelatorio
		string usuario = Membership.GetUser().UserName.ToUpper(); ;

		string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";

		strQuery = "INSERT INTO impressao (usuario, data, dt_consultas,qtd_consultas) VALUES (@user,@date,@dt_consultas,@qtd_consultas)";

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			try
			{
				SqlCommand cmd = new SqlCommand(strQuery, conn);
				conn.Open();

				cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = usuario;
				cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = dataDoSistema;
				cmd.Parameters.Add("@dt_consultas", SqlDbType.DateTime).Value = dataConsultas;
				cmd.Parameters.Add("@qtd_consultas", SqlDbType.Int).Value = total;
				int i = cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
		}
		return datasistem;
	}

	public int obterCodigoLog(string dataImpr)
	{
		string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";
		int codigo_impressao = 0;
		strQuery = "SELECT [cod] from impressao " +
	  " where data ='" + dataImpr + "'";

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			try
			{
				SqlCommand cmd = new SqlCommand(strQuery, conn);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					codigo_impressao = dr.GetInt32(0);
				}
			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
		}
		return codigo_impressao;
	}
	public void mudarDeTabela(int codigo_fila, int codigo_log)
	{
		string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";

		strQuery = "SELECT [cod],[rh],[data],[especialidade],[solicitante],[qtdExames],[retorno],[regulacao]" +
	  ",[situacao],[marcada],[consulta],[dtCon],[hrCon],[telefone],[obs],[usuario] from fila" +
	  " where cod = " + codigo_fila;

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			int cod_fila = 0;
			string rh = "";
			DateTime data = new DateTime();
			string especialidade = "";
			string solicitante = "";
			int qtdExames = 100;
			string retorno = "";
			string regulacao = "";
			string situacao = "";
			string marcada = "";
			string consulta = "";
			string dtCon = "";
			string hrCon = "";
			string telefone = "";
			string obs = "";
			string usuario = "";
			string date = "";
			try
			{
				SqlCommand cmd = new SqlCommand(strQuery, conn);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					cod_fila = dr.GetInt32(0);
					rh = dr.GetString(1);
					data = dr.GetDateTime(2);
					string dia = Convert.ToString(data.Day);
					if (dia.Length == 1)
						dia = dia.PadLeft(2, '0');
					string mes = Convert.ToString(data.Month);
					if (mes.Length == 1)
						mes = mes.PadLeft(2, '0');
					date = data.Year + "-" + mes + "-" + dia;
					especialidade = dr.GetString(3);
					solicitante = dr.GetString(4);
					qtdExames = dr.GetInt32(5);
					retorno = dr.GetString(6);
					regulacao = dr.GetString(7);
					situacao = dr.GetString(8);
					marcada = dr.GetString(9);
					consulta = dr.GetString(10);
					dtCon = dr.GetString(11);
					hrCon = dr.GetString(12);
					telefone = dr.GetString(13);

					// se o campo do telefone não estiver preenchido
					if (telefone.Equals("") || telefone.Equals("&nbsp;"))
					{
						using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
						{
							OdbcCommand cmm = cnn.CreateCommand();
							cmm.CommandText = "Select ib6telef from intb6 where ib6regist = " + rh;
							cnn.Open();
							OdbcDataReader dr1 = cmm.ExecuteReader();
							if (dr1.Read())
							{
								char[] arr = new char[] { '0', ' ' };
								telefone = dr1.GetString(0);
								telefone = telefone.TrimStart(arr);
							}
						}
					}
					obs = dr.GetString(14);
					obs = obs.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
					usuario = dr.GetString(15);

				}
				dr.Close();
				conn.Close();
				string strQuery2 = "";

				strQuery2 = "INSERT INTO Ret_marcadas_impr (cod_impressao,cod_fila, rh, data, especialidade, solicitante, qtdExames " +
				",retorno, regulacao, situacao,marcada,consulta,dtCon,hrCon,telefone,obs,usuario) VALUES (" + codigo_log + "," + cod_fila + ","
				+ rh + ",'" + date + "','" + especialidade + "','" + solicitante + "'," + qtdExames + ",'" + retorno + "','" + regulacao + "','"
				+ situacao + "','" + marcada + "','" + consulta + "','" + dtCon + "','" + hrCon + "','" + telefone + "','" +
				 obs + "','" + usuario + "'" + " )";

				SqlCommand cmd2 = new SqlCommand(strQuery2, conn);
				conn.Open();
				int i = cmd2.ExecuteNonQuery();
				conn.Close();
			}//try
			catch (SqlException e)
			{
				string erro = e.Message;
			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
		}//using

	}//fim do metodo

	public void deletarTabelaFila(int codigo_fila)
	{

		string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "";

		strQuery = "DELETE FROM fila " +

	  " where cod = " + codigo_fila;

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			try
			{
				SqlCommand cmd = new SqlCommand(strQuery, conn);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
			}

			catch (Exception ex)
			{
				string erro = ex.Message;
			}
		}
	}
	public string recuperarDataParaString(DateTime data)
	{

		string dia = Convert.ToString(data.Day);
		if (dia.Length == 1)
			dia = dia.PadLeft(2, '0');
		string mes = Convert.ToString(data.Month);
		if (mes.Length == 1)
			mes = mes.PadLeft(2, '0');

		string dataSistem = Convert.ToString(data.Year) + "-" + Convert.ToString(mes) + "-" + Convert.ToString(dia) +
			" " + Convert.ToString(data.Hour) + ":" + Convert.ToString(data.Minute) + ":" + Convert.ToString(data.Second);

		return dataSistem;

	}
	protected void btnImpr_Click1(object sender, EventArgs e)
	{
		gridCarregaGridView1();
		exportarToTxt();
	}
}
