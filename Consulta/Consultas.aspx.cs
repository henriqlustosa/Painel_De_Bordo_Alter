using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;

public partial class Consulta_Consultas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btPesq_Click(object sender, EventArgs e)
    {
        string _rh = txbRh.Text;
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select ib6prontuar, ib6pnome, ib6compos, ib6municip, ib6telef, ib6dtnasc, ib6lograd, ib6numero, ib6complem, ib6bairro from intb6 where ib6regist = " + _rh;
            cnn.Open();
            OdbcDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                lbRF.Text = dr.GetString(0);
                lbNome.Text = dr.GetString(1) + dr.GetString(2);
                lbProcedencia.Text = Procedencia.getProcedencia(dr.GetString(3));

                string tel = dr.GetString(4).TrimStart('0');
                if (tel != "")
                {
                    if (tel.Length.Equals(8))
                    {
                        tel = tel.Substring(0, 4) + "-" + tel.Substring(4, 4);
                    }
                    else if (tel.Length.Equals(9))
                    {
                        tel = tel.Substring(0, 5) + "-" + tel.Substring(5, 4);
                    }
                }
                lbTel.Text = tel;
                lbIdade.Text = calcidade.getIdade(dr.GetString(5)).ToString() + " anos"; ;
                lblograd.Text = dr.GetString(6);
                lbnumero.Text = dr.GetString(7);
                lbcomplem.Text = dr.GetString(8);
                lbbairro.Text = dr.GetString(9);

                dr.Close();
            }
        }

        //Pesquisa Histórico de Consultas
        GridView1.DataSource = AgendaPass1.gridCarregaAgendaPass(_rh);
        GridView1.DataBind();

        // Consultas Marcadas
        GridView2.DataSource = AgendaMarcada.gridCarregaAgenda(_rh);
        GridView2.DataBind();

        GridView3.DataSource = AgendaDemanda.gridCarregaAgendaDemanda(_rh);
        GridView3.DataBind();
    }
}
