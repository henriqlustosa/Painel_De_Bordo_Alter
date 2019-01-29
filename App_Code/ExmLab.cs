using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;

/// <summary>
/// Summary description for ExmLab
/// </summary>
public class ExmLab
{
	public ExmLab()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static String getRequis(string _requis)
    {
        string dt = "";
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            string requis = _requis.Replace(".", "");
            
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select c27labanoped, c27labareped, c27labpriped, c27labseqped from lab27 where c27labconsul = '" + requis +"'" ;
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();

            if (dr1.Read())
            {
                string requisicao = dr1.GetString(0) + dr1.GetString(1) + dr1.GetString(2) +dr1.GetString(3);
                dt = requisicao;

            }
            return dt;
        }
    }
}
