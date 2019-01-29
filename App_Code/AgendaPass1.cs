using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for AgendaPass
/// </summary>
public class AgendaPass1
{
    public AgendaPass1()
    {
    }
    // Carrega consultas realizadas nos últimos 6 meses
    public static DataTable gridCarregaAgendaPass(string _rh)
    {
        string sangue = "";
        string urina = "";
        string fezes = "";
        string rx = "";
        string ecg = "";
        string outros = "";
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            string dataPass = "";
            int dtAtual = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
            int dtAno = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int dtMes = Convert.ToInt32(DateTime.Now.ToString("MM"));
            if (dtMes <= 6)
            {
                dtMes = dtMes + 6;
                dtAno = dtAno - 1;

                if (dtMes <= 3)
                {
                    dataPass = Convert.ToString(dtAno) + "0" + Convert.ToString(dtMes);
                }
                else
                {
                    dataPass = Convert.ToString(dtAno) + Convert.ToString(dtMes);
                }
            }
            else
            {
                dtMes = dtMes - 6;
                dataPass = Convert.ToString(dtAno) + "0" + Convert.ToString(dtMes);
            }


            OdbcCommand cmm = cnn.CreateCommand();
            //cmm.CommandText = "select am12121, am1212a, am1201a, am1201b, am1201c, am1204, am1203,am1210, am1208 from am12 where am12121 in(" + dataPass + "," + dtAtual + ")  and am1206 = " + _rh + " Order By am12121 desc";
            cmm.CommandText = "select am12121, am1212a, am1201a, am1201b, am1201c, am1204, "+
                " am1203,am1210, am1208 from am12 where am1206 = " + _rh + " Order By am12121 desc";
            cnn.Open();
            OdbcDataReader dr1 = cmm.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("Data", System.Type.GetType("System.String"));
            dt.Columns.Add("Consulta", System.Type.GetType("System.String"));
            dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
            dt.Columns.Add("Profissional", System.Type.GetType("System.String"));
            dt.Columns.Add("Saída", System.Type.GetType("System.String"));
            dt.Columns.Add("Recursos", System.Type.GetType("System.String"));
            dt.Columns.Add("Requisição", System.Type.GetType("System.String"));

            int count = 0;

            while (dr1.Read() && count <= 6)
            {
               
                    string data = dr1.GetString(0);
                    string ano = data.Substring(0, 4);
                    string mes = data.Substring(4, 2);
                    string dia = dr1.GetString(1).TrimEnd('.');
                    data = dia + "/" + mes + "/" + ano;

                    string consulta = dr1.GetString(2) + dr1.GetString(3) + dr1.GetString(4);
                    string requisicao = ExmLab.getRequis(consulta);

                    string espec = dr1.GetString(5);
                    espec = Especialidade.getespec(espec);
                    string profissional = Profissional.getNome(dr1.GetString(6));
                    string saida = dr1.GetString(7).TrimEnd('.');
                    if (saida.Equals("0"))
                    {
                        saida = "Faltou";
                    }
                    else if (saida.Equals("1"))
                    {
                        saida = "Alta";
                    }
                    else if (saida.Equals("4"))
                    {
                        saida = "Encam. Interno";
                    }
                    else if (saida.Equals("5"))
                    {
                        saida = "Outros";
                    }
                    else if (saida.Equals("6"))
                    {
                        saida = "Ret. Obrigatório";
                    }
                    else if (saida.Equals("7"))
                    {
                        saida = "Retorno";
                    }
                    string recursos = dr1.GetString(8);
                    if (recursos != "")
                    {


                        sangue = recursos.Substring(1, 1);
                        if (sangue.Equals("1"))
                        {
                            sangue = "Sangue";
                        }
                        else
                        {
                            sangue = "";
                        }
                        urina = recursos.Substring(2, 1);
                        if (urina.Equals("1"))
                        {
                            urina = "Urina";
                        }
                        else
                        {
                            urina = "";
                        }
                        fezes = recursos.Substring(4, 1);
                        if (fezes.Equals("1"))
                        {
                            fezes = "Fezes";
                        }
                        else
                        {
                            fezes = "";
                        }
                        outros = recursos.Substring(9, 1);
                        if (outros.Equals("1"))
                        {
                            outros = "Outros";
                        }
                        else
                        {
                            outros = "";
                        }
                        ecg = recursos.Substring(10, 1);
                        if (ecg.Equals("1"))
                        {
                            ecg = "ECG";
                        }
                        else
                        {
                            ecg = "";
                        }
                        rx = recursos.Substring(8, 1);
                        if (rx.Equals("1"))
                        {
                            rx = "Raio-X";
                        }
                        else
                        {
                            rx = "";
                        }

                    }
                    recursos = sangue + " " + urina + " " + fezes + " " + outros + " " + ecg + " " + rx;
                    recursos = recursos.TrimStart();
                    recursos = recursos.TrimEnd();
                    dt.Rows.Add(new String[] { data, consulta, espec, profissional, saida, recursos, requisicao });
                    count++;
                
            }
            return dt;
        }
    }
}
