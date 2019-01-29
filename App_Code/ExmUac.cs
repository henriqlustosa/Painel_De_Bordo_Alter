using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for ExmUac
/// </summary>
public class ExmUac
{
	public ExmUac()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static DataTable gridExmUac(string _rh)
    {
        using (MySqlConnection cnn = new MySqlConnection(ConfigurationManager.ConnectionStrings["uacConnectionString"].ToString()))
        {
            MySqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT data_cad, exame, clinica, medico, c.nome FROM cadexmuac e, clin_uac c, cadguiasuac g  WHERE e.id = g.cadexmuac_id and c.id_clin = g.clin_uac_id_clin and data_cad > (date(now()) - 180) and rh = " + _rh;
            cnn.Open();
            MySqlDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Cadastro", System.Type.GetType("System.String"));
            dt.Columns.Add("Exame", System.Type.GetType("System.String"));
            dt.Columns.Add("Clínica", System.Type.GetType("System.String"));
            dt.Columns.Add("Médico", System.Type.GetType("System.String"));
            dt.Columns.Add("Local", System.Type.GetType("System.String"));

            while (dr1.Read())
            {
                string data = dr1.GetString(0).Substring(0, 10);
                string exm = dr1.GetString(1);
                string clinica = dr1.GetString(2);
                string medico = dr1.GetString(3);
                string local = dr1.GetString(4);

                dt.Rows.Add(new String[] { data, exm, clinica, medico, local });
                
            }
            return dt;
        }
    }

}
