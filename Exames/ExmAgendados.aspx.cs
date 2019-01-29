using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Exames_CadExm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
           
        //}
    }
    protected void btPesq_Click(object sender, EventArgs e)
    {
        string _rh = txbRh.Text;
        //LimpaCampos();
        string dtHoje = DateTime.Now.Date.ToShortDateString();
        //txbdtSol.Text = dtHoje;
        //txbSus.Text = Sus(_rh);

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
                //lbTel.Text = tel;
                lbIdade.Text = calcidade.getIdade(dr.GetString(5)).ToString() + " anos"; ;

                dr.Close();
            }
        }
        GridView3.DataSource = gridCarregaExames(_rh);
        GridView3.DataBind();

        GridView1.DataSource = gridCarregaExamesMarcados(_rh);
        GridView1.DataBind();
    }
   
    protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
    {
            string _rh = txbRh.Text;
            string _pac = lbNome.Text;
            string _fila = GridView3.SelectedRow.Cells[1].Text;
            string _exame = GridView3.SelectedRow.Cells[3].Text;
            string _solic = GridView3.SelectedRow.Cells[4].Text;

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(350/2);var Mtop = (screen.height/2)-(200/2);window.open( '../Exames/popupExames.aspx?rh=" + _rh + "&pac=" + _pac + "&exm=" + _exame + "&fila=" + _fila + "&solic="+_solic+"', null, 'height=200,width=350,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);

            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(400/2);window.open( '../Exames/cadDataExame.aspx?rh=" + _rh + "&pac=" + _pac + "&exm="+_exame+"&fila="+ _fila +"', null, 'height=400,width=780,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
      
    }



    public static string getExames(int cod)
    {
        string descr = "";
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select Descricao from Exames where Cod_Exame = " + cod;
            cnn.Open();
            SqlDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                descr = dr.GetString(0);
            }
        }
        return descr;
    }

    // Carrega Exames aquardando vaga
    public static DataTable gridCarregaExames(string _rh)
    {
        string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
        string strQuery = "SELECT cod,cod_exame, solicitante " +
                            "FROM Exames_Paciente " +
                            "WHERE status = 4 " + //status 4 - aquardando vaga
                            "AND rh = " + _rh;

        using (SqlConnection conn = new SqlConnection(strConexao))
        {
            SqlDataReader dr1 = null;
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                dt.Columns.Add("Fila", System.Type.GetType("System.String"));
                dt.Columns.Add("Codigo", System.Type.GetType("System.String"));
                dt.Columns.Add("Descrição", System.Type.GetType("System.String"));
                dt.Columns.Add("Solicitante", System.Type.GetType("System.String"));

                while (dr1.Read())
                {
                    string fila = dr1.GetInt32(0).ToString();
                    string codigo = dr1.GetInt32(1).ToString();
                    string descricao = getExames(dr1.GetInt32(1));
                    string solic = dr1.GetString(2);

                    dt.Rows.Add(new String[] { fila, codigo, descricao, solic });
                }

            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }
            finally
            {
                if (dr1 != null) dr1.Close();
            }
            return dt;
        }
    }


    // Carrega Exames marcados e não impressos
    public static DataTable gridCarregaExamesMarcados(string _rh)
    {
        string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";

        string strQuery = "SELECT [cod],[Descricao],[dt_consulta]" +
        ",[hr_consulta],[executante],[num_consulta]" +
        "FROM vw_exames_internos_marcados " +
        "WHERE status = 1 " + //status 4 - aquardando vaga
        "AND impr = 0 " +
        "AND rh = " + _rh;

        using (SqlConnection conn = new SqlConnection(strConexao))
        {
            SqlDataReader dr1 = null;
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                //dt.Columns.Add("Fila", System.Type.GetType("System.String"));
                dt.Columns.Add("Exame", System.Type.GetType("System.String"));
                dt.Columns.Add("Data", System.Type.GetType("System.String"));
                dt.Columns.Add("Nº Consulta", System.Type.GetType("System.String"));
                dt.Columns.Add("Executante", System.Type.GetType("System.String"));

                while (dr1.Read())
                {
                    //string fila = dr1.GetInt32(0).ToString();
                    //string codigo = dr1.GetInt32(1).ToString();
                    string exame = dr1.GetString(1);
                    string data = Convert.ToString(dr1.GetDateTime(2).ToShortDateString()) + " as " + dr1.GetString(3);
                    string numCon = dr1.GetString(5);
                    string executante = dr1.GetString(4);

                    dt.Rows.Add(new String[] { exame, data, numCon, executante });
                }

            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }
            finally
            {
                if (dr1 != null) dr1.Close();
            }
            return dt;
        }
    }
}
