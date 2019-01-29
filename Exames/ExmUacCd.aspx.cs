using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;

public partial class Exames_ExmUacCd : System.Web.UI.Page
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


        GridView4.DataSource = ExmUac.gridExmUac(_rh);
        GridView4.DataBind();

        GridView5.DataSource = AgendaCd.gridCarregaExamesCd(_rh);
        GridView5.DataBind();
    }

    protected void cor1GridView_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        //colorindo uma linha com base no conteúdo de uma célula
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (e.Row.Cells[2].Text)
            {
                case "CHECKOUT":
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Yellow;
                    break;
                case "REALIZADO":
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Green;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                    break;
                case "CHECKIN":
                    e.Row.BackColor = System.Drawing.Color.Orange;
                    break;
            }
        }

    }
    ////// fim corGridView
}
