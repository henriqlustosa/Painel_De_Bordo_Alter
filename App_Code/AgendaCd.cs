
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for AgendaCd
/// </summary>
public class AgendaCd
{
	public AgendaCd()
	{
		
	}
    // Carrega Exames realizadas nos últimos 6 meses
    public static DataTable gridCarregaExamesCd(string _rh)
    {
        string strConexao = @"Data Source=hspmins4;Initial Catalog=Centro Diagnostico;User Id=h010994;Password=soundgarden";
        string strQuery = "SELECT p.[Num_Pedido],[Procedencia],s.[Descricao],g.[Descricao],c.[DataHora]" +
                            "FROM [Centro Diagnostico].[dbo].[Pedidos] p,[Centro Diagnostico].[dbo].[Checkin_Pedido] c, [Centro Diagnostico].[dbo].[Status] s, [Centro Diagnostico].[dbo].[Grupo_Exame] g " +
                            "WHERE DataHora >= (getdate() - 180 ) " +
                            "and p.Num_Pedido = c.Num_Pedido and s.Cod_Status = p.Cod_Status and p.Cod_Grupo_Exame = g.Cod_Grupo_Exame " +
                            "and ID_Paciente = " + _rh;

        using(SqlConnection conn = new SqlConnection(strConexao)){
            SqlDataReader dr1 = null;
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                
                dt.Columns.Add("Pedido", System.Type.GetType("System.String"));
                dt.Columns.Add("Procedencia", System.Type.GetType("System.String"));
                dt.Columns.Add("Status", System.Type.GetType("System.String"));
                dt.Columns.Add("Exame", System.Type.GetType("System.String"));
                dt.Columns.Add("Data", System.Type.GetType("System.String"));

                while (dr1.Read())
                {

                    string pedido = dr1.GetInt32(0).ToString();
                    string procedencia = dr1.GetString(1);
                    string status = dr1.GetString(2);
                    string exam = dr1.GetString(3);

                    string data = dr1.GetDateTime(4).ToShortDateString();

                    dt.Rows.Add(new String[] { pedido, procedencia, status, exam, data });
                    dr1.Close();
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
