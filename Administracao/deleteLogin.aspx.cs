using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Administracao_deleteLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txbUserId.Text = Request.QueryString["UserId"];
        txbUser.Text = Request.QueryString["UserName"];
    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        string userId = txbUserId.Text;
        if (!userId.Equals(""))
        {
            string sSqlMember = "DELETE FROM Geral_Treina.dbo.aspnet_Membership WHERE userId = '" + userId + "';";
            string sSqlUserInRole = "DELETE FROM Geral_Treina.dbo.aspnet_UsersInRoles WHERE UserId = '" + userId + "';";
            string sSqlUsers = "DELETE FROM Geral_Treina.dbo.aspnet_Users WHERE UserId = '" + userId + "';";

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices"].ToString()))
            {
                SqlCommand cmm = new SqlCommand();
                try
                {
                    cmm.Connection = cnn;
                    cnn.Open();

                    cmm.CommandText = sSqlMember + sSqlUserInRole + sSqlUsers;

                    cmm.ExecuteNonQuery();
                    //Response.Write("<script language='javascript'>alert('Gravado com Sucesso!');</script>");
                    Response.Redirect("~/Administracao/deleteLogin.aspx");
                }
                catch (SqlException e1)
                {
                    Response.Write("<script language='javascript'>alert('Erro ao inserir registro :' " + e1 + "');</script>");
                }
            }
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Selecione um registro');</script>");
        }
    }
}
