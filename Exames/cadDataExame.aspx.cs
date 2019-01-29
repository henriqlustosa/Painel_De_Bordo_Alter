using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Exames_cadDataExame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbPaciente.Text = Request.QueryString["pac"];
        lbRh.Text = Request.QueryString["rh"]; 
        lbExame.Text = Request.QueryString["exm"];
        lbfilaExm.Text = Request.QueryString["fila"];
    }

    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
        int er = 0;
        string erro = "";
        string rh = lbRh.Text;
        int filaExm = Convert.ToInt32(lbfilaExm.Text);
        string numCon = txbNumCon.Text;
        string dtCon = txbDt.Text;
        string hrCon = txbHr.Text;
        string executante = txbExecutante.Text;
        int status = 0;//status zero - marcada ativo HSPM
        if (chbAtivo.Checked)
        {
            status = 1;//status 1 - marcada ativo 156
        }
        string sSql = "Insert Into Exames_Int_Marcado (cod_exm_pac, num_consulta, dt_consulta, hr_consulta, executante, status) "+
            " Values (@cod_exm,@num_com, @dt, @hr, @executante,@status);";

        string upSql = "UPDATE Exames_Paciente SET status = 1 where cod = " + filaExm;//atualiza exames marcados (1-marcada;2-realizada;3-cancelada;4-aguardando vaga) 
        
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = new SqlCommand();
            SqlCommand cmm1 = new SqlCommand();
            try
            {
                cmm.Connection = cnn;
                cmm1.Connection = cnn;
                cnn.Open();

                cmm1.CommandText = upSql;

                cmm.CommandText = sSql;
                cmm.Parameters.Add("@cod_exm", SqlDbType.Int).Value = filaExm;
                cmm.Parameters.Add("@num_com", SqlDbType.VarChar).Value = numCon;
                cmm.Parameters.Add("@dt", SqlDbType.Date).Value = Convert.ToDateTime(dtCon).ToShortDateString();
                cmm.Parameters.Add("@hr", SqlDbType.VarChar).Value = hrCon;
                cmm.Parameters.Add("@executante", SqlDbType.VarChar).Value = executante;
                //impr - default value = 0 na tabela
                cmm.Parameters.Add("@status", SqlDbType.Int).Value = 1;//1-marcada;2-realizada;3-cancelada;4-aguardando vaga
                cmm.ExecuteNonQuery();
                cmm1.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                erro = ex.Message;
                //Response.Write("<script language='javascript'>alert('Erro na operação " + ex + "');</script>");
                er = 1;
            }
            if (er == 0)
            {
                Response.Write("<script language=javascript>alert('Cadastrado com sucesso!');</script>");
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
            }
            else if (er == 1)
            {
                Response.Write("<script language='javascript'>alert('Erro na operação " + erro + "');</script>");
            }
        }
    }
}
