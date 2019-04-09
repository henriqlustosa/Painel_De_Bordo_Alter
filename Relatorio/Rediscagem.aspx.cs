using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class Relatorio_Rediscagem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLerExcel_Click(object sender, EventArgs e)
    {
        if (fupArquivo.FileContent != null)
        {

            if (Permitir(fupArquivo.FileName))
            {
                try
                {
                    string Excel = AppDomain.CurrentDomain.BaseDirectory + fupArquivo.FileName;
                    fupArquivo.SaveAs(Excel);
                    DataTable Dados = DadosExcel(Excel);

                    if (Dados != null)
                    {
                        gvExcel.DataSource = Dados;
                        gvExcel.DataBind();
                    }

                    string arquivo = Excel;
                    if (File.Exists(arquivo))
                    {
                        File.Delete(arquivo);
                    }
                }
                catch (HttpException ex)
                {
                    Response.Write("<script language='javascript'>alert('Erro ao carregar o arquivo: "+ ex.Message +  "');</script>");
                }
            }
            else
            {
                Response.Write("<script language='javascript'>alert('Extensão de arquivo não permitida.');</script>");
            }
        }
    }

    private bool Permitir(string arquivo)
    {
        string extensao = System.IO.Path.GetExtension(arquivo).ToLower();
        string[] permitido = { ".xls" };
        for (int i = 0; i < permitido.Length; i++)
        {
            if (string.Compare(extensao, permitido[i]) == 0)
            {
                return true;
            }
        } return false;
    }

    private DataTable DadosExcel(string Arquivo)
    {
        Char aspas = '"';
        string Conexao = "Provider=Microsoft.Jet.OLEDB.4.0;" +
        "Data Source=" + Arquivo + ";" +
        "Extended Properties=" + aspas + "Excel 8.0;HDR=YES" + aspas;
        System.Data.OleDb.OleDbConnection Cn = new System.Data.OleDb.OleDbConnection();
        Cn.ConnectionString = Conexao;
        Cn.Open();
        object[] Restricoes = { null, null, null, "TABLE" };
        DataTable DTSchema = Cn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, Restricoes);
        if (DTSchema.Rows.Count > 0)
        {
            string Sheet = DTSchema.Rows[0]["TABLE_NAME"].ToString();
            System.Data.OleDb.OleDbCommand Comando = new System.Data.OleDb.OleDbCommand("SELECT * FROM [" + Sheet + "]", Cn);
            DataTable Dados = new DataTable();
            System.Data.OleDb.OleDbDataAdapter DA = new System.Data.OleDb.OleDbDataAdapter(Comando);
            DA.Fill(Dados);
            Cn.Close();
            return Dados;
        }
        return null;
    }

    public void exportarToTxt()
    {
        DateTime dt = DateTime.Now;
        string dia = Convert.ToString(Convert.ToInt32(dt.Day) + 1);//dia atual + 1 = dia seguinte
        if (dia.Length == 1)
            dia = dia.PadLeft(2, '0');
        string mes = Convert.ToString(dt.Month);
        if (mes.Length == 1)
            mes = mes.PadLeft(2, '0');

        string data = Convert.ToString(dia) + Convert.ToString(mes) + Convert.ToString(dt.Year);

        string nomeArquivo = "mailing_HSPM_Retorno_" + data + ".txt";

        Response.ClearContent();
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", nomeArquivo));
        Response.ContentType = "application/text";

        StringBuilder str = new StringBuilder();
        str.Append("PACIENTE;RH;ESPECIALIDADE;DT_CONSULTA;PROFISSIONAL;COD_CONSULTA;OBSERVACAO_HOSPUB;TELEFONE;");
        str.AppendLine();

        for (int j = 0; j < gvExcel.Rows.Count; j++)
        {
            for (int k = 0; k < gvExcel.Rows[j].Cells.Count; k++)
            {
                str.Append(gvExcel.Rows[j].Cells[k].Text + ';');
            }
            str.AppendLine();
        }
        Response.Write(str.ToString());
        Response.End();
    }
    protected void btnExportar_Click(object sender, EventArgs e)
    {
        exportarToTxt();
    }
}
