using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Consulta_DadosPaciente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbUser.Text = User.Identity.Name;

            Button1.Enabled = false;
            Button2.Enabled = false;


            string dtHoje = DateTime.Now.Date.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Year.ToString().PadLeft(2, '0');
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

    protected void btPesq_Click(object sender, EventArgs e)
    {
        GridView6.SelectedIndex = -1;
        string dtHoje = DateTime.Now.Date.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Year.ToString().PadLeft(2, '0');
        txbdtSol.Text = dtHoje;

        Button1.Enabled = true;
        Button2.Enabled = false;
        string _rh = txbRh.Text.TrimStart('0');
        LimpaCampos();

        txbdtSol.Text = dtHoje;
        txbSus.Text = Sus(_rh);
        txbTel2.Text = "";
        txbTel3.Text = "";
        txbTel4.Text = "";



        //*************data e hora do início de uso do painel contando apartir da pesquisa do rh
        lbDateIni.Text = DateTime.Now.ToString();

        //*********************************************
        chbRetorno.Checked = true;
        chbRegul.Checked = false;
        try
        {
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
                    lbTel.Text = tel;
                    lbIdade.Text = calcidade.getIdade(dr.GetString(5)).ToString() + " anos"; ;
                    lblograd.Text = dr.GetString(6);
                    lbnumero.Text = dr.GetString(7);
                    lbcomplem.Text = dr.GetString(8);
                    lbbairro.Text = dr.GetString(9);


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
        catch (Exception ex)
        {
            string erro = ex.Message;

        }
        ddlEspec.Focus();

    }

    public string Sus(string rh)
    {
        string _rh = rh;
        string _sus = "";
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select sus from cartao_sus where rh= " + _rh;
            cnn.Open();
            SqlDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                _sus = dr.GetString(0);
            }
        }

        return _sus;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        string _retorno;
        string _regulacao;
        int erro = 0;
        string err = "";

        string usuario = lbUser.Text;
        string rh = txbRh.Text.TrimStart('0');
        string dtSolicitacao = txbdtSol.Text;
        string especialidade = ddlEspec.SelectedItem.ToString();
        string espec = ddlEspec.SelectedValue.ToString();
        string solicitante = ddlSol.SelectedItem.ToString();

        string marcada = ddlMarcada.SelectedValue;
        string consulta = txbConsulta.Text;
        string dtConsulta = txbDtCon.Text;

        string hrConsulta = txbHoraCon.Text;
        string obs = txbObs.Text;
        string tel1 = txbTel1.Text;
        string tel2 = txbTel2.Text;
        string tel3 = txbTel3.Text;
        string tel4 = txbTel4.Text;
        bool impr = false;
        DateTime ini_atend = Convert.ToDateTime(lbDateIni.Text);//inicio da pesquisa/atendimento
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

        //_qtd = Convert.ToInt32(txbqtd.Text);


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
                        cmm6.CommandText = "UPDATE Telefones_Adicionais SET telefone2 = @telefone2, telefone3 = @telefone3, telefone4 = @telefone4, DataDaUltimaAtualizacao = @DataDaUltimaAtualizacao , usuario = @usuario" +
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


        DateTime data = DateTime.Now;

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "INSERT INTO Fila (rh, data, especialidade, solicitante, retorno, regulacao, qtdExames,situacao, " +
            " marcada, consulta,dtCon, hrCon, telefone, obs, usuario, impr, usuarioCadastro, dataCadastro) VALUES (@rh,@data,@especialidade, @solicitante, " +
            " @retorno, @regulacao, @qtdExames,@situacao, @marcada, @consulta,@dtCon,@hrCon, @telefone, @obs, @usuario, @impr,@usuarioCadastro, @dataCadastro)";

            cmm.Parameters.Add("@rh", SqlDbType.VarChar).Value = rh;
            cmm.Parameters.Add("@data", SqlDbType.Date).Value = dtSolicitacao;
            cmm.Parameters.Add("@especialidade", SqlDbType.VarChar).Value = especialidade;
            cmm.Parameters.Add("@solicitante", SqlDbType.VarChar).Value = solicitante;
            cmm.Parameters.Add("@retorno", SqlDbType.VarChar).Value = _retorno;
            cmm.Parameters.Add("@regulacao", SqlDbType.VarChar).Value = _regulacao;
            cmm.Parameters.Add("@qtdExames", SqlDbType.Int).Value = 0;
            cmm.Parameters.Add("@situacao", SqlDbType.VarChar).Value = "Cadastrado";
            cmm.Parameters.Add("@marcada", SqlDbType.VarChar).Value = marcada;
            cmm.Parameters.Add("@consulta", SqlDbType.VarChar).Value = consulta.Trim();
            cmm.Parameters.Add("@dtCon", SqlDbType.VarChar).Value = dtConsulta.Trim();
            cmm.Parameters.Add("@hrCon", SqlDbType.VarChar).Value = hrConsulta.Trim();
            cmm.Parameters.Add("@telefone", SqlDbType.VarChar).Value = tel1;
            cmm.Parameters.Add("@obs", SqlDbType.VarChar).Value = obs;
            cmm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
            cmm.Parameters.Add("@impr", SqlDbType.Bit).Value = impr;
            cmm.Parameters.Add("@usuarioCadastro", SqlDbType.VarChar).Value = usuario;
            cmm.Parameters.Add("@dataCadastro", SqlDbType.DateTime).Value = data;

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
        dtSolicitacao = "";
        consulta = "";
        obs = "";
        tel1 = "";
        tel2 = "";
        tel3 = "";
        tel4 = "";
        if (erro.Equals(0))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Gravado com sucesso!');", true);
        }
        else if (erro.Equals(1))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro ao gravar: " + err + "');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro: Contate o administrador');", true); //mensagem de alerta de exames realizados do paciente antes de um ano
        }

        //*********pega data e hora do fim do atendimento
        DateTime fim_atend = DateTime.Now;
        tempoAtend(rh, ini_atend, fim_atend);
       

        LimpaCampos();
        string dtHoje = DateTime.Now.Date.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Year.ToString().PadLeft(2, '0');
        txbdtSol.Text = dtHoje;
        txbRh.Text = "";
        txbRh.Focus();
        GridView6.SelectedIndex = -1;
    }


    //Calcula tempo de cadastro a partir do início da pesquisa do paciente ao cadastro de solicitação
    private void tempoAtend(string rh, DateTime ini_atend, DateTime fim_atend)
    {
        int _rh = Convert.ToInt32(rh);
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();

            //atualizar
            cmm.CommandText = "INSERT INTO Time_atend (cod_pac, time_ini, time_fim) VALUES (@cod,@ini, @fim)";

            cmm.Parameters.Add("@cod", SqlDbType.Int).Value = _rh;
            cmm.Parameters.Add("@ini", SqlDbType.DateTime).Value = ini_atend;
            cmm.Parameters.Add("@fim", SqlDbType.DateTime).Value = fim_atend;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }
    }
    //Atualizar
    protected void GridView6_SelectedIndexChanged(object sender, EventArgs e)
    {
        Button1.Enabled = false;
        Button2.Enabled = true;
        lbcod.Text = GridView6.SelectedRow.Cells[1].Text;
        txbdtSol.Text = GridView6.SelectedRow.Cells[2].Text;
        ddlEspec.SelectedItem.Text = GridView6.SelectedRow.Cells[3].Text;
        ddlSol.SelectedItem.Text = GridView6.SelectedRow.Cells[4].Text;
        string observacao = "";
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





        if (GridView6.SelectedRow.Cells[9].Text == "Sim")
        {
            ddlMarcada.SelectedValue = "Sim";
        }
        else
            ddlMarcada.SelectedValue = "Não";

        string registro = txbRh.Text.TrimStart('0');
        string especialidade = GridView6.SelectedRow.Cells[3].Text;
        string solicitante = GridView6.SelectedRow.Cells[4].Text;
        string codConsulta = "";
        string dataConsulta = "";
        string horaConsulta = "";
        string codEspecialidade = "";
        txbConsulta.Text = "";
        txbDtCon.Text = "";
        txbHoraCon.Text = "";
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

        try
        {
            using (OdbcConnection cnn4 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm4 = cnn4.CreateCommand();
                cmm4.CommandText = "select concat(ia9codprin,ia9codsub) from inta9 where ia9descr = '" + especialidade + "'";
                cnn4.Open();
                OdbcDataReader dr4 = cmm4.ExecuteReader();
                if (dr4.Read())
                {

                    codEspecialidade = dr4.GetString(0);

                    using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
                    {
                        OdbcCommand cmm = cnn.CreateCommand();
                        cmm.CommandText = "select am1101b,am1101c,am1106, am1113 from am11, intc0 where  am1103 = ic0cpf and am1108 = " + registro + "  and ic0nome = '" + solicitante + "' and am1104=" + codEspecialidade + " order by am1106";
                        cnn.Open();
                        OdbcDataReader dr = cmm.ExecuteReader();

                        while (dr.Read())
                        {

                            codConsulta = dr.GetDecimal(0).ToString().PadLeft(4, '0') + "-" + dr.GetDecimal(1).ToString();
                            dataConsulta = dr.GetDecimal(2).ToString();
                            horaConsulta = dr.GetDecimal(3).ToString();



                            using (SqlConnection cnn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                            {

                                SqlCommand cmm1 = cnn1.CreateCommand();
                                cmm1.CommandText = "SELECT  * FROM [Geral_Treina].[dbo].[Ret_marcadas_impr] where consulta = '" + codConsulta + "' and rh = " + registro + " and dtCon = '" + dataFormatada(dataConsulta) + "'";
                                cnn1.Open();

                                SqlDataAdapter da = new SqlDataAdapter(cmm1);

                                DataTable dt = new DataTable();
                                da.Fill(dt);


                                using (SqlConnection cnn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                                {
                                    SqlCommand cmm2 = cnn1.CreateCommand();
                                    cmm2.CommandText = "SELECT  * FROM [Geral_Treina].[dbo].[Fila] where consulta = '" + codConsulta + "' and rh = '" + registro + "' and dtCon = '" + dataFormatada(dataConsulta) + "'";
                                    cnn2.Open();
                                    SqlDataAdapter da2 = new SqlDataAdapter(cmm2);

                                    DataTable dt2 = new DataTable();
                                    da2.Fill(dt2);

                                    if (dt.Rows.Count == 0 && dt2.Rows.Count == 0)
                                    {
                                        txbConsulta.Text = codConsulta;
                                        txbDtCon.Text = dataFormatada(dataConsulta);
                                        txbHoraCon.Text = horaFormatada(horaConsulta);



                                        txbTel1.Text = GridView6.SelectedRow.Cells[13].Text;
                                        observacao = GridView6.SelectedRow.Cells[14].Text;
                                        txbObs.Text = HttpUtility.HtmlDecode(observacao);


                                        return;
                                    }
                                    else
                                    {
                                        string message = "alert('Consulta já cadastrada no Sistema Painel de Bordo')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                        txbConsulta.Text = GridView6.SelectedRow.Cells[10].Text;
                                        txbDtCon.Text = GridView6.SelectedRow.Cells[11].Text;
                                        txbHoraCon.Text = GridView6.SelectedRow.Cells[12].Text;

                                    }
                                }
                            }
                        }

                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("{0} Exception caught.", ex);
        }





        txbTel1.Text = GridView6.SelectedRow.Cells[13].Text;
        observacao = GridView6.SelectedRow.Cells[14].Text;
        txbObs.Text = HttpUtility.HtmlDecode(observacao);

    }






    protected void Button2_Click(object sender, EventArgs e)
    {

        string err = "";
        int erro = 0;
        string codigo = lbcod.Text;
        string usuario = lbUser.Text;
        string rh = txbRh.Text.TrimStart('0');
        string dtSolicitacao = txbdtSol.Text;
        string especialidade = ddlEspec.SelectedItem.ToString();
        string solicitante = ddlSol.SelectedItem.ToString();

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
                cmm.Parameters.Add("@qtdExames", SqlDbType.Int).Value = 0;
                cmm.Parameters.Add("@retorno", SqlDbType.VarChar).Value = _retorno;
                cmm.Parameters.Add("@regulacao", SqlDbType.VarChar).Value = _regulacao;
                cmm.Parameters.Add("@situacao", SqlDbType.VarChar).Value = "Cadastrado";
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
        AtualizarLog();
        LimpaCampos();
        string dtHoje = DateTime.Now.Date.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Date.Year.ToString().PadLeft(2, '0');
        txbdtSol.Text = dtHoje;
        txbRh.Text = "";
        txbRh.Focus();
        GridView6.SelectedIndex = -1;

    }
    public void AtualizarLog()
    {
        string codigo = lbcod.Text;
        string marcada = "";
        string usuario = User.Identity.Name; ;
        DateTime data = DateTime.Now;

        using (SqlConnection cnn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm2 = cnn2.CreateCommand();
            cmm2.CommandText = "SELECT marcada FROM [Geral_Treina].[dbo].[Fila] where cod ='" + codigo + "'";
            cnn2.Open();
            SqlDataReader dr = cmm2.ExecuteReader();
            if (dr.Read())
            {
                marcada = dr.GetString(0);
                if (marcada.Equals("Sim"))
                {
                    using (SqlConnection cnn6 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                    {
                        SqlCommand cmm6 = cnn6.CreateCommand();


                        cmm6.CommandText = "UPDATE Fila SET situacao = @situacao WHERE cod= '" + codigo + "'";



                        cmm6.Parameters.Add("@situacao", SqlDbType.VarChar).Value = "Marcado";



                        try
                        {
                            cnn6.Open();
                            cmm6.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string err = ex.Message;

                        }
                    }

                    //
                    using (SqlConnection cnn5 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                    {
                        SqlCommand cmm5 = cnn5.CreateCommand();
                        cmm5.CommandText = "INSERT INTO [Geral_Treina].[dbo].[Log_Fila] (statusFila, horaAtualizacaoStatus, usuario, cod_fila) VALUES (@statusFila,@horaAtualizacaoStatus,@usuario,@cod_fila)";
                        cmm5.Parameters.Add("@statusFila", SqlDbType.VarChar).Value = 2;
                        cmm5.Parameters.Add("@horaAtualizacaoStatus", SqlDbType.DateTime).Value = data;
                        cmm5.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                        cmm5.Parameters.Add("@cod_fila", SqlDbType.Int).Value = Int32.Parse(codigo);

                        try
                        {
                            cnn5.Open();
                            cmm5.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string err = ex.Message;

                        }

                    }


                }



            }

        }


    }







public void LimpaCampos()
{
    txbSus.Text = "";
    lbRF.Text = "";
    lbNome.Text = "";
    lbIdade.Text = "";
    lblograd.Text = "";
    lbbairro.Text = "";
    lbcomplem.Text = "";
    lbnumero.Text = "";
    lbProcedencia.Text = "";
    lbTel.Text = "";
    ddlMarcada.SelectedIndex = 0;
    ddlEspec.SelectedIndex = 0;

    ddlSol.SelectedIndex = 0;
    txbDtCon.Text = "";
    txbHoraCon.Text = "";
    lbcod.Text = "";
    txbdtSol.Text = "";

    txbConsulta.Text = "";
    txbObs.Text = "";
    txbTel1.Text = "";
    txbTel2.Text = "";
    txbTel3.Text = "";
    txbTel4.Text = "";
    chbRetorno.Checked = true;
    chbRegul.Checked = false;
}

protected void Button_click_event(Object sender, EventArgs e)
{
    Button btn = (Button)sender;
    GridViewRow row = (GridViewRow)btn.NamingContainer;
    // assuming you store the ID in a Hiddenield:

    string strID = row.Cells[1].Text;
    string usuario = lbUser.Text;


    string rh = txbRh.Text;



    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(400/2);window.open( '../Exames/cadExames.aspx?ID=" + strID + "&user=" + usuario + "&rh=" + rh + "' , null, 'height=400,width=780,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);


}
protected void btnAtuSUS_Click(object sender, EventArgs e)
{
    string _sus = txbSus.Text;
    int _rh = Convert.ToInt32(txbRh.Text);

    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
    {

        SqlCommand cmm1 = cnn.CreateCommand();
        cmm1.CommandText = "SELECT rh FROM cartao_sus WHERE rh = " + _rh;
        cnn.Open();
        SqlDataReader dr = cmm1.ExecuteReader();
        if (dr.Read())
        {
            dr.Close();
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "UPDATE cartao_sus SET sus = @sus WHERE rh = " + _rh + ";";
            cmm.Parameters.Add("@sus", SqlDbType.VarChar).Value = _sus;
            try
            {
                //cnn.Open();
                cmm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Atualizado com sucesso!');", true); //mensagem de alerta de exames realizados do paciente antes de um ano
        }

        else
        {
            dr.Close();
            SqlCommand cmm2 = cnn.CreateCommand();
            cmm2.CommandText = "INSERT INTO cartao_sus (rh, sus) VALUES (@rh,@sus)";

            cmm2.Parameters.Add("@rh", SqlDbType.Int).Value = _rh;
            cmm2.Parameters.Add("@sus", SqlDbType.VarChar).Value = _sus;
            try
            {
                cmm2.ExecuteNonQuery();
                Response.Write("<script language=javascript>alert('Cadastrado com sucesso!');</script>");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ERRO:  " + ex + "');", true); //mensagem de alerta
            }

        }
    }
}

protected void Button3_Click(object sender, EventArgs e)
{
    string confirmValue = Request.Form["confirm_value"];
    if (confirmValue == "Yes")
    {
        string _rh = txbRh.Text.TrimStart('0');
        string _pac = lbNome.Text.Replace('\'', ' ');
        string _solicitante = ddlSol.SelectedItem.ToString().Replace('\'', ' ');
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(400/2);window.open( '../Exames/cadExames.aspx?rh=" + _rh + "&pac=" + _pac + "&sol=" + _solicitante + "', null, 'height=400,width=780,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
    }

}
public override void VerifyRenderingInServerForm(Control control)
{

}
private string dataFormatada(string data)
{
    return data.Substring(6, 2).PadLeft(2, '0') + "/" + data.Substring(4, 2).PadLeft(2, '0') + "/" + data.Substring(0, 4);

}
private string horaFormatada(string hora)
{

    int horaformatada = int.Parse(hora) / 60;
    int minuto = int.Parse(hora) % 60;
    return horaformatada.ToString().PadLeft(2, '0') + ':' + minuto.ToString().PadLeft(2, '0');

}





public void Consulta(object sender)
{

    if (txbRh.Text.Trim() == "")
        txbRh.Text = "0";
    string registro = txbRh.Text.TrimStart('0');

    string especialidade = "";
    string solicitante = "";
    string codConsulta = txbConsulta.Text;
    string codConPrinc = codConsulta.Substring(0, 4);
    string codConVer = codConsulta.Substring(5, 1);
    string dataConsulta = "";
    string horaConsulta = "";
    string codEspecialidade = "";
    txbConsulta.Text = "";
    txbDtCon.Text = "";
    txbHoraCon.Text = "";

    DateTime dateTime = DateTime.Now;
    string dia = Convert.ToString(Convert.ToInt32(dateTime.Day));//dia atual + 1 = dia seguinte
    if (dia.Length == 1)
        dia = dia.PadLeft(2, '0');
    string mes = Convert.ToString(dateTime.Month);
    if (mes.Length == 1)
        mes = mes.PadLeft(2, '0');

    string data = Convert.ToString(dateTime.Year) + Convert.ToString(mes) + Convert.ToString(dia);

    try
    {





        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select am1101b,am1101c,am1106, am1113,ic0nome,am1104  from am11, intc0 where  am1103 = ic0cpf and am1108 = " + registro + " and am1101b= " + codConPrinc + " and am1101c= " + codConVer + " and am1106 >= " + data + " order by am1106";
            cnn.Open();
            OdbcDataReader dr = cmm.ExecuteReader();

            if (dr.Read())
            {

                codConsulta = dr.GetDecimal(0).ToString().PadLeft(4, '0') + "-" + dr.GetDecimal(1).ToString();
                dataConsulta = dr.GetDecimal(2).ToString();
                horaConsulta = dr.GetDecimal(3).ToString();
                solicitante = dr.GetString(4).ToString();
                codEspecialidade = dr.GetString(5).ToString().PadLeft(7, '0');
                string codEspPrinc = codEspecialidade.Substring(0, 4);
                string codEspSec = codEspecialidade.Substring(4, 2);



                using (OdbcConnection cnn4 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
                {
                    OdbcCommand cmm4 = cnn4.CreateCommand();
                    cmm4.CommandText = "select ia9descr from inta9 where ia9codprin = '" + codEspPrinc + "' and  ia9codsub = '" + codEspSec + "'";
                    cnn4.Open();
                    OdbcDataReader dr4 = cmm4.ExecuteReader();
                    if (dr4.Read())
                    {
                        especialidade = dr4.GetString(0).ToString();
                    }
                }
                using (SqlConnection cnn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {

                    SqlCommand cmm1 = cnn1.CreateCommand();
                    cmm1.CommandText = "SELECT  * FROM [Geral_Treina].[dbo].[Ret_marcadas_impr] where consulta = '" + codConsulta + "' and rh = " + registro + " and dtCon = '" + dataFormatada(dataConsulta) + "'";
                    cnn1.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmm1);

                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    using (SqlConnection cnn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                    {
                        SqlCommand cmm2 = cnn1.CreateCommand();
                        cmm2.CommandText = "SELECT  * FROM [Geral_Treina].[dbo].[Fila] where consulta = '" + codConsulta + "' and rh = " + registro + " and dtCon = '" + dataFormatada(dataConsulta) + "'";
                        cnn2.Open();
                        SqlDataAdapter da2 = new SqlDataAdapter(cmm2);

                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);

                        if (dt.Rows.Count == 0 && dt2.Rows.Count == 0)
                        {
                            txbConsulta.Text = codConsulta;
                            txbDtCon.Text = dataFormatada(dataConsulta);
                            txbHoraCon.Text = horaFormatada(horaConsulta);
                            ddlSol.SelectedValue = solicitante;
                            ddlEspec.SelectedValue = especialidade;
                            ddlMarcada.SelectedValue = "Sim";





                            return;
                        }
                        else
                        {
                            string message = "alert('Consulta já cadastrada no Sistema Painel de Bordo')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                    }
                }
            }
            else
            {
                string message = "alert('Código de consulta: " + codConsulta + " não pertence a este RH:" + registro + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }

        }
    }



    catch (Exception ex1)
    {
        Console.WriteLine("{0} Exception caught.", ex1);
    }
}

protected void txbConsulta_TextChanged(object sender, EventArgs e)
{
    if (txbConsulta.Text.Length == 6)
    {
        Consulta(sender);
    }
}
protected void GridView6_RowDataBound(object sender, GridViewRowEventArgs e)
{

    e.Row.Cells[1].Visible = false;

}
}
