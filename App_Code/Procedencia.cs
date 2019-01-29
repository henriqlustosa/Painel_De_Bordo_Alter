using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;

/// <summary>
/// Summary description for Procedencia
/// </summary>
public class Procedencia
{
	public Procedencia()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //Pesquisa Procedencia
    public static string getProcedencia(string _procedencia)
    {
        string uf = _procedencia.Substring(0, 2);
        string compos = _procedencia.Substring(2, 5);
        string desc = "";



        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select id4descricao from intd4 where id4uf = '" + uf + "' and id4compos = '" + compos + "'";
            cnn.Open();
            OdbcDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                desc = dr.GetString(0);
            }
        }
        return desc;
    }
}
