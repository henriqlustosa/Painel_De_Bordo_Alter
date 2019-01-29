using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for AgendaDemanda
/// </summary>
public class AgendaDemanda
{
	public AgendaDemanda()
	{
		
	}
    //Carrega agenda Demanda Paciente
    public static DataTable gridCarregaAgendaDemanda(string _rh)
    {
        string dtAtual = DateTime.Now.ToString("yyyyMMdd");

        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select am5605,am5602, am5608b, am5608c from am56 where am5613 = 1 and am5605 >= '" + dtAtual + "' and am5604 = " + _rh + " order By am5605 desc ";
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("Data", System.Type.GetType("System.String"));
            dt.Columns.Add("Consulta", System.Type.GetType("System.String"));
            dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
            //dt.Columns.Add("Status", System.Type.GetType("System.String"));

            while (dr1.Read())
            {
                string data = calcidade.DataFormatada(dr1.GetString(0));

                string espec = dr1.GetString(1);
                string consul = dr1.GetString(2) + dr1.GetString(3);
                //string status = dr1.GetString(4);

                espec = Especialidade.getespec(espec);

                dt.Rows.Add(new String[] { data, consul, espec });
            }
            return dt;
        }
    }

}
