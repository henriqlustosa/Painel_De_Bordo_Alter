using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;

/// <summary>
/// Summary description for Profissional
/// </summary>
public class Profissional
{
	public Profissional()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static String getNome(string _cod)
    {  
        string desc = "";
        
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select ic0nome from intc0 where ic0cpf = " + _cod;
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
