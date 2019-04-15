using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RelatorioExames_ConsultaExames : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
      

            CarregaGrupo();
            CarregaExames();
            ddlEspecialidades();
            CarregaSituacao();

            string sSqlm = "Select ic0nome from intc0 Where ic0nome not like 'Z %' AND ic0nome not like '%RESIDENTE%' ORDER BY ic0nome";
            OdbcConnection con1 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString());
            OdbcCommand cmd1 = new OdbcCommand(sSqlm, con1);
            OdbcDataAdapter da1 = new OdbcDataAdapter(cmd1);

            DataSet ds1 = new DataSet();


            da1.Fill(ds1);
            ddlSolicitante.DataTextField = ds1.Tables[0].Columns["ic0nome"].ToString();
            ddlSolicitante.DataSource = ds1.Tables[0];
            ddlSolicitante.DataBind();
            ddlSolicitante.Items.Insert(0, "Todos");
            ddlSolicitante.SelectedIndex = 0;
            con1.Close();
            ddlExame.Items.Insert(0, "Todos");
            ddlExame.SelectedIndex = 0;
            ddlSituacao.Items.Insert(0, "Todos");
        }
    }
    private void ddlEspecialidades()
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {

                string sql = "Select ia9codprin, ia9codsub, ia9descr From inta9 Where ia9descr not like 'X%' order by ia9descr";

                DataTable dt = new DataTable();
                dt = Especialidades(sql);
                ddlEspecialidade.DataSource = dt;
                ddlEspecialidade.DataTextField = "ia9descr";
                ddlEspecialidade.DataValueField = "ia9descr";
                ddlEspecialidade.DataBind();
                ddlEspecialidade.Items.Insert(0, "Todos");
                ddlEspecialidade.SelectedIndex = 0;

              
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

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
                ddlGrupo.Items.Insert(0, "Todos");
                ddlGrupo.SelectedIndex = 0;
                dr2.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }


    }
    public void CarregaSituacao()
    {
        /************* Carrega o Grupo de Exames *************************/
        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {

                SqlCommand cmm = cnn.CreateCommand();
                cmm.Connection = cnn;
                cnn.Open();
                cmm.CommandText = "SELECT [Cod_Situacao], [Descricao] FROM [Situacao_Exame]";

                SqlDataReader dr2 = cmm.ExecuteReader();
                ddlSituacao.DataSource = dr2;
                ddlSituacao.DataValueField = "Cod_Situacao";
                ddlSituacao.DataTextField = "Descricao";
                ddlSituacao.DataBind();
         
                dr2.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }


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
                ddlExame.Items.Insert(0, "Todos");
                ddlExame.SelectedIndex = 0;
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
        string strRh = txtRh.Text;
        string strDtSolicInicio = txbDtSolicitacaoInicio.Text.Equals("") ? "" : FormatarData2(txbDtSolicitacaoInicio.Text);
        string strDtSolicFim = txbDtSolicitacaoFim.Text.Equals("") ? "" : FormatarData2(txbDtSolicitacaoFim.Text);
        string strDtAgendInicio = txbDtAgendamentoInicio.Text.Equals("") ? "" : FormatarData(txbDtAgendamentoInicio.Text);
        string strDtAgendFim = txbDtAgendamentoFim.Text.Equals("") ? "" : FormatarData(txbDtAgendamentoFim.Text);
        string strSolici = ddlSolicitante.SelectedItem.Text;
        string strEspecial = ddlEspecialidade.SelectedItem.Text;
        string strFalta =chbFaltou.Checked.ToString();
        string strGrupo= ddlGrupo.SelectedItem.Value;
        string strSituacao = ddlSituacao.SelectedItem.Value;


        string strExames = ddlExame.SelectedItem.Value.ToString();
        System.Text.StringBuilder builder = new System.Text.StringBuilder("SELECT cod_fila, p.cod_exame as cod_exame, solicitante, especialidade, obs, cod, dataSolicitacao, dataAgendamento,falta,rh,s.Descricao as descricao  FROM [Geral_Treina].[dbo].[Exames_Paciente] as p join[Geral_Treina].[dbo].[Exames] as e on p.cod_exame = e.Cod_Exame join [Geral_Treina].[dbo].[Situacao_Exame]as s on s.Cod_Situacao= p.exameStatus where  impr = 0  ");
        if (!strRh.Equals(""))
            builder.Append(" and rh = " + strRh);
        if (!strDtSolicInicio.Equals(""))
        { 
            if (!strDtSolicFim.Equals(""))
                builder.Append("and dataSolicitacao between '" + strDtSolicInicio + "' and '" + strDtSolicFim +"'");
            else
                    builder.Append("and dataSolicitacao between '" + strDtSolicInicio + "' and '" + strDtSolicInicio +"'");
        }
        if (!strDtAgendInicio.Equals(""))
        {
            if (!strDtAgendFim.Equals(""))
                builder.Append("and dataAgendamento between '" + strDtAgendInicio + "' and '" + strDtAgendFim +"'");
            else
                builder.Append("and dataAgendamento between '" + strDtAgendInicio + "' and '" + strDtAgendInicio + "'");
        }
        if (!strSolici.Equals("Todos"))
            builder.Append(" and solicitante = '" + strSolici +"'");
        
        if (!strEspecial.Equals("Todos"))
            builder.Append(" and especialidade = '" + strEspecial + "'");

        if (!strExames.Equals("Todos"))
            builder.Append(" and p.cod_exame = " + strExames);
        builder.Append(" and falta =  '" + strFalta +"'");
        if (!strGrupo.Equals("Todos"))
            builder.Append("  and Cod_Grupo_Exame = " + strGrupo);
        builder.Append(" and falta =  '" + strFalta + "'");
        if(!strSituacao.Equals("Todos"))
            builder.Append(" and exameStatus = " + strSituacao);






        try
        {
           
            
                grvExamesPesquisados.DataSource = CarregarGrvExamesPesquisados(builder.ToString());
                grvExamesPesquisados.DataBind();
                
            
        }
        catch(Exception ex)
        {
            string erro = ex.Message;

        }

    }
