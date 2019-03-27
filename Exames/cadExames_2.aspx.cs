using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Exames_cadExames_2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cod_tabela_exame = Request.QueryString["ID"];
        string usuario = Request.QueryString["user"];
        string exame = Request.QueryString["exame"];
        string grupo = Request.QueryString["grupo"];
        







        if (!IsPostBack)
        {
            txbDtSolicitacao.Text = FormatarData3(DateTime.Now.ToString());
           

            string rh = BuscaRh(cod_tabela_exame);
           
            CarregaNome(rh);
            CarregaGrupo();
            CarregaExames(grupo);
            CarregaPagina(cod_tabela_exame);

            ddlGrupo.Items.FindByValue(grupo).Selected=true;
            ddlExame.Items.FindByValue(exame).Selected = true;



        }

    }


    public void CarregaExames(string grupo)
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = cnn.CreateCommand();
                cmm.Connection = cnn;
                cnn.Open();


                /********* Carrega exames conforme Grupo selecionado ***************/
                cmm.CommandText = "Select * from Exames where Cod_Grupo_Exame = @cod_grupo order by Descricao";
                cmm.Parameters.Add("@cod_grupo", SqlDbType.Int).Value = Convert.ToInt32(grupo);
                SqlDataReader dr1 = cmm.ExecuteReader();
                ddlExame.DataSource = dr1;
                ddlExame.DataValueField = "Cod_Exame";
                ddlExame.DataTextField = "Descricao";
                ddlExame.DataBind();
                dr1.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }
    }

    public void CarregaPagina(string strID)
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                /*************** Carrega os Dados RH, Especialidade, Solicitante ************************/
                SqlCommand cmm = cnn.CreateCommand();
                cmm.Connection = cnn;
                cmm.CommandText = "SELECT [rh],[solicitante],[especialidade],[obs],[dataSolicitacao],[dataAgendamento] FROM [Geral_Treina].[dbo].[Exames_Paciente] where cod =" + strID;
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    lbRh.Text = dr.GetInt32(0).ToString();
                    lbSolicitante.Text = dr.GetString(1);
                    lbEspecialidade.Text = dr.GetString(2);
                    txbDtSolicitacao.Text = FormatarData4(dr.GetDateTime(4).ToString());
                    txbDtAgendamento.Text = dr.GetDateTime(5).ToString().Equals("01/01/1900 00:00:00") ? "" : FormatarData5(dr.GetDateTime(5).ToString());

                    string observacao = dr.GetString(3);
                    txbObs.Text = HttpUtility.HtmlDecode(observacao);




                }
                dr.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }
    }

    public void CarregaNome(string rh)
    {
        
        /************* Carrega o Nome do Paciente *************************/
        try
        {
            using (OdbcConnection cnn3 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm3 = cnn3.CreateCommand();
                cmm3.CommandText = "Select ib6pnome, ib6compos from intb6 where ib6regist = " + rh;
                cnn3.Open();
                OdbcDataReader dr3 = cmm3.ExecuteReader();
                if (dr3.Read())
                {

                    lbPaciente.Text = dr3.GetString(0) + dr3.GetString(1);
                }
                dr3.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }


    }
    public void CarregaGrupo()
    {
        /************* Carrega o Grupo de Exames *************************/
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {

                SqlCommand cmm = cnn.CreateCommand();
                cmm.Connection = cnn;
                cnn.Open();
                cmm.CommandText = "Select * from Grupo_Exame order by Descricao";

                SqlDataReader dr2 = cmm.ExecuteReader();
                ddlGrupo.DataSource = dr2;
                ddlGrupo.DataValueField = "Cod_Grupo_Exame";
                ddlGrupo.DataTextField = "Descricao";
                ddlGrupo.DataBind();
                dr2.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }


    }


    protected string BuscaRh(string cod_exame)
    {
        string rh = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                /********* Carrega exames conforme Grupo selecionado ***************/
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "SELECT [rh] FROM [Geral_Treina].[dbo].[Exames_Paciente] where cod = @cod_exame";
                cmm.Parameters.Add("@cod_exame", SqlDbType.Int).Value = Convert.ToInt32(cod_exame);
                cmm.Connection = cnn;
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();
                if (dr1.Read())
                {
                    rh = dr1.GetInt32(0).ToString();

                }
            } 
        }
        catch (Exception ex)
        {
            string erro = ex.Message;
      

        }
        return rh;
    }

    protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                /********* Carrega exames conforme Grupo selecionado ***************/
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "Select * from Exames where Cod_Grupo_Exame = @cod_grupo order by Descricao";
                cmm.Parameters.Add("@cod_grupo", SqlDbType.Int).Value = Convert.ToInt32(ddlGrupo.SelectedValue);
                cmm.Connection = cnn;
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();
                ddlExame.DataSource = dr1;
                ddlExame.DataValueField = "Cod_Exame";
                ddlExame.DataTextField = "Descricao";
                ddlExame.DataBind();
                dr1.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }


    }
    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>alert('Atualização não realizada!');</script>");
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);



    }

    private void InserirExames()
    {
        int rh = Convert.ToInt32(lbRh.Text);
        string solicitante = lbSolicitante.Text;
        string especialidade = lbEspecialidade.Text;
        string cod_exame = ddlExame.SelectedItem.Value;

        int cod_fila = Convert.ToInt32(Request.QueryString["ID"]);
        string obs = txbObs.Text;
        bool impr = false; // mandado para o mailling ou não.
        string usuarioCadastro = Request.QueryString["user"];
        DateTime dataCadastro = DateTime.Now;
        string statusSituacao = ddlSituacao.SelectedItem.Value;
        string dataAgendamento = txbDtAgendamento.Text;
        if (!dataAgendamento.Equals(""))
            dataAgendamento = FormatarData(txbDtAgendamento.Text);
        else
            dataAgendamento = "1900-01-01 00:00:00.000";
        string dataSolicitacao = txbDtSolicitacao.Text;
        if (!dataSolicitacao.Equals(""))
            dataSolicitacao = FormatarData2(txbDtSolicitacao.Text);
        else
        {
            Response.Write("<script language='javascript'>alert('Atenção! Data de Solicitação não preenchida');</script>");
            return;
        }


        bool falta = chbFaltou.Checked.Equals(true) ? true : false;

        string sSql = "Insert Into Exames_Paciente (rh, solicitante ,cod_exame,impr, exameStatus, cod_fila, usuario, obs, especialidade, dataCadastro,dataUltimaAtualizacao,dataAgendamento,dataSolicitacao,falta) Values (@rh,@sol,@cod_exame,@impr,@status, @cod_fila,@usuarioCadastro,@obs, @especialidade,@dataCadastro,@dataUltimaAtualizacao,@dataAgendamento, @dataSolicitacao, @falta);";
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = new SqlCommand();
            try
            {
                cmm.Connection = cnn;
                cnn.Open();

                cmm.CommandText = sSql;
                cmm.Parameters.Add("@rh", SqlDbType.Int).Value = rh;
                cmm.Parameters.Add("@sol", SqlDbType.VarChar).Value = solicitante;
                cmm.Parameters.Add("@cod_exame", SqlDbType.Int).Value = cod_exame;
                cmm.Parameters.Add("@impr", SqlDbType.Bit).Value = impr;
                cmm.Parameters.Add("@status", SqlDbType.Int).Value = statusSituacao;
                cmm.Parameters.Add("@cod_fila", SqlDbType.Int).Value = cod_fila;
                cmm.Parameters.Add("@usuarioCadastro", SqlDbType.VarChar).Value = usuarioCadastro;
                cmm.Parameters.Add("@obs", SqlDbType.VarChar).Value = obs;
                cmm.Parameters.Add("@especialidade", SqlDbType.VarChar).Value = especialidade;
                cmm.Parameters.Add("@dataCadastro", SqlDbType.DateTime).Value = dataCadastro;
                cmm.Parameters.Add("@dataUltimaAtualizacao", SqlDbType.DateTime).Value = dataCadastro;
                cmm.Parameters.Add("@dataAgendamento", SqlDbType.DateTime).Value = DateTime.Parse(dataAgendamento, CultureInfo.GetCultureInfo("en-US"));
                cmm.Parameters.Add("@dataSolicitacao", SqlDbType.Date).Value = DateTime.Parse(dataSolicitacao, CultureInfo.GetCultureInfo("en-US")).Date;
                cmm.Parameters.Add("@falta", SqlDbType.Bit).Value = falta;



                cmm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {

                Response.Write("<script language='javascript'>alert('Erro na operação " + e.Message + "');</script>");
            }
        }
    }


    private void AdicionarExame()
    {
        int cod_fila = Convert.ToInt32(Request.QueryString["ID"]);
        int qtdExames = 0;

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            try
            {
                /********* Carrega exames conforme Grupo selecionado ***************/
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "SELECT [qtdExames] FROM [Geral_Treina].[dbo].[Fila] where cod =" + cod_fila;
                cmm.Parameters.Add("@cod_grupo", SqlDbType.Int).Value = Convert.ToInt32(ddlGrupo.SelectedValue);
                cmm.Connection = cnn;
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();
                if (dr1.Read())
                {
                    qtdExames = dr1.GetInt32(0);
                    qtdExames = qtdExames + 1;


                }


                dr1.Close();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                Response.Write("<script language='javascript'>alert('Erro na operação " + erro + "');</script>");
            }
        }


        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();

            //atualizar
            cmm.CommandText = " update [Geral_Treina].[dbo].[Fila] set qtdExames= @qtdExames  where cod =" + cod_fila + ";";

            //atualizar
            cmm.Parameters.Add("@qtdExames", SqlDbType.Int).Value = qtdExames;


            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string err = ex.Message;

            }
        }
    }

    protected void btnAtualizar_Click(object sender, EventArgs e)
    {
        if (txbDtAgendamento.Text != "" && ddlSituacao.SelectedValue == "1")
        {
            Response.Write("<script language='javascript'>alert('Atenção: Data de Agendamento marcado, portanto o status do exame não pode ser como encaminhado');</script>");
            LimparPágina();
        }
        else
        {
            try
            {

                LogExamesPacientes();
                UpdateExames();


            }
            catch (SqlException e1)
            {
                Response.Write("<script language='javascript'>alert('Erro ao atualizar registro " + e1.Message + "');</script>");
            }

            Response.Write("<script language=javascript>alert('Atualizado com sucesso!');</script>");
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);

        }

    }


   
    protected string FormatarData(string data)
    {

        data = data.Substring(6, 4) + "-" + data.Substring(3, 2) + "-" + data.Substring(0, 2) + data.Substring(10, 6);


        return data;

    }
    protected string FormatarData2(string data)
    {

        data = data.Substring(6, 4) + "-" + data.Substring(3, 2) + "-" + data.Substring(0, 2);


        return data;

    }
    protected string FormatarData3(string data)
    {

        data = data.Substring(0, 10);


        return data;

    }


    protected void LimparPágina()
    {
        txbDtAgendamento.Text = "";
        txbDtSolicitacao.Text = "";
        txbObs.Text = "";
        ddlSituacao.SelectedIndex = 0;
        ddlExame.SelectedIndex = 0;
        ddlGrupo.SelectedIndex = 0;
        chbFaltou.Checked = false;

    }

    protected void LogExamesPacientes()
    {
        int rh = Convert.ToInt32(lbRh.Text);

        string usuario = Request.QueryString["user"];
        string dataAtualizacao = DateTime.Now.ToString();
        dataAtualizacao = FormatarData(dataAtualizacao);

        string statusSituacao = ddlSituacao.SelectedValue;
        string cod = Request.QueryString["ID"];


        string exameStatus = "";

        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {

                SqlCommand cmm = cnn.CreateCommand();
                cmm.Connection = cnn;
                cnn.Open();
                cmm.CommandText = "Select exameStatus from Exames_Paciente WHERE cod =" + cod;

                SqlDataReader dr2 = cmm.ExecuteReader();
                if (dr2.Read())
                {
                    exameStatus = dr2.GetInt32(0).ToString();

                }
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }
        if (exameStatus != ddlSituacao.SelectedValue)
        {

            string sSql = "Insert Into Log_Exames_Pacientes (statusExame, horaAtualizacaoStatus ,usuario,cod_exames) Values (@statusExames,@horaAtualizacaoStatus,@usuario, @cod_exames);";
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = new SqlCommand();
                try
                {
                    cmm.Connection = cnn;
                    cnn.Open();

                    cmm.CommandText = sSql;
                    cmm.Parameters.Add("@statusExames", SqlDbType.VarChar).Value = statusSituacao;
                    cmm.Parameters.Add("@horaAtualizacaoStatus", SqlDbType.VarChar).Value = dataAtualizacao;
                    cmm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                    cmm.Parameters.Add("@cod_exames", SqlDbType.VarChar).Value = cod;

                    cmm.ExecuteNonQuery();
                }
                catch (SqlException e)
                {

                    Response.Write("<script language='javascript'>alert('Erro na operação " + e.Message + "');</script>");
                }
            }
        }
    }

    protected void UpdateExames()
    {
        int rh = Convert.ToInt32(lbRh.Text);
        string solicitante = lbSolicitante.Text;
        string especialidade = lbEspecialidade.Text;
        string cod_exame = ddlExame.SelectedItem.Value;
        string cod = Request.QueryString["ID"];
      
        int cod_fila = Convert.ToInt32(Request.QueryString["ID"]);
        string obs = txbObs.Text;
        string usuario = Request.QueryString["user"];
        string strDataAtualizacao = DateTime.Now.ToString();

        string dataAtualizacao = FormatarData(strDataAtualizacao);
        string dataSolicitacao = FormatarData2(txbDtSolicitacao.Text);
        string dataAgendamento = txbDtAgendamento.Text;
        if (!dataAgendamento.Equals(""))
            dataAgendamento = FormatarData(txbDtAgendamento.Text);
        else
            dataAgendamento = "1900-01-01 00:00:00.000";
        string falta = "0";
        string statusSituacao = ddlSituacao.SelectedItem.Value;
        if (chbFaltou.Checked)
            falta = "1";
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();


            cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Exames_Paciente] SET cod_exame =" + cod_exame + ",exameStatus =" + statusSituacao + ",usuario ='" + usuario + "',obs='" + obs + "', dataUltimaAtualizacao ='" + dataAtualizacao + "' ,dataSolicitacao='" + dataSolicitacao + "', dataAgendamento ='" + dataAgendamento + "' ,falta = " + falta + "  where cod = " + cod;





            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;

            }
        }
    }




  

 
    protected string FormatarData5(string data)
    {

        data = data.Substring(0,16);


        return data;

    }


    protected string FormatarData4(string data)
    {

        data = data.Substring(0, 10);


        return data;

    }
}