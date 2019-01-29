using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;

/// <summary>
/// Summary description for Especialidade
/// </summary>
public class Especialidade
{
	public Especialidade()
	{
    }
		public static String getespec(string _cod)
        {
                string desc = "";
                string cod = "";
                string sub = "";

            string espec = _cod;
            if (espec.Length == 6)
            {
                cod = "0" + espec.Substring(0, 3);
                sub = espec.Substring(3, 2);
            }
            else if (espec.Length == 5)
            {
                cod = "00" + espec.Substring(0, 2);
                sub = espec.Substring(2, 2);
            }

            using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "Select ia9descr from inta9 where ia9codprin = '" + cod + "' and ia9codsub = '" + sub + "'";
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