public static DataTable CarregarGrvExamesPesquisados(string str)
{
    string strConexao = @"Data Source=10.48.16.14;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
    string strQuery = str;

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
            dt.Columns.Add("RH", System.Type.GetType("System.String"));
            dt.Columns.Add("Nome", System.Type.GetType("System.String"));
            dt.Columns.Add("Solicitante", System.Type.GetType("System.String"));
            dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
            dt.Columns.Add("Grupo de Exames", System.Type.GetType("System.String"));
            dt.Columns.Add("Exames", System.Type.GetType("System.String"));
            dt.Columns.Add("Observacao", System.Type.GetType("System.String"));
            dt.Columns.Add("Data Solicitacao", System.Type.GetType("System.String"));
            dt.Columns.Add("Data Agendamento", System.Type.GetType("System.String"));
            dt.Columns.Add("Situacao", System.Type.GetType("System.String"));
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
                string rh = dr1.GetInt32(9).ToString();
                string nome = CarregaNome(rh);
                string situacao = dr1.GetString(10);

               dt.Rows.Add(new String[] { codigo_fila, codigo, rh,nome, solicitante, especialidade, grupo_exames, exames, obs, dataSolicitacao, dataAgendamento, situacao, falta });
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
protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGrupo.SelectedItem.Text.Equals("Todos"))
        {
            ddlExame.Items.Clear();
            ddlExame.Items.Insert(0, "Todos");
            ddlExame.SelectedIndex = 0;
        }
        else
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
                
                    ddlExame.Items.Insert(0, "Todos");
                    dr1.Close();
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message;

            }
        }
    }
    protected void grvExamesPesquisados_SelectedIndexChanged(object sender, EventArgs e)
    {
        string usuario = User.Identity.Name;


        string cod_tabela_exame = grvExamesPesquisados.SelectedRow.Cells[2].Text;
        string cod_exame = "";
        string cod_grupo = "";

        try
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                /********* Carrega exames conforme Grupo selecionado ***************/
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "SELECT e.[cod_exame],[Cod_Grupo_Exame] FROM [Geral_Treina].[dbo].[Exames_Paciente] as p join [Geral_Treina].[dbo].[Exames] as e on p.cod_exame =e.Cod_Exame  where cod =@cod_exame";
                cmm.Parameters.Add("@cod_exame", SqlDbType.Int).Value = Convert.ToInt32(cod_tabela_exame);
                cmm.Connection = cnn;
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();
                if (dr1.Read())
                {
                    cod_grupo = dr1.GetInt32(1).ToString();
                    cod_exame = dr1.GetInt32(0).ToString();
                }

                ddlExame.Items.Insert(0, "Todos");
                dr1.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }


        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(400/2);window.open( '../Exames/cadExames_2.aspx?ID=" + cod_tabela_exame + "&user=" + usuario  + "&exame=" + cod_exame + "&grupo=" + cod_grupo + "', null, 'height =400,width=780,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);


    }
    protected void grvExamesPesquisados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
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
    public static string CarregaNome(string rh)
    {
        string nome = "";
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

                    nome = dr3.GetString(0) + dr3.GetString(1);
                }
                dr3.Close();
            }
        }
        catch (Exception ex)
        {
            string erro = ex.Message;

        }

        return nome;
    }
}