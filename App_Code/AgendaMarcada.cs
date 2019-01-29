using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for AgendaMarcada
/// </summary>
public class AgendaMarcada
{
	public AgendaMarcada()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //Carrega agenda dp Paciente
    public static DataTable gridCarregaAgenda(string _rh)
    {

        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select am1106,am1101a, am1101b, am1101c, am1104, am1103 from am11 where am1110 = 1 and am1108 = " + _rh + " Order By am1106";
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("Data", System.Type.GetType("System.String"));
            dt.Columns.Add("Consulta", System.Type.GetType("System.String"));
            dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
            dt.Columns.Add("Profissional", System.Type.GetType("System.String"));

            while (dr1.Read())
            {
                string data = dr1.GetString(0);
                int ano = Convert.ToInt32(data.Substring(0, 4));
                int mes = Convert.ToInt32(data.Substring(4, 2));
                int dia = Convert.ToInt32(data.Substring(6, 2));
                data = dia + "/" + mes + "/" + ano;

                string consulta = /* dr1.GetString(1) + */ dr1.GetString(2) + dr1.GetString(3);
                string espec = dr1.GetString(4);
                espec = Especialidade.getespec(espec);
                string profissional = Profissional.getNome(dr1.GetString(5));

                dt.Rows.Add(new String[] { data, consulta, espec, profissional });

            }
            return dt;
        }
    }

}
