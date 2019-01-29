using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

public partial class Pesquisa_Rh : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Button2.Enabled = false;


            string dtHoje = DateTime.Now.Date.ToShortDateString();
            txbdtSol.Text = dtHoje;

            ddlEspecialidades();

            string sSqlm = "Select ic0nome from intc0 Where ic0nome not like 'Z %' AND ic0nome not like '%RESIDENTE%' ORDER BY ic0nome";
            OdbcConnection con1 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString());
            OdbcCommand cmd1 = new OdbcCommand(sSqlm, con1);
            OdbcDataAdapter da1 = new OdbcDataAdapter(cmd1);

            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            ddlSol.DataTextField = ds1.Tables[0].Columns["ic0nome"].ToString();
            ddlSol.DataSource = ds1.Tables[0];
            ddlSol.DataBind();
            con1.Close();
        }
    }

    private DataTable Especialidades(string sql)
    {
        OdbcConnection con = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString());
        OdbcCommand cmd = new OdbcCommand(sql, con);
        OdbcDataAdapter da = new OdbcDataAdapter(cmd);

        DataTable dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

    private void ddlEspecialidades()
    {
        string sql = "Select ia9codprin, ia9codsub, ia9descr From inta9 Where ia9descr not like 'X%' order by ia9descr";

        DataTable dt = new DataTable();
        dt = Especialidades(sql);
        ddlEspec.DataSource = dt;
        ddlEspec.DataTextField = "ia9descr";
        ddlEspec.DataValueField = "ia9descr";
        ddlEspec.DataBind();
    }


    protected void GridView6_SelectedIndexChanged(object sender, EventArgs e)
    {  
        Button2.Enabled = true;
        lbcod.Text = GridView6.SelectedRow.Cells[1].Text.Replace("&nbsp;", " "); ;
        txbdtSol.Text = GridView6.SelectedRow.Cells[2].Text.Replace("&nbsp;", " "); ;
        ddlEspec.SelectedItem.Text = GridView6.SelectedRow.Cells[3].Text.Replace("&nbsp;", " "); ;
        ddlSol.SelectedItem.Text = GridView6.SelectedRow.Cells[4].Text.Replace("&nbsp;", " "); ;

        string _ret = GridView6.SelectedRow.Cells[5].Text;
        string _reg = GridView6.SelectedRow.Cells[6].Text;
        txbTel2.Text = "";
        txbTel3.Text = "";
        txbTel4.Text = "";
        if (_ret.Equals("Sim"))
        {
            chbRetorno.Checked = true;
        }
        else
        {
            chbRetorno.Checked = false;
        }
        if (_reg.Equals("Sim"))
        {
            chbRegul.Checked = true;
        }
        else
        {
            chbRegul.Checked = false;
        }
        string registro = txbRh.Text;

        txbqtd.Text = GridView6.SelectedRow.Cells[7].Text.Replace("&nbsp;", " ");

        ddlSituacao.Text = GridView6.SelectedRow.Cells[8].Text.Replace("&nbsp;", " ");

        ddlMarcada.SelectedValue = GridView6.SelectedRow.Cells[9].Text.Replace("&nbsp;", " ");

        txbConsulta.Text = GridView6.SelectedRow.Cells[10].Text.Replace("&nbsp;", " ");
        txbDtCon.Text = GridView6.SelectedRow.Cells[11].Text.Replace("&nbsp;", " ");
        txbHoraCon.Text = GridView6.SelectedRow.Cells[12].Text.Replace("&nbsp;", " ");

        txbTel1.Text = GridView6.SelectedRow.Cells[13].Text.Replace("&nbsp;", " ");
        string observacao = GridView6.SelectedRow.Cells[14].Text.Replace("&nbsp;", " ");
        txbObs.Text = HttpUtility.HtmlDecode(observacao);
      
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT telefone2,telefone3,telefone4 FROM Telefones_Adicionais WHERE rh='" + registro + "'";

            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    txbTel2.Text = dr.GetString(0);
                    txbTel3.Text = dr.GetString(1);
                    txbTel4.Text = dr.GetString(2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }
    }

    protected void btPesq_Click(object sender, EventArgs e)
    {
        Button2.Enabled = false;
        string _rh = txbRh.Text;
        LimpaCampos();
        string dtHoje = DateTime.Now.Date.ToShortDateString();
        txbdtSol.Text = dtHoje;
        txbTel2.Text = "";
        txbTel3.Text = "";
        txbTel4.Text = "";

        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select ib6pnome, ib6compos, ib6municip, ib6telef, ib6dtnasc from intb6 where ib6regist = " + _rh;
            cnn.Open();
            OdbcDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                lbNome.Text = dr.GetString(0) + dr.GetString(1);

                string tel = dr.GetString(3).TrimStart('0');
                if (tel != "")
                {
                    if (tel.Length.Equals(8))
                    {
                        tel = tel.Substring(0, 4) + "-" + tel.Substring(4, 4);
                        txbTel1.Text = "(11)  0" + tel;

                    }
                    else if (tel.Length.Equals(9))
                    {
                        tel = tel.Substring(0, 5) + "-" + tel.Substring(5, 4);
                        txbTel1.Text = "(11) " + tel;
                    }
                    else if (tel.Length.Equals(10))
                    {
                        tel = "(" + tel.Substring(0, 2) + ") 0" + tel.Substring(2, 4) + "-" + tel.Substring(6, 4);
                        txbTel1.Text = tel;
                    }
                    else if (tel.Length.Equals(11))
                    {
                        tel = "(" + tel.Substring(0, 2) + ") " + tel.Substring(2, 5) + "-" + tel.Substring(7, 4);
                        txbTel1.Text = tel;
                    }
                }
                lbIdade.Text = calcidade.getIdade(dr.GetString(4)).ToString() + " anos"; 

                dr.Close();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Número de RH inexistente.');", true);
                txbRh.Text = "";
                LimpaCampos();
                dtHoje = DateTime.Now.Date.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Year.ToString().PadLeft(2, '0');
                txbdtSol.Text = dtHoje;
                return;
            }
            //Colocar uma função para buscar os telefones no Banco de Dados Geral_Treina

            using (SqlConnection cnn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {

                SqlCommand cmm1 = cnn1.CreateCommand();
                cmm1.CommandText = "SELECT  telefone2,telefone3,telefone4 FROM [Geral_Treina].[dbo].[Telefones_Adicionais] where rh = '" + _rh + "'";
                cnn1.Open();

                SqlDataReader drTel = cmm1.ExecuteReader();
                if (drTel.Read())
                {
                    txbTel2.Text = drTel.GetString(0);
                    txbTel3.Text = drTel.GetString(1);
                    txbTel4.Text = drTel.GetString(2);
                }
            }
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string err = "";
        int erro = 0;
        string codigo = lbcod.Text;
        string usuario = User.Identity.Name;
        string rh = txbRh.Text;
        string dtSolicitacao = txbdtSol.Text;
        string especialidade = ddlEspec.SelectedItem.ToString();
        string solicitante = ddlSol.SelectedItem.ToString();
        int _qdt = Convert.ToInt32(txbqtd.Text); //converter para Int32
        string _retorno = "";
        string _regulacao = "";
        DateTime dataMomento = DateTime.Now;


        if (chbRetorno.Checked)
        {
            _retorno = "Sim";
        }
        else
        {
            _retorno = "Não";
        }

        if (chbRegul.Checked)
        {
            _regulacao = "Sim";
        }
        else
        {
            _regulacao = "Não";
        }

        string situacao = ddlSituacao.SelectedValue;
        string marcada = ddlMarcada.SelectedValue;
        string consulta = txbConsulta.Text;
        string dtConsulta = txbDtCon.Text;
        string hrConsulta = txbHoraCon.Text;
        string obs = txbObs.Text;
        string tel1 = txbTel1.Text;
        string tel2 = txbTel2.Text;
        string tel3 = txbTel3.Text;
        string tel4 = txbTel4.Text;


        if (codigo != "")
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = cnn.CreateCommand();

                //atualizar
                cmm.CommandText = "UPDATE Fila SET rh = @rh, data = @data, especialidade = @especialidade, solicitante = @solicitante, " +
                    " qtdExames = @qtdExames,retorno = @retorno, regulacao = @regulacao,situacao = @situacao, marcada = @marcada," +
                    "consulta = @consulta,dtCon = @dtCon, hrCon = @hrCon ,telefone = @telefone," +
                    "obs = @obs, usuario = @usuario WHERE cod = " + codigo + ";";

                //atualizar
                cmm.Parameters.Add("@rh", SqlDbType.VarChar).Value = rh;
                cmm.Parameters.Add("@data", SqlDbType.Date).Value = dtSolicitacao;
                cmm.Parameters.Add("@especialidade", SqlDbType.VarChar).Value = especialidade;
                cmm.Parameters.Add("@solicitante", SqlDbType.VarChar).Value = solicitante;
                cmm.Parameters.Add("@qtdExames", SqlDbType.Int).Value = _qdt;
                cmm.Parameters.Add("@retorno", SqlDbType.VarChar).Value = _retorno;
                cmm.Parameters.Add("@regulacao", SqlDbType.VarChar).Value = _regulacao;
                cmm.Parameters.Add("@situacao", SqlDbType.VarChar).Value = situacao;
                cmm.Parameters.Add("@marcada", SqlDbType.VarChar).Value = marcada;
                cmm.Parameters.Add("@consulta", SqlDbType.VarChar).Value = consulta.Trim();
                cmm.Parameters.Add("@dtCon", SqlDbType.VarChar).Value = dtConsulta;
                cmm.Parameters.Add("@hrCon", SqlDbType.VarChar).Value = hrConsulta;
                cmm.Parameters.Add("@telefone", SqlDbType.VarChar).Value = tel1;
                cmm.Parameters.Add("@obs", SqlDbType.VarChar).Value = obs;
                cmm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                try
                {
                    cnn.Open();
                    cmm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    erro = 1;
                }
            }
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "SELECT * FROM Telefones_Adicionais WHERE rh='" + rh + "'";

                try
                {
                    cnn.Open();
                    SqlDataReader dr = cmm.ExecuteReader();
                    if (dr.HasRows == false && (tel2 != "" || tel3 != "" || tel4 != ""))
                    {
                        using (SqlConnection cnn5 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                        {
                            SqlCommand cmm5 = cnn5.CreateCommand();
                            cmm5.CommandText = "INSERT INTO Telefones_Adicionais (rh, telefone2, telefone3, telefone4, DataDeCadastro, DataDaUltimaAtualizacao,usuario) VALUES (@rh,@telefone2,@telefone3,@telefone4,@DataDeCadastro,@DataDaUltimaAtualizacao,@usuario)";
                            cmm5.Parameters.Add("@rh", SqlDbType.VarChar).Value = rh;
                            cmm5.Parameters.Add("@telefone2", SqlDbType.VarChar).Value = tel2;
                            cmm5.Parameters.Add("@telefone3", SqlDbType.VarChar).Value = tel3;
                            cmm5.Parameters.Add("@telefone4", SqlDbType.VarChar).Value = tel4;
                            cmm5.Parameters.Add("@DataDeCadastro", SqlDbType.DateTime).Value = dataMomento;
                            cmm5.Parameters.Add("@DataDaUltimaAtualizacao", SqlDbType.DateTime).Value = dataMomento;
                            cmm5.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                            try
                            {
                                cnn5.Open();
                                cmm5.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                err = ex.Message;
                                erro = 1;
                            }

                        }

                    }
                    else if (dr.HasRows == true)
                    {
                        using (SqlConnection cnn6 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                        {
                            SqlCommand cmm6 = cnn6.CreateCommand();

                            //atualizar
                            cmm6.CommandText = "UPDATE Telefones_Adicionais SET telefone2 = @telefone2, telefone3 = @telefone3, telefone4 = @telefone4, DataDaUltimaAtualizacao = @DataDaUltimaAtualizacao, usuario = @usuario " +
                                " WHERE rh = '" + rh + "'";

                            //atualizar
                            cmm6.Parameters.Add("@telefone2", SqlDbType.VarChar).Value = tel2;
                            cmm6.Parameters.Add("@telefone3", SqlDbType.VarChar).Value = tel3;
                            cmm6.Parameters.Add("@telefone4", SqlDbType.VarChar).Value = tel4;
                            cmm6.Parameters.Add("@DataDaUltimaAtualizacao", SqlDbType.DateTime).Value = dataMomento;
                            cmm6.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;


                            try
                            {
                                cnn6.Open();
                                cmm6.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                err = ex.Message;
                                erro = 1;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    erro = 1;
                }

            }
            dtSolicitacao = "";
            consulta = "";
            obs = "";
            tel1 = "";
            if (erro.Equals(0))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Atualizado com sucesso!');", true);
            }
            else if (erro.Equals(1))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro ao atualizar: " + err + "');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro: Contate o administrador');", true); //mensagem de alerta de exames realizados do paciente antes de um ano
            }

        }
        LimpaCampos();
        string dtHoje = DateTime.Now.Date.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Year.ToString().PadLeft(2, '0');
        txbdtSol.Text = dtHoje;
        txbRh.Text = "";
        txbRh.Focus();
        GridView6.SelectedIndex = -1;   
    }
    public void LimpaCampos()
    {
        
        lbNome.Text = "";
        lbIdade.Text = "";
     
      
           
      
        
        ddlMarcada.SelectedIndex = 0;
        ddlEspec.SelectedIndex = 0;
        ddlSituacao.SelectedIndex = 0;
        ddlSol.SelectedIndex = 0;
        txbDtCon.Text = "";
        txbHoraCon.Text = "";
        lbcod.Text = "";
        txbdtSol.Text = "";
        txbqtd.Text = "";
        txbConsulta.Text = "";
        txbObs.Text = "";
        txbTel1.Text = "";
        txbTel2.Text = "";
        txbTel3.Text = "";
        txbTel4.Text = "";
        chbRetorno.Checked = true;
        chbRegul.Checked = false;
    }
    
}
