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
using System.Text;
using System.IO;
using System.Web.Security;
public partial class Relatorio_impressaoExames : System.Web.UI.Page
{
    //Método que carrega a página
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    // Método que faz o start para a exportação do arquivo, ao clicar
    // no botão "Exportar arquivo" busca no Banco de Dados:- os registros
    // marcados como sim, que foram marcados até a data atual.
    // Entrada:  vazio
    // Saída: Data Set contendo a consulta que retorna todas as consultas cadastradas até a data atual.
    public DataSet exportarArquivo()
    {
        string dtParaCons = txbDtRelatorio.Text;
        DateTime data = Convert.ToDateTime(dtParaCons);

        data = data.AddDays(15);
        string dia = dtParaCons.Substring(0, 2);
        //if (dia.Length == 1)
        //    dia = dia.PadLeft(2, '0');
        string mes = dtParaCons.Substring(3, 2);
        //if (mes.Length == 1)
        //    mes = mes.PadLeft(2, '0');
        string ano = dtParaCons.Substring(6, 4);

        string dataParaConsulta = ano + "-" + mes + "-" + dia;



        string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
        string strQuery = "";
        
        strQuery = "SELECT [cod],[rh],[cod_exame],[Descricao],[dt_consulta],[hr_consulta],[executante],[num_consulta]" +
            "FROM [Geral_Treina].[dbo].[vw_exames_internos_marcados] where impr = 'false' and dt_consulta='" + dataParaConsulta + "'";


        using (SqlConnection conn = new SqlConnection(strConexao))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            SqlCommand cmd = new SqlCommand(strQuery, conn);

            try
            {
                cmd.CommandType = CommandType.Text;
                adapter.TableMappings.Add("Table", "ArquivoTxt");
                adapter.SelectCommand = cmd;
                adapter.Fill(dataSet);
            }

            catch (Exception ex)
            {
                string erro = ex.Message;
            }

            return dataSet;
        }
    }

    // Método para que retorne uma tabela com todos os campos que serão  utilizados para a exportação
    // do arquivo txt.
    // Entrada:
    //     conjunto1 - Data Set contendo a consulta que retorna todas as consultas CADASTRADAS até a data atual
    //     DateTime dt - data base que a partir dela sera verificada as datas de consultas AGENDADAS

    public DataTable consultasAgendadas(DataSet conjunto1)
    {

        DataTable dt2 = new DataTable();

        try
        {
            // Adicionando as colunas que serão visualizadas no arquivo txt
            dt2.Columns.Add("Nome", System.Type.GetType("System.String"));    // Nome do paciente
            dt2.Columns.Add("Rh", System.Type.GetType("System.String"));      // Registro hospitar   
            dt2.Columns.Add("Especialidade", System.Type.GetType("System.String"));  // Especialidade médica
            dt2.Columns.Add("Dt_Consulta", System.Type.GetType("System.String"));     // Data agendada da consulta
            dt2.Columns.Add("Executante", System.Type.GetType("System.String"));    // Nome do médico
            dt2.Columns.Add("Consulta", System.Type.GetType("System.String"));       // Código da consulta
            dt2.Columns.Add("Telefone", System.Type.GetType("System.String"));       // Telefone do paciente

            foreach (DataRow pRow in conjunto1.Tables["ArquivoTxt"].Rows)
            {
                string rh = pRow["rh"].ToString();
                string nome = getNome(rh);
                string espec = pRow["Descricao"].ToString();
                string data3 = pRow["dt_consulta"].ToString();
                data3 = recuperarDataParaString(data3);

                string hora = pRow["hr_consulta"].ToString();
                string sol = pRow["executante"].ToString();
                string datConsult = data3 + " " + hora;

                string consulta = pRow["num_consulta"].ToString(); ;
                // Buscar o telefone na tabela fila do Sistema Painel de Bordo
                string tel = getTelefonePainelBordo(rh);
                // se o campo do telefone não estiver preenchido
                if (tel.Equals("") || tel.Equals("&nbsp;"))
                {
                    using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
                    {
                        OdbcCommand cmm = cnn.CreateCommand();
                        cmm.CommandText = "Select ib6telef from intb6 where ib6regist = " + rh;
                        cnn.Open();
                        OdbcDataReader dr = cmm.ExecuteReader();
                        if (dr.Read())
                        {
                            char[] arr = new char[] { '0', ' ' };
                            tel = dr.GetString(0);
                            tel = tel.TrimStart(arr);
                            if (tel.Equals(""))
                            {
                                tel = "Nao cadastrado";
                            }
                        }
                    }
                }


                dt2.Rows.Add(new String[] { nome, rh, espec, datConsult, sol, consulta, tel });

            }//for each


        } //try
        catch (Exception ex)
        {
            string erro = ex.Message;
        }
        return dt2;



    }
    public string recuperarDataParaString(string dat)
    {
        DateTime data = Convert.ToDateTime(dat);
        string dia = Convert.ToString(data.Day);
        if (dia.Length == 1)
            dia = dia.PadLeft(2, '0');
        string mes = Convert.ToString(data.Month);
        if (mes.Length == 1)
            mes = mes.PadLeft(2, '0');

        string dataSistem = Convert.ToString(data.Year) + "-" + Convert.ToString(mes) + "-" + Convert.ToString(dia);


        return dataSistem;

    }


    public void exportarToTxt()
    {

        DateTime dt = DateTime.Now;
        string dia = Convert.ToString(dt.Day);
        if (dia.Length == 1)
            dia = dia.PadLeft(2, '0');
        string mes = Convert.ToString(dt.Month);
        if (mes.Length == 1)
            mes = mes.PadLeft(2, '0');

        string data = Convert.ToString(dia) + Convert.ToString(mes) + Convert.ToString(dt.Year);

        string nomeArquivo = "mailing_HSPM_Exames_" + data + ".txt";

        Response.ClearContent();
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", nomeArquivo));
        Response.ContentType = "application/text";
        StringBuilder str = new StringBuilder();

        str.Append("PACIENTE;RH;EXAME;DT_CONSULTA;PROFISSIONAL;COD_CONSULTA;TELEFONE");
        str.AppendLine();


        for (int j = 0; j < GridView1.Rows.Count; j++)
        {
            for (int k = 0; k < GridView1.Rows[j].Cells.Count; k++)
            {
                str.Append(GridView1.Rows[j].Cells[k].Text + ';');
            }
            str.AppendLine();
        }
        Response.Write(str.ToString());
        Response.End();
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



    // Método  para preencher o GriedView
    public void gridCarregaGridView1()
    {
        GridView1.DataSource = consultasAgendadas(exportarArquivo());
        GridView1.DataBind();
    }


    // Método para colocar impr como true na tabela Exame_Paciente 

    public void setImprExmPaciente(DataSet conjunto1)
    {

        foreach (DataRow pRow in conjunto1.Tables["ArquivoTxt"].Rows)
        {
            string codBuscado = pRow["cod"].ToString();
            string strConexao = @"Data Source=hspmins4;Initial Catalog=Geral_Treina;User Id=h010994;Password=soundgarden";
            string strQuery = "";


            /*  UPDATE table_name
  SET column1=value1,column2=value2,...
  WHERE some_column=some_value;*/

            strQuery = "UPDATE [Geral_Treina].[dbo].[Exames_Paciente]" +
                "SET impr = 'true' where cod = " + codBuscado;

            using (SqlConnection conn = new SqlConnection(strConexao))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strQuery, conn);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string erro = ex.Message;
                }
            }
        }//for each
    }


    protected void btnImpr_Click1(object sender, EventArgs e)
    {
        gridCarregaGridView1();
        setImprExmPaciente(exportarArquivo());
        exportarToTxt();
    }

    public string getTelefonePainelBordo(string rh)
    {
        string telefone = "";
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT telefone FROM [Geral_Treina].[dbo].[Fila] where rh =" + rh;
            cnn.Open();
            SqlDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                telefone = dr.GetString(0);
            }
        }
        return telefone;

    }

}
