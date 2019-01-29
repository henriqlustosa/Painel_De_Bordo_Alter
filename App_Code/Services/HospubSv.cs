using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using CD.DAL.BusinessEntities;
using System.Collections.Generic;


/// <summary>
/// Summary description for HospubSv
/// </summary>
namespace CD.DAL.Services
{
    public class HospubSv
    {
        public HospubSv()
        {
        }

        public static Paciente getPacienteByRH(int _rh)
        {
            Paciente pc = new Paciente();
            using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm = cnn.CreateCommand();
                //cmm.CommandText = "Select ib6nome from intb6 where ib6regist = " + _rh.ToString();
                cmm.CommandText = "Select ib6prontuar, ib6pnome, ib6compos, ib6municip, ib6telef, ib6dtnasc from intb6 where ib6regist = " + _rh.ToString();
                cnn.Open();
                OdbcDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    pc.Id = _rh;
                    pc.Rf = dr.GetInt32(0);
                    pc.Nome = dr.GetString(1) + dr.GetString(2);
                }
            }
            return pc;
        }

        public static Paciente getPacienteByBE(int _be)
        {
            Paciente pc = new Paciente();
            using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "Select c54identific from cen54 where n54numbolet = " + _be.ToString();
                cnn.Open();
                OdbcDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    pc.Nome = dr.GetString(0);
                    pc.Id = _be;
                }
            }
            return pc;
        }

        public static SqlDS.MedicosDataTable getAllMedicos()
        {
            SqlDS.MedicosDataTable mds = new SqlDS.MedicosDataTable();
            using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "Select ic0cpf,ic0nome from intc0 where ic0nome not like '%(ENF)%' and ic0nome not like '%(AUX. ENF.)%' "
                                    + " and ic0nome not like '%(AUX ENF)%' and ic0nome not like '%(ENF.)%'    order by ic0nome";
                cnn.Open();
                OdbcDataReader dr = cmm.ExecuteReader();
                SqlDS.MedicosRow md;
                while (dr.Read())
                {
                    md =  mds.NewMedicosRow();
                    md.Cpf = Convert.ToInt64(dr.GetDecimal(0));
                    md.Nome = dr.GetString(1);
                    mds.Rows.Add(md);
                }
            }
            return mds;        
        }

        public static SqlDS.ProcedenciaDataTable getProceds(string _setor)
        {
           SqlDS.ProcedenciaDataTable mds = new SqlDS.ProcedenciaDataTable();
    
                
                if (_setor.Equals("2") || _setor.Equals("1"))
                {
                    WSHospub.Serv sv = new WSHospub.Serv();
                    WSHospub.Clinica[] clins= sv.getClinicas();
                    SqlDS.ProcedenciaRow md;
                   foreach(WSHospub.Clinica cl in clins)
                    {
                        md = mds.NewProcedenciaRow();
                        md.ID = cl.Codigo;
                        md.Descr = cl.Nome;
                        mds.Rows.Add(md);
                    }
                }
                else
                {
                    if (_setor.Equals("3"))
                    {
                        WSHospub.Serv sv = new WSHospub.Serv();
                        WSHospub.SetorPS[] sps = sv.getSetoresPS();                      
                        SqlDS.ProcedenciaRow md;
                        foreach (WSHospub.SetorPS sp in sps)
                        {
                            md = mds.NewProcedenciaRow();
                            md.ID = sp.Codigo;
                            md.Descr = sp.Nome;
                            mds.Rows.Add(md);
                        }
                    }


                }
                return mds;
           
        }



     

    }

}