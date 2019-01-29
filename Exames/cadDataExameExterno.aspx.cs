using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Exames_cadDataExameExterno : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            lbPaciente.Text = Request.QueryString["pac1"];   // Nome do paciente
            lbRh.Text = Request.QueryString["rh1"];          // Registro Hospitalar
            lbExame.Text = Request.QueryString["exm1"];      // Código do Exame 
            lbfilaExm.Text = Request.QueryString["fila1"];   // Chave primária gerado automaticamente da tabela Exame_Interno_Paciente
            lbSolicitante.Text = Request.QueryString["solic1"];
            // Grupo de exames
            // Solicitante se encontra na tabela Exame_Interno_Paciente
            string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
            string strQuery = "";
            string cod_tabela_exame = lbfilaExm.Text;
            strQuery = "SELECT g.Descricao" +
                " FROM [Geral_Treina].[dbo].[Exames_Paciente] p " +
                " INNER JOIN [Geral_Treina].[dbo].[Exames] e" +
                " ON p.cod_exame = e.Cod_Exame" +
                " INNER JOIN [Geral_Treina].[dbo].[Grupo_Exame] g" +
                " ON e.Cod_Grupo_Exame = g.Cod_Grupo_Exame " +
                " WHERE p.cod = " + cod_tabela_exame;

            using (SqlConnection conn = new SqlConnection(strConexao))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(strQuery, conn);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lbGrupoExame.Text = dr.GetString(0);

                    }
                }
                catch (Exception ex)
                {
                    string erro = ex.Message;
                }
            }

        }
    }



    protected void btnGravarExamExterno_Click(object sender, EventArgs e)
    {
        DateTime data = Convert.ToDateTime(txbDtLiberacao.Text);

        string dia = Convert.ToString(data.Day);
        if (dia.Length == 1)
            dia = dia.PadLeft(2, '0');

        string mes = Convert.ToString(data.Month);
        if (mes.Length == 1)
            mes = mes.PadLeft(2, '0');

        string dataParaLiberacao = Convert.ToString(data.Year) + "-" + Convert.ToString(mes) + "-" + Convert.ToString(dia) ;
        string constr = ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString;

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO [Geral_Treina].[dbo].[Exames_Ext_Marcado]([data_liberacao],[observ],[cod_exm_ext_pac])" +
                " VALUES('" + dataParaLiberacao + "','" + txbObs.Text + "'," + lbfilaExm.Text + ")", con))
            {
                try
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    

                }

                catch (Exception ex)
                {
                    string erro = ex.Message;
                }
                Response.Write("<script language=javascript>alert('Cadastrado com sucesso!');</script>");
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
                
            }//1o. using
        }//2o. using
    }
}
