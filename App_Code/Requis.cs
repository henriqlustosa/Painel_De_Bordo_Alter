using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for Requis
/// </summary>
public class Requis
{
	public Requis()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable gridRequis(string _rh)
    {
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select c27labanoped,c27labareped, c27labpriped, c27labseqped, d27labrequis, "+
            " i27labcodesp, c27labconsul, " +
            " ic0nome " +
            " from lab27, intc0 " +
            " where i27labcpfmed = ic0cpf" +
            " and i27labregist = " + _rh +
            " order by d27labrequis desc";
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Requisição", System.Type.GetType("System.String"));
            dt.Columns.Add("Data Cadastro", System.Type.GetType("System.String"));
            dt.Columns.Add("Consulta", System.Type.GetType("System.String"));
            dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
            dt.Columns.Add("Profissional", System.Type.GetType("System.String"));
            int count = 0;

            while (dr1.Read() && count <= 5)
            {
                string requis = dr1.GetString(0) + dr1.GetString(1) + dr1.GetString(2) + dr1.GetString(3);
                string dtRequis = calcidade.DataFormatada(dr1.GetString(4));
                string espec = Especialidade.getespec(dr1.GetString(5));
                string consulta = dr1.GetString(6);
                string profissional = dr1.GetString(7);
                dt.Rows.Add(new String[] { requis, dtRequis, consulta, espec, profissional });
                count++;
            }
            return dt;
        }
    }

    public static DataTable gridExames(string _anoped, string _areped, string _priped, string _secped)
    {
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select c31labanoped,c31labareped, c31labpriped, c31labseqped, " +
            " c31labmarca, c31labcodexm, c19labdesc, c31labcodmot " +
            " from lab31, lab19 " +
            " where c31labanoped ='" + _anoped +"'"+
            " and c31labareped = '" + _areped +"'"+
            " and c31labpriped = '" + _priped +"'"+
            " and c31labseqped = '" + _secped +"'"+
            " and c31labcodexm = c19labcodexm ";
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Código Exame", System.Type.GetType("System.String"));
            dt.Columns.Add("Descrição Exame", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));
            dt.Columns.Add("Motivo", System.Type.GetType("System.String"));

            while (dr1.Read())
            {
                //string requis = dr1.GetString(0) + dr1.GetString(1) + dr1.GetString(2) + dr1.GetString(3);
                //string dtRequis = calcidade.DataFormatada(dr1.GetString(4));
                string statusExm = dr1.GetString(4);
                string codExm = dr1.GetString(5);
                string descr = dr1.GetString(6);
                string motivo = dr1.GetString(7);
                string status = "";
                if (statusExm.Equals("0"))
                {
                    status = "Aguardando Coleta";
                }
                else if (statusExm.Equals("1"))
                {
                    status = "Aguardando Impres de Mapa";
                }
                else if (statusExm.Equals("2"))
                {
                    status = "Aguardando Resultado do Exame";
                }
                else if (statusExm.Equals("3"))
                {
                    status = "Aguardando Liberação";
                }
                else if (statusExm.Equals("4"))
                {
                    status = "Aguardando Impressão";
                }
                else if (statusExm.Equals("5"))
                {
                    status = "Resultado Impresso";
                }
                else if (statusExm.Equals("6"))
                {
                    status = "Código sem utilização";
                }
                else if (statusExm.Equals("7"))
                {
                    status = "Coleta não realizada";
                }
                else if (statusExm.Equals("8"))
                {
                    status = "Exame não realizado";
                }
                else if (statusExm.Equals("9"))
                {
                    status = "Resultado do Exame Rejeitado";
                }
                else
                {
                    status = "Status desconhecido";
                }


                dt.Rows.Add(new String[] { codExm, descr, status, motivo });
            }
            return dt;
        }
    }

    // Carrega Requisição do Paciente
    public static DataTable gridCarregaRequis(string _rh)
    {

        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select c27labanoped,c27labareped, c27labpriped, c27labseqped, d27labrequis, d27labagdcol, c29labmarca " +
            " from lab27, lab29 " +
            " where c27labanoped = c29labanoped and c27labareped = c29labareped and c27labpriped = c29labpriped and c27labseqped = c29labseqped " +
            " and i27labregist = " + _rh;
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("Requisição", System.Type.GetType("System.String"));
            dt.Columns.Add("Data Cadastro", System.Type.GetType("System.String"));
            dt.Columns.Add("Data Coleta", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));

            while (dr1.Read())
            {
                string requis = dr1.GetString(0) + dr1.GetString(1) + dr1.GetString(2) + dr1.GetString(3);
                string dtRequis = calcidade.DataFormatada(dr1.GetString(4));
                string dtCol = calcidade.DataFormatada(dr1.GetString(5));
                string statusExm = dr1.GetString(6);
                string status = "";
                if (statusExm.Equals("0"))
                {
                    status = "Aguardando Coleta";
                }
                else if (statusExm.Equals("1"))
                {
                    status = "Aguardando Impres de Mapa";
                }
                else if (statusExm.Equals("2"))
                {
                    status = "Aguardando Resultado do Exame";
                }
                else if (statusExm.Equals("3"))
                {
                    status = "Aguardando Liberação";
                }
                else if (statusExm.Equals("4"))
                {
                    status = "Aguardando Impressão";
                }
                else if (statusExm.Equals("5"))
                {
                    status = "Resultado Impresso";
                }
                else if (statusExm.Equals("6"))
                {
                    status = "Código sem utilização";
                }
                else if (statusExm.Equals("7"))
                {
                    status = "Coleta não realizada";
                }
                else if (statusExm.Equals("8"))
                {
                    status = "Exame não realizado";
                }
                else if (statusExm.Equals("9"))
                {
                    status = "Resultado do Exame Rejeitado";
                }
                else
                {
                    status = "Status desconhecido";
                }


                dt.Rows.Add(new String[] { requis, dtRequis, dtCol, status });

            }
            return dt;
        }
    }
}
