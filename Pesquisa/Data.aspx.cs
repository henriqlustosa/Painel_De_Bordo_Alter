using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.Odbc;
using System.Configuration;

public partial class Pesquisa_Data : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    

    public void ExportarToExel(String saveAsFile)
    {
        // O linite de linhas do Excel é 65536
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + saveAsFile + ".xls");
        // Remover caracteres do header - Content-Type
        HttpContext.Current.Response.Charset = "";
        //HttpContext.Current.Response.WriteFile("style.txt")
        // desabilita o view state.
        GridView2.EnableViewState = false;
        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        GridView2.RenderControl(htw);
        // Escrever o html no navegador
        HttpContext.Current.Response.Write(sw.ToString());
        // termina o response
        HttpContext.Current.Response.End();

    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportarToExel(txbData.Text);
    }

    protected void btnPesq_Click(object sender, EventArgs e)
    {
        string dt = txbData.Text;
        string dia, mes, ano;
        dia = dt.Substring(0,2);
        mes = dt.Substring(3, 2);
        ano = dt.Substring(6,4);
        dt = ano + "-" + mes + "-" + dia;
        
        GridView2.DataSource = gridCarregaFilaPorData(dt);
        GridView2.DataBind();
    }

    public static String getNome(string _rh)
    {
        string nome = "";

        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select ib6pnome,ib6compos from intb6 where ib6regist = " + _rh;
            cnn.Open();
            OdbcDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                nome = dr.GetString(0) + dr.GetString(1);
            }
        }
        return nome;
    }

    public static DataTable gridCarregaFilaPorData(string _data)
    {
        string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
        string strQuery = "SELECT [cod],CONVERT(varchar,[data], 103),[rh],[especialidade],[solicitante]" +
        ",[retorno], [regulacao],[qtdExames],[situacao],[marcada],[consulta],[telefone]" +
        ",[obs],[usuario] FROM [Geral_Treina].[dbo].[Fila] "+ 
        " where data = '"+ _data +"'";
        using (SqlConnection conn = new SqlConnection(strConexao))
        {
            SqlDataReader dr1 = null;
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                dr1 = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                dt.Columns.Add("Cod", System.Type.GetType("System.String"));
                dt.Columns.Add("Data", System.Type.GetType("System.String"));
                dt.Columns.Add("Rh", System.Type.GetType("System.String"));
                dt.Columns.Add("Nome", System.Type.GetType("System.String"));
                dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
                dt.Columns.Add("Solicitante", System.Type.GetType("System.String"));
                dt.Columns.Add("Retorno", System.Type.GetType("System.String"));
                dt.Columns.Add("Regulação", System.Type.GetType("System.String"));
                dt.Columns.Add("Qtd Exames", System.Type.GetType("System.String"));
                dt.Columns.Add("Situação", System.Type.GetType("System.String"));
                dt.Columns.Add("Marcada", System.Type.GetType("System.String"));
                dt.Columns.Add("Consulta", System.Type.GetType("System.String"));
                dt.Columns.Add("Telefone", System.Type.GetType("System.String"));
                dt.Columns.Add("Observação", System.Type.GetType("System.String"));
                dt.Columns.Add("Usuário", System.Type.GetType("System.String"));


                while (dr1.Read())
                {

                    string cod = dr1.GetInt32(0).ToString();
                    string data = dr1.GetString(1);
                    string rh = dr1.GetString(2);
                    string nome = getNome(rh);
                    string espec = dr1.GetString(3);
                    string sol = dr1.GetString(4);
                    string retorno = dr1.GetString(5);
                    string regul = dr1.GetString(6);
                    string qtdExm = dr1.GetInt32(7).ToString();
                    string sit = dr1.GetString(8);
                    string marcada = dr1.GetString(9);
                    string consulta = dr1.GetString(10);
                    string tel = dr1.GetString(11);
                    string obs = dr1.GetString(12);
                    string usuario = dr1.GetString(13);

                    dt.Rows.Add(new String[] { cod, data, rh, nome, espec, sol, retorno, regul, qtdExm, sit, marcada, consulta, tel, obs, usuario });
                }
                //dr1.Close();
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
