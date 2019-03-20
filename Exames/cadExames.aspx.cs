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
using System.Globalization;

public partial class Exames_cadExamest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strID = Request.QueryString["ID"];




        if (!IsPostBack)
        {

            btnAtualizar.Enabled = false;
            CarregaPagina(strID);
            CarregaNome();
            CarregaGrupo();
            CarregaExames();
            CarregaGridViewExamesSolicitados(strID);
            CarregaGridViewExamesMarcados(strID);




        }

    }

    public void CarregaGridViewExamesSolicitados(string strID)
    {
        grvExamesSolicitados.DataSource = FilaExames.gridCarregaExamesSolicitados(strID);
        grvExamesSolicitados.DataBind();
    }
    public void CarregaGridViewExamesMarcados(string strID)
    {
        grvExamesMarcados.DataSource = FilaExames.gridCarregaExamesMarcados(strID);
        grvExamesMarcados.DataBind();
    }
    public void CarregaExames()
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
                cmm.Parameters.Add("@cod_grupo", SqlDbType.Int).Value = Convert.ToInt32(ddlGrupo.SelectedValue);
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
                cmm.CommandText = "SELECT [rh],[especialidade],[solicitante] FROM [Geral_Treina].[dbo].[Fila] WHERE cod =" + strID;
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    lbRh.Text = dr.GetString(0);

                    lbEspecialidade.Text = dr.GetString(1);
                    lbSolicitante.Text = dr.GetString(2);



                }
                dr.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }
    }

    public void CarregaNome()
    {
        /************* Carrega o Nome do Paciente *************************/
        try
        {
            using (OdbcConnection cnn3 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm3 = cnn3.CreateCommand();
                cmm3.CommandText = "Select ib6pnome, ib6compos from intb6 where ib6regist = " + lbRh.Text;
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




    protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
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
    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
     
        try
        {

            InserirExames();
            AdicionarExame();
            Response.Write("<script language=javascript>alert('Cadastrado com sucesso!');</script>");
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);

        }
        catch (SqlException e1)
        {
            Response.Write("<script language='javascript'>alert('Erro ao inserir registro " + e1 + "');</script>");
        }

       
    }

    private void InserirExames()
    {
        int rh = Convert.ToInt32(lbRh.Text);
        string solicitante = lbSolicitante.Text;
        string especialidade = lbEspecialidade.Text;
        string cod_exame = ddlExame.SelectedValue;

        int cod_fila = Convert.ToInt32(Request.QueryString["ID"]);
        string obs = txbObs.Text;
        bool impr = false; // mandado para o mailling ou não.
        string usuarioCadastro = Request.QueryString["user"];
        DateTime dataCadastro = DateTime.Now;
        string statusSituacao = ddlSituacao.SelectedValue;
        string dataAgendamento =  FormatarData(txbDtAgendamento.Text);
        string dataSolicitacao = FormatarData2(txbDtSolicitacao.Text);
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


    protected void grvExamesMarcados_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnCadastrar.Enabled = false;
        btnAtualizar.Enabled = true;
        string cod = grvExamesMarcados.SelectedRow.Cells[2].Text;
        lbSolicitante.Text = grvExamesMarcados.SelectedRow.Cells[3].Text;
        lbEspecialidade.Text = grvExamesMarcados.SelectedRow.Cells[4].Text;
        ddlGrupo.SelectedItem.Text = grvExamesMarcados.SelectedRow.Cells[5].Text;
        ddlExame.SelectedItem.Text = grvExamesMarcados.SelectedRow.Cells[6].Text;
        txbObs.Text = Server.HtmlDecode(grvExamesMarcados.SelectedRow.Cells[7].Text);
        txbDtSolicitacao.Text = grvExamesMarcados.SelectedRow.Cells[8].Text;
        txbDtAgendamento.Text = Server.HtmlDecode(grvExamesMarcados.SelectedRow.Cells[9].Text);
        bool falta = grvExamesMarcados.SelectedRow.Cells[10].Text.Equals("Sim") ? true : false;
        chbFaltou.Checked = falta;
        
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
                    ddlSituacao.SelectedValue = dr2.GetInt32(0).ToString();

                }
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

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
        string cod = "";
        if (grvExamesSolicitados.SelectedRow == null)
        {
            cod = grvExamesMarcados.SelectedRow.Cells[2].Text;
        }
        else
        {
            cod = grvExamesSolicitados.SelectedRow.Cells[2].Text;
        }
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
        string cod_exame = ddlExame.SelectedValue;
        string cod = "";
        if (grvExamesSolicitados.SelectedRow == null)
        {
            cod = grvExamesMarcados.SelectedRow.Cells[2].Text;
        }
        else
        {
            cod = grvExamesSolicitados.SelectedRow.Cells[2].Text;
        }
        int cod_fila = Convert.ToInt32(Request.QueryString["ID"]);
        string obs = txbObs.Text;
        string usuario = Request.QueryString["user"];
        string strDataAtualizacao = DateTime.Now.ToString();
                    
        string dataAtualizacao=FormatarData(strDataAtualizacao);
        string dataSolicitacao =FormatarData2( txbDtSolicitacao.Text);
        string dataAgendamento =  FormatarData(txbDtAgendamento.Text);
        string falta = "0";
        string statusSituacao = ddlSituacao.SelectedValue;
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




    protected void grvExamesSolicitados_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnCadastrar.Enabled = false;
        btnAtualizar.Enabled = true;
        string cod = grvExamesSolicitados.SelectedRow.Cells[2].Text;
        lbSolicitante.Text = grvExamesSolicitados.SelectedRow.Cells[3].Text;
        lbEspecialidade.Text = grvExamesSolicitados.SelectedRow.Cells[4].Text;
        ddlGrupo.SelectedItem.Text = grvExamesSolicitados.SelectedRow.Cells[5].Text;
        ddlExame.SelectedItem.Text = grvExamesSolicitados.SelectedRow.Cells[6].Text;
        txbObs.Text = Server.HtmlDecode(grvExamesSolicitados.SelectedRow.Cells[7].Text);
        txbDtSolicitacao.Text = grvExamesSolicitados.SelectedRow.Cells[8].Text;
        txbDtAgendamento.Text = Server.HtmlDecode(grvExamesSolicitados.SelectedRow.Cells[9].Text);
        bool falta = grvExamesSolicitados.SelectedRow.Cells[10].Text.Equals("Sim") ? true : false ;
        chbFaltou.Checked = falta;
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
                    ddlSituacao.SelectedValue = dr2.GetInt32(0).ToString();

                }
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }

    }

    protected void grvExamesSolicitados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
    }
    protected void grvExamesMarcados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
    }
}