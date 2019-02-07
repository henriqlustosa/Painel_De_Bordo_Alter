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

public partial class Exames_cadExamest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strID = Request.QueryString["ID"];




        if (!IsPostBack)
        {
         
                CarregaPagina(strID);
                CarregaNome();
                CarregaExames();
                CarregaGridViewExamesMarcados(strID );
                CarregaGridViewExamesSolicitados(strID);

                

        }

    }

    public void CarregaGridViewExamesSolicitados(string strID)
    {
        GridView3.DataSource = FilaExames.gridCarregaExames(lbRh.Text, strID);
        GridView3.DataBind();
    }
    public void CarregaGridViewExamesMarcados(string strID)
    {
        GridView1.DataSource = FilaExames.gridCarregaExamesMarcados(lbRh.Text, strID);
        GridView1.DataBind();
    }
    public void CarregaExames()
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = cnn.CreateCommand();
                cmm.Connection = cnn;


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

    public void CarregaPagina( string strID)
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

    protected void btnCad_Click(object sender, EventArgs e)
    {
        string codExame = ddlExame.SelectedValue;
        string exame = ddlExame.SelectedItem.ToString();

      //  listBoxCod.Items.Add(new ListItem(codExame));
       // listBoxExm.Items.Add(new ListItem(exame));
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
        int rh = Convert.ToInt32(lbRh.Text);
        string solicitante = lbSolicitante.Text;
        string especialidade = lbEspecialidade.Text;
         string cod_exame = ddlExame.SelectedValue;
       
        int cod_fila = Convert.ToInt32(Request.QueryString["ID"]);
        string obs = txbObs.Text;
        bool impr = false; // mandado para o mailling ou não.
        int status = 4;// colocar o status de Enc para CD
        string usuario = Request.QueryString["user"];
        DateTime dataCadastro = DateTime.Now;
        try
        {
           
                    // invoca método para gravar dados
            InserirExames(rh, solicitante, cod_exame, impr, status, cod_fila,usuario,obs,especialidade,dataCadastro );
            AdicionarExame(cod_fila);
            
        }
        catch (SqlException e1)
        {
            Response.Write("<script language='javascript'>alert('Erro ao inserir registro " + e1 + "');</script>");
        }
        
        Response.Write("<script language=javascript>alert('Cadastrado com sucesso!');</script>");
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
    }

    private void InserirExames(int rh,string solic ,string p,bool impr, int status, int cod_fila, string usuario, string obs,string especialidade, DateTime dataCadastro  )
    {
        int _rh = rh;
        int cod_exame = Convert.ToInt32(p);
        string sSql = "Insert Into Exames_Paciente (rh, solicitante ,cod_exame,impr, status, cod_fila, usuario, obs, especialidade, dataCadastro) Values (@rh,@sol,@cod_exame,@impr,@status, @cod_fila,@usuario,@obs, @especialidade,@dataCadastro);";
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = new SqlCommand();
            try
            {
                cmm.Connection = cnn;
                cnn.Open();

                cmm.CommandText = sSql;
                cmm.Parameters.Add("@rh", SqlDbType.Int).Value = rh;
                cmm.Parameters.Add("@sol", SqlDbType.VarChar).Value = solic;
                cmm.Parameters.Add("@cod_exame", SqlDbType.Int).Value = cod_exame;
                cmm.Parameters.Add("@impr", SqlDbType.Bit).Value = impr;
                cmm.Parameters.Add("@status", SqlDbType.Int).Value = 4;//1-marcada;2-realizada;3-cancelada;4-aguardando vaga
                cmm.Parameters.Add("@cod_fila", SqlDbType.Int).Value = cod_fila;
                cmm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                cmm.Parameters.Add("@obs", SqlDbType.VarChar).Value = obs;
                cmm.Parameters.Add("@especialidade", SqlDbType.VarChar).Value = especialidade;
                cmm.Parameters.Add("@dataCadastro", SqlDbType.DateTime).Value = dataCadastro;
          
                cmm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
               
                Response.Write("<script language='javascript'>alert('Erro na operação " + e + "');</script>");
            }
        }
    }

    private void AdicionarExame(int cod_fila)
    {

        string err = "";
        int erro = 0;
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
                string erro2 = ex.Message;
                Response.Write("<script language='javascript'>alert('Erro na operação " + erro2 + "');</script>");
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
                err = ex.Message;
                erro = 1;
            }
        }
    }

    protected void btnAtualizar_Click(object sender, EventArgs e)
    {

    }
    protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}