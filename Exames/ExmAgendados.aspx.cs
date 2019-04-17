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
        string strQuery = "SELECT cod_fila,cod_exame, solicitante,especialidade,obs,cod,dataSolicitacao,dataAgendamento,falta " +
                            "FROM Exames_Paciente " +
                            "WHERE exameStatus in (1,6)" + //status 1 - aguardando vaga
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

                dt.Columns.Add("Cod_Fila", System.Type.GetType("System.String"));
                dt.Columns.Add("Codigo", System.Type.GetType("System.String"));
                //dt.Columns.Add("Cod_Exame", System.Type.GetType("System.String"));
                dt.Columns.Add("Solicitante", System.Type.GetType("System.String"));
                dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
                dt.Columns.Add("Grupo de Exames", System.Type.GetType("System.String"));
                dt.Columns.Add("Exames", System.Type.GetType("System.String"));
                dt.Columns.Add("Observacao", System.Type.GetType("System.String"));
                dt.Columns.Add("Data Solicitacao", System.Type.GetType("System.String"));
                dt.Columns.Add("Data Agendamento", System.Type.GetType("System.String"));
                dt.Columns.Add("Falta no Exame", System.Type.GetType("System.String"));


                while (dr1.Read())
                {
                    string codigo_fila = dr1.GetInt32(0).ToString();
                    //string codigo_exame = dr1.GetInt32(1).ToString();
                    string solicitante = dr1.GetString(2);
                    string especialidade = dr1.GetString(3);
                    string grupo_exames = getGrupoExame(dr1.GetInt32(1));
                    string exames = getExame(dr1.GetInt32(1));
                    string obs = dr1.GetString(4);
                    string codigo = dr1.GetInt32(5).ToString();
                    string dataSolicitacao = String.Format("{0:dd/MM/yyyy}", dr1.GetDateTime(6));
                    string dataAgendamento = dr1.GetDateTime(7).ToString().Equals("01/01/1900 00:00:00") ? "" : String.Format("{0:dd/MM/yyyy hh:mm}", dr1.GetDateTime(7)); ;
                    string falta = dr1.GetBoolean(8).ToString().Equals("True") ? "Sim" : "Não";

                    string especialidae = dr1.GetString(2);
                    dt.Rows.Add(new String[] { codigo_fila, codigo, solicitante, especialidade, grupo_exames, exames, obs, dataSolicitacao, dataAgendamento, falta });
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

        string strQuery = "SELECT cod_fila,cod_exame, solicitante,especialidade,obs,cod,dataSolicitacao,dataAgendamento,falta " +
                            "FROM Exames_Paciente " +
                            "WHERE exameStatus in (2,4)" + //status 2 - Agendadado e 4  - Reagendado
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

                dt.Columns.Add("Cod_Fila", System.Type.GetType("System.String"));
                dt.Columns.Add("Codigo", System.Type.GetType("System.String"));
                //dt.Columns.Add("Cod_Exame", System.Type.GetType("System.String"));
                dt.Columns.Add("Solicitante", System.Type.GetType("System.String"));
                dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
                dt.Columns.Add("Grupo de Exames", System.Type.GetType("System.String"));
                dt.Columns.Add("Exames", System.Type.GetType("System.String"));
                dt.Columns.Add("Observacao", System.Type.GetType("System.String"));
                dt.Columns.Add("Data Solicitacao", System.Type.GetType("System.String"));
                dt.Columns.Add("Data Agendamento", System.Type.GetType("System.String"));
                dt.Columns.Add("Falta no Exame", System.Type.GetType("System.String"));


                while (dr1.Read())
                {
                    string codigo_fila = dr1.GetInt32(0).ToString();
                    //string codigo_exame = dr1.GetInt32(1).ToString();
                    string solicitante = dr1.GetString(2);
                    string especialidade = dr1.GetString(3);
                    string grupo_exames = getGrupoExame(dr1.GetInt32(1));
                    string exames = getExame(dr1.GetInt32(1));
                    string obs = dr1.GetString(4);
                    string codigo = dr1.GetInt32(5).ToString();
                    string dataSolicitacao = String.Format("{0:dd/MM/yyyy}", dr1.GetDateTime(6));
                    string dataAgendamento = dr1.GetDateTime(7).ToString().Equals("01/01/1900 00:00:00") ? "" : String.Format("{0:dd/MM/yyyy hh:mm}", dr1.GetDateTime(7)); ;
                    string falta = dr1.GetBoolean(8).ToString().Equals("True") ? "Sim" : "Não";


                    string especialidae = dr1.GetString(2);
                    dt.Rows.Add(new String[] { codigo_fila, codigo, solicitante, especialidade, grupo_exames, exames, obs, dataSolicitacao, dataAgendamento, falta });
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
        public static string getExame(int cod)
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
        public static string getGrupoExame(int cod)
        {
            string descr = "";
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "SELECT g.Descricao FROM [Geral_Treina].[dbo].[Exames] as e join [Geral_Treina].[dbo].[Grupo_Exame] as g"
            + " on e.Cod_Grupo_Exame = g.Cod_Grupo_Exame where e.Cod_Exame =" + cod;
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    descr = dr.GetString(0);
                }
            }
            return descr;
        }
    protected void grvExamesSolicitados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
    }
    protected void grvExamesMarcados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
    }
}
