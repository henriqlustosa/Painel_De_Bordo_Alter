using System;
using System.Configuration;
using System.Data.Odbc;
using System.Data.SqlClient;

public partial class Consulta_AgendamentoRetorno : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            btnCadastrar.Enabled = false;
            lbRh.Text = Request.QueryString["rh"];
            lbPaciente.Text = Request.QueryString["pac"];
           
            lbEspecialidade.Text = Request.QueryString["espec"];
            lbSolicitante.Text = Request.QueryString["solic"];

            string simMarcada = Request.QueryString["simMarcada"];
            if (simMarcada == "Sim")
            {
                btnCadastrar.Enabled = false;
                ddlMarcada.SelectedValue = simMarcada;
                txbNumConsulta.Text = Request.QueryString["numConsulta"];
                txbData.Text = Request.QueryString["dataRegistro"];
                txbHora.Text = Request.QueryString["horaRegistro"];

                string situacao = Request.QueryString["situacao"];
                if (situacao == "Ativo HSPM")//desnecessário
                {

                    rdOpcao.SelectedValue = "1";

                }
                else if (situacao == "Cancelada")//por enquanto desnecessário
                {
                    rdOpcao.SelectedValue = "2";
                }

                else  
                {
            
                    rdOpcao.SelectedValue = "";
                }
            }
            else
                btnCadastrar.Enabled = true;
         }
     }
      
    
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        btnCadastrar.Enabled = true ;
       
        string rhParaPesquisa = lbRh.Text;
        string numConsPesq = txbNumConsulta.Text;
        bool numExiste = verificarNumeroDaConsulta(numConsPesq, rhParaPesquisa);
        txbData.Enabled = false;
        
    }
    public bool testeEspecialidadeSub(string espSub)
    {
        if (!(espSub.Length == 6))
         espSub = espSub.PadLeft(6, '0');
        string rhParaPesquisar = lbRh.Text;
        string espSubPrin = espSub.Substring(0, 4);
        string espSubSub = espSub.Substring(4,2);
        string nomeEspecHospub = "";

        string query3 = "select ia9descr from inta9 where ia9codprin ='" + espSubPrin + "' and ia9codsub = '" + espSubSub +"'";

        using (OdbcConnection cnn3 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm3 = new OdbcCommand();
            try
            {
                cmm3.Connection = cnn3;
                cnn3.Open();
                cmm3.CommandText = query3;
                OdbcDataReader dr3 = cmm3.ExecuteReader();
                if (dr3.Read())
                {
                   nomeEspecHospub = dr3.GetString(0);


                }

                    string query4 = "SELECT * FROM [Geral_Treina].[dbo].[Fila] where rh =" + rhParaPesquisar + " and especialidade = '" + nomeEspecHospub +"'";
               
                using (SqlConnection cnn4 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {
                  SqlCommand cmm4 = new SqlCommand();
                  try
                  {
                      cmm4.Connection = cnn4;
                      cnn4.Open();
                      cmm4.CommandText = query4;
                      SqlDataReader dr4 = cmm4.ExecuteReader();
                   

                      if (!dr4.Read())
                      {
                        Response.Write("<script language=javascript>alert('Nº da Consulta não é o mesmo da Especialidade!');</script>");
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
                        txbNumConsulta.Text = "";
                        return false;
                        
                      }
                      

                  }
                  catch (SqlException ex)
                  {
                     string erro = ex.Message;
                  }
                

              }

                

            }
            catch (SqlException ex)
            {
                string erro = ex.Message;
              
            }
     


        }
        return true;

    }
    public bool verificarNumeroDaConsulta(string numConsulta, string numRh)
    {
        bool numExiste = true;
        int er = 0;
        string erro = "";
        string[] split = numConsulta.Split('-');

        string query1 = "select * from am11 where am1101b = " + split[0] + " and am1101c = " + split[1];

        string query2 = "select am1106,am1113,am1104,am1103 from am11 where am1101b = " + split[0] + " and am1101c =" + split[1] + " and am1108 = " + numRh;
        string query3 = "select * from am56 where am5608b = " + split[0] + " and am5608c = " + split[1];
        string query4 = "select am5605,am5602,am5605 from am56 where am5608b = " + split[0] + " and am5608c =" + split[1] + " and am5604 = " + numRh;
        using (OdbcConnection cnn = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm1 = new OdbcCommand();
            OdbcCommand cmm2 = new OdbcCommand();
            OdbcCommand cmm3 = new OdbcCommand();
            OdbcCommand cmm4 = new OdbcCommand();
            try
            {
                cmm1.Connection = cnn;
                cmm2.Connection = cnn;
                cmm3.Connection = cnn;
                cmm4.Connection = cnn;

                cnn.Open();

                cmm1.CommandText = query1;
                OdbcDataReader dr1 = cmm1.ExecuteReader();
                cmm3.CommandText = query3;
                OdbcDataReader dr3 = cmm3.ExecuteReader();
                if (!dr1.Read() || !dr3.Read())
                {
                    numExiste = false;
                    Response.Write("<script language=javascript>alert('Nº da Consulta não existe!');</script>");
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
                    txbNumConsulta.Text = "";

                }
                else
                {
                    cmm2.CommandText = query2;
                    OdbcDataReader dr2 = cmm2.ExecuteReader();
                    cmm4.CommandText = query4;
                    OdbcDataReader dr4 = cmm4.ExecuteReader();
                    if (!dr2.Read() && !dr4.Read())
                    {
                        numExiste = false;
                        Response.Write("<script language=javascript>alert('Nº da Consulta não é o mesmo do Registro Hospitalar !');</script>");
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
                        txbNumConsulta.Text = "";
                      

                    }
                    else if (dr2.Read())
                    {
                        string espSub = dr2.GetString(2);      // codigo que contém especialidade e subespecialidade 
                        espSub = espSub.Replace(".", "");
                        bool testEsp = testeEspecialidadeSub(espSub);

                        string profissionalCpf = dr2.GetString(3);        // CPF do médico/profissional 
                        profissionalCpf = profissionalCpf.Replace(".", "");
                        bool testProf = testeProfissional(profissionalCpf);



                        if (testEsp == true && testProf == true)
                        {

                            txbData.Text = recuperarData(Convert.ToString(dr1.GetDecimal(0))); // data no formato aaaammdd 
                            txbData.Enabled = false;
                            txbHora.Text = calcularHora(Convert.ToString(dr1.GetDecimal(1)));
                            // txbHora.Enabled = false;
                            ddlMarcada.SelectedValue = "Sim";
                        }

                    }
                    else
                    {
                        string espSub = dr4.GetString(1);      // codigo que contém especialidade e subespecialidade 
                        espSub = espSub.Replace(".", "");
                        bool testEsp = testeEspecialidadeSub(espSub);





                        if (testEsp == true)
                        {

                            txbData.Text = recuperarData(Convert.ToString(dr4.GetDecimal(2))); // data no formato aaaammdd 
                            txbData.Enabled = false;
                            txbHora.Text = "";
                            // txbHora.Enabled = false;
                            ddlMarcada.SelectedValue = "Sim";
                        }

                    }

                }

            }
            catch (SqlException ex)
            {
                erro = ex.Message;
                //Response.Write("<script language='javascript'>alert('Erro na operação " + ex + "');</script>");
                er = 1;
            }
        
            if (er == 1)
            {
                Response.Write("<script language='javascript'>alert('Erro na operação " + erro + "');</script>");
            }
        }

        return numExiste;
        
    }
    public bool testeProfissional(string cpfProfissional)
    {
        string rhParaPesquisar = lbRh.Text;
        string nomeProfissional = "";

        string query5 = "select ic0nome from intc0 where ic0cpf = " + cpfProfissional;

        using (OdbcConnection cnn5 = new OdbcConnection(ConfigurationManager.ConnectionStrings["HospubConn"].ToString()))
        {
            OdbcCommand cmm5 = new OdbcCommand();
            try
            {
                cmm5.Connection = cnn5;
                cnn5.Open();
                cmm5.CommandText = query5;
                OdbcDataReader dr5 = cmm5.ExecuteReader();
                if (dr5.Read())
                {
                    nomeProfissional = dr5.GetString(0);
                }
                string query6 = "SELECT * FROM [Geral_Treina].[dbo].[Fila] where rh = " + rhParaPesquisar + " and solicitante = '" + nomeProfissional + "'";
                using (SqlConnection cnn6 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {
                    SqlCommand cmm6 = new SqlCommand();
                    try
                    {
                        cmm6.Connection = cnn6;
                        cnn6.Open();
                        cmm6.CommandText = query6;
                        SqlDataReader dr6 = cmm6.ExecuteReader();


                        if (!dr6.Read())
                        {
                            Response.Write("<script language=javascript>alert('Nº da Consulta não é o mesmo do Solicitante!');</script>");
                            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
                            txbNumConsulta.Text = "";
                            return false;
                        }


                    }
                    catch (SqlException ex)
                    {
                        string erro = ex.Message;
                    }

                }


            }
            catch (SqlException ex)
            {
               string erro = ex.Message;

            }



        }

        return true;

    }
    public string recuperarData(string Data)
    {
        Data = Data.Replace(".", "");

        string ano = Data.Substring(0, 4);
        string mes = Data.Substring(4, 2);
        string dia = Data.Substring(6, 2);

  
         string dataPadrao =  dia  + mes  + ano;

         return dataPadrao;
    }
    public string calcularHora(string horaAcumulada)
    {
        horaAcumulada = horaAcumulada.Replace(".", "");

        int horaPad = Convert.ToInt32(horaAcumulada);

        string Hora = Convert.ToString(horaPad / 60);
        if (Hora.Length.Equals(1))
            Hora = Hora.PadLeft(2, '0');
        string Minutos = Convert.ToString(horaPad % 60);
        if (Minutos.Length.Equals(1))
            Minutos = Minutos.PadLeft(2, '0');
        string horaPadrao = Hora + Minutos;

        return horaPadrao;
    }



    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
       
        int erro = 0;
        string err = "";

  
        string marcada = ddlMarcada.SelectedValue;  
        string consulta = txbNumConsulta.Text;         
        string dtConsulta = txbData.Text; 
      
        string hrConsulta = txbHora.Text;

        
        string rhParaPesquisa = lbRh.Text;
        string numConsPesq = txbNumConsulta.Text;

        bool numExiste = verificarNumeroDaConsulta(numConsPesq, rhParaPesquisa);
        if (numExiste == true)
        {
            string fila = Request.QueryString["fila"];

            if (rdOpcao.SelectedValue == "1")
            {
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {
                    SqlCommand cmm = cnn.CreateCommand();
                    cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Fila] SET impr = 'true', situacao = 'Ativo HSPM' where cod =" + fila;


                    try
                    {
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;

                    }
                }

            }
            else if (rdOpcao.SelectedValue == "2")
            {
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {
                    SqlCommand cmm = cnn.CreateCommand();
                    cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Fila] SET  situacao = 'Cancelada' where cod =" + fila;
                    try
                    {
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;

                    }
                }
            }
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Fila] SET marcada = '"+ marcada + "', consulta = '" + consulta + "',dtCon = '" + dtConsulta + "', hrCon ='" + hrConsulta +"' where cod =" + fila;

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
    
        if (erro.Equals(0))
        {
            Response.Write("<script language=javascript>alert('Arquivo gravado com sucesso!');</script>");
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
        }
        else if (erro.Equals(1))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro ao gravar: " + err + "');", true);
            

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro: Contate o administrador');", true); //mensagem de alerta de exames realizados do paciente antes de um ano
            
        }

       }
    }


    protected void btnAtualizar_Click(object sender, EventArgs e)
    {
        int erro = 0;
        string err = "";


        string marcada = ddlMarcada.SelectedValue;
        string consulta = txbNumConsulta.Text;
        string dtConsulta = txbData.Text;

        string hrConsulta = txbHora.Text;
        string rhParaPesquisa = lbRh.Text;
        string numConsPesq = txbNumConsulta.Text;

        bool numExiste = verificarNumeroDaConsulta(numConsPesq, rhParaPesquisa);
        if (numExiste == true)
        {



            string fila = Request.QueryString["fila"];

            if (rdOpcao.SelectedValue == "1")
            {
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {
                    SqlCommand cmm = cnn.CreateCommand();
                    cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Fila] SET impr = 'true', situacao = 'Ativo HSPM' where cod =" + fila;


                    try
                    {
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;

                    }






                }

            }
            else if (rdOpcao.SelectedValue == "2")
            {
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
                {
                    SqlCommand cmm = cnn.CreateCommand();
                    cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Fila] SET  situacao = 'Cancelada' where cod =" + fila;


                    try
                    {
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;

                    }






                }



            }

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = cnn.CreateCommand();
                cmm.CommandText = "UPDATE [Geral_Treina].[dbo].[Fila] SET marcada = '" + marcada + "', consulta = '" + consulta + "',dtCon = '" + dtConsulta + "', hrCon ='" + hrConsulta + "' where cod =" + fila;



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

            if (erro.Equals(0))
            {
                Response.Write("<script language=javascript>alert('Arquivo atualizado com sucesso!');</script>");
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
            }
            else if (erro.Equals(1))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro ao gravar: " + err + "');", true);


            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Erro: Contate o administrador');", true); //mensagem de alerta de exames realizados do paciente antes de um ano

            }


        }

    }
}
