using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using CD.DAL.BusinessEntities;

/// <summary>
/// Summary description for SqlSv
/// </summary>
public class SqlSv
{
    public SqlSv()
    {
    }

    public static SQLInsert gravaPedidoExtra(int _id, int _tipId, String _nome, DateTime _dt, long _cpfMed,
        String _nomeMed, int _tipSet, int _codSet, String _nomeSet,int _tipexm)
    {
        SQLInsert sqi = new SQLInsert();

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {

            cnn.Open();
            SqlTransaction st = cnn.BeginTransaction();
            SqlCommand cmm = cnn.CreateCommand();
            cmm.Transaction = st;
            cmm.CommandText = "INSERT INTO Pedidos ([ID_Paciente]  ,[Nome_Paciente]  ,[Cod_Tipo_Paciente] "
                + ",[Procedencia],[Cod_Procedencia],[Cod_Tipo_Procedencia],[Nome_Solicitante],[CPF_Solcitante]"
                + ",[Cod_Status],[Cod_Tipo_Pedido],[Cod_Grupo_Exame]) VALUES "
                + "(@idpac,@nomep,@tippac,@nomeset,@codset,@tipset,@nomemed,@cpfmed,@status,@tipped,@tipexm);SELECT SCOPE_IDENTITY()";

            cmm.Parameters.Add("@idpac", SqlDbType.Int).Value = _id;
            cmm.Parameters.Add("@nomep", SqlDbType.NVarChar).Value = _nome;
            cmm.Parameters.Add("@tippac", SqlDbType.Int).Value = _tipId;
            cmm.Parameters.Add("@nomeset", SqlDbType.NVarChar).Value = _nomeSet;
            cmm.Parameters.Add("@codset", SqlDbType.Int).Value = _codSet;
            cmm.Parameters.Add("@tipset", SqlDbType.Int).Value = _tipSet;
            cmm.Parameters.Add("@nomemed", SqlDbType.NVarChar).Value = _nomeMed;
            cmm.Parameters.Add("@cpfmed", SqlDbType.BigInt).Value = _cpfMed;
            cmm.Parameters.Add("@status", SqlDbType.Int).Value = Tipos.Status.Checkin;
            cmm.Parameters.Add("@tipped", SqlDbType.Int).Value = Tipos.Pedido.Extra;
            cmm.Parameters.Add("@tipexm", SqlDbType.Int).Value = _tipexm;
            try
            {               
                int id = Convert.ToInt32(cmm.ExecuteScalar());
                sqi.Id = id;
                cmm.CommandText = "Insert into pedido_extra (num_pedido,datahora) values (@numped,@dth)";
                cmm.Parameters.Add("@numped", SqlDbType.Int).Value = id;
                cmm.Parameters.Add("@dth", SqlDbType.DateTime).Value = _dt.Date;
                cmm.ExecuteNonQuery();

                cmm.CommandText = "Insert into checkin_Pedido (num_pedido,datahora) values (@numped,@dth)";
                cmm.ExecuteNonQuery();
                cmm.CommandText = "INSERT INTO Log_Pedido ([Num_Pedido],[TimeStamp],[Usuario],[Cod_Ent_Log]) "
                    + "VALUES (@numped,getdate(),@usu,@entlog)";
                cmm.Parameters.Add("@entlog", SqlDbType.Int).Value = 1;
                cmm.Parameters.Add("@usu", SqlDbType.UniqueIdentifier).Value = (Guid)Membership.GetUser().ProviderUserKey;
                cmm.ExecuteNonQuery();

                st.Commit();
                sqi.Success = true;
            }
            catch (Exception ex)
            {

                sqi.Success = false;
                sqi.Erro = ex.Message;
                try
                {
                    st.Rollback();
                }
                catch (Exception ex1)
                {
                    sqi.Erro += "  -  " + ex1.Message;

                }
            }

        }
        return sqi;
    }
    

