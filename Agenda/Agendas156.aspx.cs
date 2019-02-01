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
using System.IO;
using System.Data.Odbc;

public partial class Agenda_Agendas156 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlEspecialidadeSub();
            Panel1.Visible = false;
        }
    }

    private void ddlEspecialidadeSub()
    {
        using (OdbcConnection cnn3 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm3 = cnn3.CreateCommand();
            cmm3.CommandText = "select ia9codprin, ia9codsub, ia9descr from inta9 where ia9descr like '%156%' order by ia9descr";
            cnn3.Open();

            OdbcDataReader dr3 = cmm3.ExecuteReader();
            while (dr3.Read())
            {
                string codPrin = dr3.GetString(0);
                string codSub = dr3.GetString(1);
                string codigo = dr3.GetString(0).Substring(0, 3) + "." + dr3.GetString(0).Substring(3, 1) + "/" + dr3.GetString(1);
                string nome = dr3.GetString(2).TrimEnd();

                while (nome.IndexOf("  ") >= 0)
                {
                    nome = nome.Replace("  ", " ");
                }
                int size = (40 - nome.Length);

                for (int i = 0; i < size; i++)
                {
                    nome = nome.Insert(nome.Length, ".");
                }
                string codigoEspec = dr3.GetString(0) + dr3.GetString(1);

                ddlEspecialidades.Items.Add(new ListItem(nome + codigo, codigoEspec));
            }
        }
    }

    protected void btPesquisarProfissional_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        //GridView2.DataSource = null;
        //GridView2.DataBind();

        lbcodescala.Text = "Código da especialidade: " + ddlEspecialidades.SelectedValue;
        GridView1.DataSource = gridProfissionais(ddlEspecialidades.SelectedValue);
        GridView1.DataBind();

        GridView3.DataSource = null;
        GridView3.DataBind();

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        string profissional = GridView1.SelectedRow.Cells[1].Text;
        profissional = profissional.Replace(".", "").Replace("-", "");

        GridView3.DataSource = gridDatasFuturas(ddlEspecialidades.SelectedValue, profissional);
        GridView3.DataBind();

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }
    private void ExportGridToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Agenda" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        GridView3.GridLines = GridLines.Both;
        GridView3.HeaderStyle.Font.Bold = true;
        GridView3.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }

    protected void btnExportarExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    //////////////////////////////
    public static String getNome(string _cod)
    {  
        string desc = "";
        
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "Select ic0nome from intc0 where ic0cpf = " + _cod;
            cnn.Open();
            OdbcDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                desc = dr.GetString(0);
            }
        }
        return desc;
    }
    public static DataTable gridProfissionais(string _codAgenda)
    {

        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            DataTable dt = new DataTable();
            OdbcCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "select distinct am1901, ic0nome from tsql.am19, tsql.intc0 where am1901 = ic0cpf and am1903 = " + _codAgenda + "order by ic0nome";
            try
            {
                cnn.Open();
                OdbcDataReader dr1 = cmm.ExecuteReader();
                dt.Columns.Add("CPF", System.Type.GetType("System.String"));
                dt.Columns.Add("Nome do Profissional", System.Type.GetType("System.String"));
                char[] ponto = { '.', ' ' };
                while (dr1.Read())
                {

                    string cpf = dr1.GetString(0).TrimEnd(ponto);
                    cpf = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
                    string codAgenda = dr1.GetString(1);

                    dt.Rows.Add(new String[] { cpf, codAgenda });
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return dt;
        }
    }
        public static DataTable gridDatasFuturas(string _codAgenda, string _cpf)
    {
        int statusConsulta = 5;// status da consulta 5 (impedimento - vaga)
        DataTable dt = new DataTable();
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm = cnn.CreateCommand();

            cmm.CommandText = "select am1104, am1103,am1106,am1113 from tsql.am11 where am1110 = " + statusConsulta + " and am1103 = " + _cpf + " and am1104 = " + _codAgenda;
            try
            {
                cnn.Open();
                OdbcDataReader dr1 = cmm.ExecuteReader();

                dt.Columns.Add("Cód Especialidade", System.Type.GetType("System.String"));
                dt.Columns.Add("Especialidade", System.Type.GetType("System.String"));
                dt.Columns.Add("CPF Profissional", System.Type.GetType("System.String"));
                dt.Columns.Add("Nome Profissional", System.Type.GetType("System.String"));
                dt.Columns.Add("Data", System.Type.GetType("System.String"));
                dt.Columns.Add("Hora", System.Type.GetType("System.String"));
                char[] ponto = { '.', ' ' };
                while (dr1.Read())
                {
                    string codEspec = dr1.GetString(0).TrimEnd(ponto);
                    string _codEspec = Convert.ToUInt64(codEspec).ToString(@"000\.0\/00");
                    string descEspec = getespec(codEspec);
                    string cpf = dr1.GetString(1).TrimEnd(ponto);
                    cpf = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
                    string nomeProf = getNome(dr1.GetString(1));
                    string data = DataFormatada(dr1.GetString(2));
                    string hora = calcularHora(dr1.GetString(3).TrimEnd(ponto));

                    dt.Rows.Add(_codEspec, descEspec, cpf, nomeProf, data, hora);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        return dt;
    }

        public static String DataFormatada(string dt)
        {
            string data = dt;
            int ano = Convert.ToInt32(data.Substring(0, 4));
            int mes = Convert.ToInt32(data.Substring(4, 2));
            int dia = Convert.ToInt32(data.Substring(6, 2));
            data = dia + "/" + mes + "/" + ano;

            return data;
        }
        public static String calcularHora(string horaAcumulada)
        {
            horaAcumulada = horaAcumulada.Replace(".", "");

            int horaPad = Convert.ToInt32(horaAcumulada);

            string Hora = Convert.ToString(horaPad / 60);
            if (Hora.Length.Equals(1))
                Hora = Hora.PadLeft(2, '0');
            string Minutos = Convert.ToString(horaPad % 60);
            if (Minutos.Length.Equals(1))
                Minutos = Minutos.PadLeft(2, '0');
            string horaPadrao = Hora + ":" + Minutos;

            return horaPadrao;
        }

        public static String getespec(string _cod)
        {
            string desc = "";
            string cod = "";
            string sub = "";

            string espec = _cod;

            if (espec.Length == 4)
            {
                cod = "00" + espec.Substring(0, 2);
                sub = espec.Substring(3, 2);
            }
            else if (espec.Length == 5)
            {
                cod = "0" + espec.Substring(0, 3);
                sub = espec.Substring(3, 2);
            }
            //else if (espec.Length == 6)
            //{
            //    cod = "0" + espec.Substring(0, 3);
            //    sub = espec.Substring(3, 2);
            //}


            using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
            {
                OdbcCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "Select ia9descr from inta9 where ia9codprin = '" + cod + "' and ia9codsub = '" + sub + "'";
                cnn.Open();
                OdbcDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    desc = dr.GetString(0);
                }
            }
            return desc;
        }
}
