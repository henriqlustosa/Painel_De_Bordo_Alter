using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for FilaExames
/// </summary>
public class FilaExames
{
	public FilaExames()
	{
		
	}
	public static string getExame(int cod)
	{
		string descr = "";
		using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
		{
			SqlCommand cmm = cnn.CreateCommand();
			cmm.CommandText = "Select Descricao from Exames where Cod_Exame = " + cod;
			cnn.Open();
			SqlDataReader dr = cmm.ExecuteReader();
			if (dr.Read())
			{
				descr = dr.GetString(0);
			}
		}
		return descr;
	}
	public static string getGrupoExame(int cod)
	{
		string descr = "";
		using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
		{
			SqlCommand cmm = cnn.CreateCommand();
			cmm.CommandText = "SELECT g.Descricao FROM [Geral_Treina].[dbo].[Exames] as e join [Geral_Treina].[dbo].[Grupo_Exame] as g"
		+ " on e.Cod_Grupo_Exame = g.Cod_Grupo_Exame where e.Cod_Exame =" + cod;
			cnn.Open();
			SqlDataReader dr = cmm.ExecuteReader();
			if (dr.Read())
			{
				descr = dr.GetString(0);
			}
		}
		return descr;
	}


	// Carrega Exames aquardando vaga
	public static DataTable gridCarregaExamesSolicitados( string cod_fila)
	{
		string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
		string strQuery = "SELECT cod_fila,cod_exame, solicitante,especialidade,obs,cod " +
							"FROM Exames_Paciente " +
							"WHERE status = 4 " + //status 4 - aquardando vaga
							"AND cod_fila = " + cod_fila; 

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			SqlDataReader dr1 = null;
			SqlCommand cmd = new SqlCommand(strQuery, conn);
			DataTable dt = new DataTable();

			try
			{
				conn.Open();
				dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);

				dt.Columns.Add("Cod_Fila", System.Type.GetType("System.String"));
                dt.Columns.Add("Codigo", System.Type.GetType("System.String"));
                //dt.Columns.Add("Cod_Exame", System.Type.GetType("System.String"));
                dt.Columns.Add("Solicitante", System.Type.GetType("System.String"));
				dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
				dt.Columns.Add("Grupo de Exames", System.Type.GetType("System.String"));
				dt.Columns.Add("Exames", System.Type.GetType("System.String"));
				dt.Columns.Add("Observacao", System.Type.GetType("System.String"));

				while (dr1.Read())
				{
					string codigo_fila = dr1.GetInt32(0).ToString();
					//string codigo_exame = dr1.GetInt32(1).ToString();
					string solicitante = dr1.GetString(2);
					string especialidade = dr1.GetString(3);
					string grupo_exames = getGrupoExame(dr1.GetInt32(1));
					string exames = getExame(dr1.GetInt32(1));
					string obs = dr1.GetString(4);
                    string codigo = dr1.GetInt32(5).ToString();

                    string especialidae = dr1.GetString(2);
					dt.Rows.Add(new String[] { codigo_fila, codigo, solicitante, especialidade, grupo_exames, exames, obs });
				}

			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
			finally
			{
				if (dr1 != null) dr1.Close();
			}
			return dt;
		}
	}


	// Carrega Exames marcados e não impressos
	public static DataTable gridCarregaExamesMarcados( string cod_fila)
	{
		string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";

		string strQuery = "SELECT [cod],[Descricao],[dt_consulta]" +
		",[hr_consulta],[executante],[num_consulta]" +
		"FROM vw_exames_internos_marcados " +
		"WHERE status = 1 " + //status 4 - aquardando vaga
		"AND impr = 0 " +
		"AND  cod_fila = " + cod_fila;

		using (SqlConnection conn = new SqlConnection(strConexao))
		{
			SqlDataReader dr1 = null;
			SqlCommand cmd = new SqlCommand(strQuery, conn);
			DataTable dt = new DataTable();

			try
			{
				conn.Open();
				dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);

				dt.Columns.Add("Cod_Fila", System.Type.GetType("System.String"));
				dt.Columns.Add("Cod_Exame", System.Type.GetType("System.String"));
				dt.Columns.Add("Data", System.Type.GetType("System.String"));
				dt.Columns.Add("Nº Consulta", System.Type.GetType("System.String"));
				dt.Columns.Add("Executante", System.Type.GetType("System.String"));

				while (dr1.Read())
				{
					//string fila = dr1.GetInt32(0).ToString();
					//string codigo = dr1.GetInt32(1).ToString();
					string exame = dr1.GetString(1);
					string data = Convert.ToString(dr1.GetDateTime(2).ToShortDateString()) + " as " + dr1.GetString(3);
					string numCon = dr1.GetString(5);
					string executante = dr1.GetString(4);

					dt.Rows.Add(new String[] { exame, data, numCon, executante });
				}

			}
			catch (Exception ex)
			{
				string erro = ex.Message;
			}
			finally
			{
				if (dr1 != null) dr1.Close();
			}
			return dt;
		}
	}


}