    public static DadosEtiqueta getDadosEtiqueta(int _numped)
    {
        DadosEtiqueta etiq = new DadosEtiqueta();         
           
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT  Checkin_Pedido.DataHora, Pedidos.ID_Paciente, Pedidos.Nome_Paciente,"
                +"Pedidos.Cod_Tipo_Paciente, Pedidos.Procedencia,Grupo_Exame.descricao,Pedidos.Cod_Grupo_Exame "
                +"FROM Pedidos INNER JOIN Checkin_Pedido ON Pedidos.Num_Pedido = Checkin_Pedido.Num_Pedido "
                + "INNER JOIN Grupo_Exame on pedidos.cod_Grupo_Exame = Grupo_Exame.cod_Grupo_Exame "
                +"WHERE (Pedidos.Num_Pedido = @numped)";
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                if(dr.Read())
                {
                    etiq.Dt = dr.GetDateTime(0);
                    etiq.Id = dr.GetInt32(1);
                    
                    etiq.Nome = dr.GetString(2);
                    etiq.Numped =_numped;
                    etiq.CodTipId = dr.GetInt32(3);
                    etiq.Procedencia = dr.GetString(4);
                    etiq.TipExm = dr.GetString(5);
                    etiq.CodExm = dr.GetInt32(6);
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }
        }
        
        
        return etiq;
    }


    public static SqlDS.Atuacao_ProfissionalDataTable getAllAtuacao()
    {
        SqlDSTableAdapters.Atuacao_ProfissionalTableAdapter dap = new SqlDSTableAdapters.Atuacao_ProfissionalTableAdapter();
        return dap.GetData();
    }

    public static SqlDS.ProfissionaisDataTable getProfByCod(int _atu)
    {
        SqlDSTableAdapters.ProfissionaisTableAdapter dap = new SqlDSTableAdapters.ProfissionaisTableAdapter();
        return dap.GetDataByAtu(_atu);
    }

    public static SqlDS.ProfsFULLDataTable getFullProfByCod(int _atu)
    {
        SqlDSTableAdapters.ProfsFULLTableAdapter dap = new SqlDSTableAdapters.ProfsFULLTableAdapter();
        return dap.GetData(_atu);
    }


    public static void gravaProfAtu(String _nome, int _atu,int _codcli,String _clin)
    {
        SqlDSTableAdapters.ProfissionaisTableAdapter dap = new SqlDSTableAdapters.ProfissionaisTableAdapter();
        dap.Insert(_nome, _atu,_clin,_codcli);    
    }

    public static void delProfAtu(int _cod)
    {
        SqlDSTableAdapters.ProfissionaisTableAdapter dap = new SqlDSTableAdapters.ProfissionaisTableAdapter();
        dap.Delete(_cod);
    }

    public SqlDS.Grupo_ExameDataTable getAllTpExms()
    {
        SqlDSTableAdapters.Grupo_ExameTableAdapter dap = new SqlDSTableAdapters.Grupo_ExameTableAdapter();
        return dap.GetData();
    
    }


    public static SqlDS.MateriaisDataTable getMatByExm(int _exm)
    {
        SqlDSTableAdapters.MateriaisTableAdapter dap = new SqlDSTableAdapters.MateriaisTableAdapter();
        return dap.GetDataByExm(_exm);
    }

    public static void gravaMaterial(String _descr, int _exm)
    {
        SqlDSTableAdapters.MateriaisTableAdapter dap = new SqlDSTableAdapters.MateriaisTableAdapter();
        dap.Insert(_descr, _exm);
    }

    public static void delMaterial(int _cod)
    {
        SqlDSTableAdapters.MateriaisTableAdapter dap = new SqlDSTableAdapters.MateriaisTableAdapter();
        dap.Delete(_cod);
    }

    public static DadosPedido getDadosPedido(int _numped)
    {
        DadosPedido dp = new DadosPedido();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT Pedidos.ID_Paciente, Pedidos.Nome_Paciente, Pedidos.Cod_Tipo_Paciente, Pedidos.Cod_Procedencia, Pedidos.Cod_Tipo_Procedencia, "
         +"Pedidos.CPF_Solcitante, Pedidos.Cod_Grupo_Exame, Pedido_Extra.DataHora, Pedidos.Cod_Status, Grupo_Exame.Descricao "
+"FROM Pedidos INNER JOIN "
  +"Pedido_Extra ON Pedidos.Num_Pedido = Pedido_Extra.Num_Pedido INNER JOIN "
    +"Grupo_Exame ON Pedidos.Cod_Grupo_Exame = Grupo_Exame.Cod_Grupo_Exame "
+"WHERE (Pedidos.Num_Pedido = @numped)";
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            cnn.Open();
            try
            {
                SqlDataReader dr = cmm.ExecuteReader();
                if (dr.Read())
                {
                    dp.Numpedido = _numped;
                    dp.Id = dr.GetInt32(0);
                    dp.Nome = dr.GetString(1);
                    dp.Tip_id = dr.GetInt32(2);
                    dp.Cod_setor = dr.GetInt32(3);
                    dp.Tipo_Setor = dr.GetInt32(4);
                    dp.Cpf_med = dr.GetString(5);
                    dp.Exm_id = dr.GetInt32(6);
                    dp.Dt = dr.GetDateTime(7);
                    dp.Cod_status = dr.GetInt32(8);
                    dp.Exame = dr.GetString(9);
                }
            }
            catch (Exception ex)
            {
               string erro = ex.Message;
            }
        
        }
        return dp;
    }

    public static SQLInsert alteraPedido(int _numped,String _cpf,String _medico,int _tip_proc,int _cof_proc,
        String _proc,int _codexm,DateTime _dt) 
    {
        SQLInsert sqli = new SQLInsert();
        
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
          
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "UPDATE Pedidos SET Procedencia = @proc, Cod_Procedencia = @cod_proc,"
                         +"Cod_Tipo_Procedencia = @tip_proc, Nome_Solicitante = @medico, "
                         +"CPF_Solcitante = @cpf, Cod_Grupo_Exame = @codexm WHERE "
                         +"(Num_Pedido = @numped)";
             cnn.Open();
                SqlTransaction st = cnn.BeginTransaction();
            try
            {
               
                cmm.Transaction = st;
                cmm.Parameters.Add("@proc", SqlDbType.NVarChar).Value = _proc;
                cmm.Parameters.Add("@cod_proc", SqlDbType.Int).Value = _cof_proc;
                cmm.Parameters.Add("@tip_proc", SqlDbType.Int).Value = _tip_proc;
                cmm.Parameters.Add("@medico", SqlDbType.NVarChar).Value = _medico;
                cmm.Parameters.Add("@cpf", SqlDbType.NVarChar).Value = _cpf;
                cmm.Parameters.Add("@codexm", SqlDbType.Int).Value = _codexm;
                cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
                cmm.ExecuteNonQuery();
                cmm.CommandText = "UPDATE Pedido_Extra SET DataHora = @dt WHERE (Num_Pedido = @numped)";
                cmm.Parameters.Add("@dt", SqlDbType.DateTime).Value = _dt;
                cmm.ExecuteNonQuery();

                cmm.CommandText = "UPDATE Checkin_Pedido SET DataHora = @dt WHERE (Num_Pedido = @numped)";
                cmm.ExecuteNonQuery();

                cmm.CommandText = "INSERT INTO Log_Pedido ([Num_Pedido],[TimeStamp],[Usuario],[Cod_Ent_Log]) "
                   + "VALUES (@numped,getdate(),@usu,@entlog)";
                cmm.Parameters.Add("@entlog", SqlDbType.Int).Value = 2;
                cmm.Parameters.Add("@usu", SqlDbType.UniqueIdentifier).Value = (Guid)Membership.GetUser().ProviderUserKey;
                cmm.ExecuteNonQuery();

                st.Commit();
                sqli.Success = true;
            }
            catch (Exception ex)
            {
                try
                {
                    sqli.Success = false;
                    sqli.Erro = ex.Message;
                    st.Rollback();
                }
                catch (Exception ex1)
                {
                    sqli.Erro += ex1.Message;
                    
                }
            
            }
        
        }
        return sqli;
    }

    public static void cadProfPedido(int _idprof, int _numped)
    {     
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "INSERT INTO Profissionais_Pedido (Cod_Profissional, "
            +"Num_Pedido) VALUES (@prof,@numped)";
            cmm.Parameters.Add("@prof", SqlDbType.Int).Value = _idprof;
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
                
            }
        }
    
    
    }

    public static void delProfPedido(int _idprof, int _numped)
    {
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "DELETE FROM Profissionais_Pedido WHERE "
                +"(Num_Pedido = @numped) AND (Cod_Profissional = @prof)";
            cmm.Parameters.Add("@prof", SqlDbType.Int).Value = _idprof;
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }
        }


    }

    public static Profs getProfsPed(int _ped)
    {
        Profs profs = new Profs();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT Profissionais_Pedido.Cod_Profissional, Profissionais.Nome "
                +"FROM Profissionais INNER JOIN Profissionais_Pedido ON "
            +"Profissionais.Cod_Profissional = Profissionais_Pedido.Cod_Profissional "
            + "WHERE (Profissionais_Pedido.Num_Pedido = @numped) order by Profissionais.Nome ";
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _ped;
            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                Prof pf;
                while (dr.Read())
                {
                    pf = new Prof();
                    pf.Cod = dr.GetInt32(0);
                    pf.Nome = dr.GetString(1);
                   
                    profs.Items.Add(pf.Cod, pf);
                }
            }
            catch
            {
            }

        }
        return profs;  
    }

    public static MateriaisUtils getMatsPed(int _ped)
    {
        MateriaisUtils mats = new MateriaisUtils();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT Materiais_Pedido.Cod_Material, Materiais.Descricao, Materiais_Pedido.Qtd " 
                             +"FROM Materiais_Pedido INNER JOIN Materiais ON "
                             +"Materiais_Pedido.Cod_Material = Materiais.Cod_Material "
                             + "WHERE (Materiais_Pedido.Num_Pedido = @numped) order by Materiais.Descricao ";
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _ped;
            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                MaterialUtil mat;
                while (dr.Read())
                {
                    mat = new MaterialUtil();
                   mat.Cod= dr.GetInt32(0);
                    mat.Descr = dr.GetString(1);
                    mat.Qtd = dr.GetInt32(2);
                    mat.Descr = mat.Descr + "   - Qtd: " + mat.Qtd.ToString();
                    mats.Items.Add(mat.Cod, mat);
                }
            }
            catch
            {
            }

        }
        return mats;
    }

    public static void cadMatPedido(int _mat, int _numped,int _qtd)
    {
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "INSERT INTO Materiais_Pedido (Cod_Material, "
            + "Num_Pedido,Qtd) VALUES (@mat,@numped,@qtd)";
            cmm.Parameters.Add("@mat", SqlDbType.Int).Value = _mat;
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            cmm.Parameters.Add("@qtd", SqlDbType.Int).Value = _qtd;

            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }
        }


    }

    public static void delMatPedido(int _mat, int _numped)
    {
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "DELETE FROM Materiais_Pedido WHERE "
                + "(Num_Pedido = @numped) AND (Cod_Material = @mat)";
            cmm.Parameters.Add("@mat", SqlDbType.Int).Value = _mat;
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }
        }


    }

    public static SqlDS.ExamesDataTable getExmsGrupo(int _gp)
    {
        SqlDSTableAdapters.ExamesTableAdapter dap = new SqlDSTableAdapters.ExamesTableAdapter();
        return dap.GetDataByGrupo(_gp);
    }
    
    public static ExamesRealzs getExmsPed(int _ped)
    {
        ExamesRealzs exms = new ExamesRealzs();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT Exames_Pedido.Cod_Exame, Exames.Descricao FROM Exames_Pedido "
                             +"INNER JOIN Exames ON Exames_Pedido.Cod_Exame = Exames.Cod_Exame "
                             + "WHERE (Exames_Pedido.Num_Pedido = @numped) order by Exames.Descricao";
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _ped;
            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                ExameRealz exm;
                while (dr.Read())
                {
                    exm = new ExameRealz();
                    exm.Cod_exame = dr.GetInt32(0);
                    exm.Descr = dr.GetString(1);
                    exms.Items.Add(exm.Cod_exame, exm);
                }
            }
            catch 
            {               
            }
          
            
        
        }
        return exms;
    }

    public static void cadExmPedido(int _exm, int _numped)
    {
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "INSERT INTO Exames_Pedido (Cod_Exame, "
            + "Num_Pedido) VALUES (@exm,@numped)";
            cmm.Parameters.Add("@exm", SqlDbType.Int).Value = _exm;
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }
    }

    public static void delExmPedido(int _exm, int _numped)
    {
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "DELETE FROM Exames_Pedido WHERE "
                + "(Num_Pedido = @numped) AND (Cod_Exame = @exm)";
            cmm.Parameters.Add("@exm", SqlDbType.Int).Value = _exm;
            cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }
        }


    }

    public static void gravaExmGrupo(string _desc, int _gp)
    {
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "INSERT INTO Exames (Descricao, "
            + "Cod_Grupo_Exame) VALUES (@desc,@grupo)";
            cmm.Parameters.Add("@desc", SqlDbType.NVarChar).Value = _desc;
            cmm.Parameters.Add("@grupo", SqlDbType.Int).Value = _gp;
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }
    }

    public static void delExmGrupo(int _exm)
    {

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "DELETE FROM Exames WHERE "
                + "Cod_Exame = @exm";
            cmm.Parameters.Add("@exm", SqlDbType.Int).Value = _exm;
           
            try
            {
                cnn.Open();
                cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }
        }

    }


    public static SQLInsert gravaFechamentoPedido(int _numped, ExamesRealzs _exms, Profs _profs, MateriaisUtils _mats, int _status)
    {
        SQLInsert sqi = new SQLInsert();

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
          
                         
                cnn.Open();
                 SqlTransaction st= cnn.BeginTransaction();
                cmm.Transaction = st;
                try
                {

                    cmm.CommandText = "select cod_status from pedidos where num_pedido = @numped";
                    cmm.Parameters.Add("@numped", SqlDbType.Int).Value = _numped;
                    int status =(int)cmm.ExecuteScalar();
                    int ent_log = 4;
                    if (status != 3)
                    {
                        cmm.CommandText = "update pedidos set cod_status = 3 where num_pedido = @numped";
                        cmm.ExecuteNonQuery();
                        ent_log = 3;
                    }
                    cmm.CommandText = "DELETE FROM Exames_Pedido WHERE (Num_Pedido = @numped)";
                   
                    cmm.ExecuteNonQuery();

                    if (_exms.Items.Count > 0)
                    {
                        cmm.CommandText = "INSERT INTO Exames_Pedido (Cod_Exame,Num_Pedido) VALUES (@exm,@numped)";
                        cmm.Parameters.Add("@exm", SqlDbType.Int);

                        foreach (ExameRealz exm in _exms.Items.Values)
                        {
                            cmm.Parameters["@exm"].Value = exm.Cod_exame;
                            cmm.ExecuteNonQuery();
                        }
                    }

                    cmm.CommandText = "DELETE FROM Profissionais_Pedido WHERE (Num_Pedido = @numped)";                  
                    cmm.ExecuteNonQuery();

                    if (_profs.Items.Count > 0)
                    {
                        cmm.CommandText = "INSERT INTO Profissionais_Pedido (Cod_Profissional, Num_Pedido) VALUES (@prof,@numped)";
                        cmm.Parameters.Add("@prof", SqlDbType.Int);

                        foreach (Prof pf in _profs.Items.Values)
                        {
                            cmm.Parameters["@prof"].Value = pf.Cod;
                            cmm.ExecuteNonQuery();
                        }
                    }

                    cmm.CommandText = "DELETE FROM Materiais_Pedido WHERE (Num_Pedido = @numped)";
                    cmm.ExecuteNonQuery();

                    if (_mats.Items.Count > 0)
                    {
                        cmm.CommandText = "INSERT INTO Materiais_Pedido (Cod_Material, Num_Pedido,Qtd) VALUES (@mat,@numped,@qtd)";
                        cmm.Parameters.Add("@mat", SqlDbType.Int);
                        cmm.Parameters.Add("@qtd", SqlDbType.Int);

                        foreach (MaterialUtil mt in _mats.Items.Values)
                        {
                            cmm.Parameters["@mat"].Value = mt.Cod;
                            cmm.Parameters["@qtd"].Value = mt.Qtd;
                            cmm.ExecuteNonQuery();
                        }
                    }

                    cmm.CommandText = "INSERT INTO Log_Pedido ([Num_Pedido],[TimeStamp],[Usuario],[Cod_Ent_Log]) "
                   + "VALUES (@numped,getdate(),@usu,@entlog)";
                    cmm.Parameters.Add("@entlog", SqlDbType.Int).Value = ent_log;
                    cmm.Parameters.Add("@usu", SqlDbType.UniqueIdentifier).Value = (Guid)Membership.GetUser().ProviderUserKey;
                    cmm.ExecuteNonQuery();

                    st.Commit();
                    sqi.Success = true;


                }
                catch (Exception ex)
                {
                    sqi.Success = false;
                    sqi.Erro = ex.Message;
                    try
                    {
                        st.Rollback();
                    }
                    catch (Exception ex1)
                    {
                        sqi.Erro += ex1.Message;

                    }
                }
        }

        return sqi;
    
    }


    public static PedidosDetails getPedidosByID(int _id, int _tipo_id)
    {
        PedidosDetails pds = new PedidosDetails();

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT Grupo_Exame, Setor, Status, Procedencia, Nome_Paciente," 
                              +"Num_Pedido, Nome_Solicitante,data FROM Pedidos_Detais "
                              +"WHERE (ID_Paciente = @id) AND (Cod_Tipo_Paciente = @tipo) "
                              +"order by num_pedido";
            cmm.Parameters.Add("@id", SqlDbType.Int).Value = _id;
            cmm.Parameters.Add("@tipo", SqlDbType.Int).Value = _tipo_id;
            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();
                PedidoDetail pd;
                while (dr.Read())
                {
                    pd = new PedidoDetail();
                    pd.Exame = dr.GetString(0);
                    pd.Setor = dr.GetString(1);
                    pd.Status = dr.GetString(2);
                    pd.Procedencia = dr.GetString(3);
                    pd.Paciente = dr.GetString(4);
                    pd.Numped = dr.GetInt32(5);
                    pd.Solicitante = dr.GetString(6);
                    pd.Data = dr.GetDateTime(7);
                   
                    pd.Id_pac = _id;
                    if (_tipo_id == 1)
                        pd.Tipo_id = "RH";
                    else
                        pd.Tipo_id = "BE";
                    pds.Items.Add(pd);
                }

            }
            catch(Exception ex) { }
        }

        return pds;
    
    
    }



    public static PedidoDetail getPedidoByPed(int _ped)
    {
        PedidoDetail pd = new PedidoDetail();

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConn"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT Grupo_Exame, Setor, Status, Procedencia, Nome_Paciente,"
                              + "Nome_Solicitante,data,id_paciente,cod_tipo_paciente FROM Pedidos_Detais "
                              + "WHERE (Num_pedido = @ped)";
            cmm.Parameters.Add("@ped", SqlDbType.Int).Value = _ped;
           
            try
            {
                cnn.Open();
                SqlDataReader dr = cmm.ExecuteReader();              
                if (dr.Read())
                {                  
                    pd.Exame = dr.GetString(0);
                    pd.Setor = dr.GetString(1);
                    pd.Status = dr.GetString(2);
                    pd.Procedencia = dr.GetString(3);
                    pd.Paciente = dr.GetString(4);                  
                    pd.Solicitante = dr.GetString(5);
                    pd.Data = dr.GetDateTime(6);
                    pd.Id_pac = dr.GetInt32(7);
                    pd.Numped = _ped;
                    int _tipo_id = dr.GetInt32(8);
                    if (_tipo_id == 1)
                        pd.Tipo_id = "RH";
                    else
                        pd.Tipo_id = "BE";                   
                }
            }
            catch (Exception ex) { }
        }

        return pd;
    }

    public static SqlDS.vw_Log_PedidoDataTable getHistbyPed(int _numped)
    {
        SqlDSTableAdapters.vw_Log_PedidoTableAdapter dap = new SqlDSTableAdapters.vw_Log_PedidoTableAdapter();
        return dap.GetDatabyPed(_numped);  
    }

    public static Reports.vw_Entradas_LogDataTable getRelLogDia(int _mes,int _ano)
    {
        ReportsTableAdapters.vw_Entradas_LogTableAdapter dap = new ReportsTableAdapters.vw_Entradas_LogTableAdapter();
        return dap.GetData(_ano,_mes);
    }

    public static Reports.vw_Entradas_Log_MensalDataTable getRelLogMes(int _mes, int _ano)
    {
        ReportsTableAdapters.vw_Entradas_Log_MensalTableAdapter dap = new ReportsTableAdapters.vw_Entradas_Log_MensalTableAdapter();
        return dap.GetData(_mes,_ano);
   }
}
