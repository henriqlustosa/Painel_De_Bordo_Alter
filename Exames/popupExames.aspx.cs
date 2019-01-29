using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Exames_popupExames : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbrh.Text = Request.QueryString["rh"];
        lbpac.Text = Request.QueryString["pac"];
        lbfila.Text = Request.QueryString["fila"];
        lbexm.Text = Request.QueryString["exm"];
        lbsol.Text = Request.QueryString["solic"];
    }
    protected void btnExterno_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(500/2);var Mtop = (screen.height/2)-(500/2);window.open( '../Exames/cadDataExameExterno.aspx?rh1=" + lbrh.Text + "&pac1=" + lbpac.Text + "&exm1=" + lbexm.Text + "&fila1=" + lbfila.Text + "&solic1=" + lbsol.Text + "', null, 'height=400,width=800,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
    }
    protected void btnInterno_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(500/2);var Mtop = (screen.height/2)-(500/2);window.open( '../Exames/cadDataExame.aspx?rh=" + lbrh.Text + "&pac=" + lbpac.Text + "&exm=" + lbexm.Text + "&fila=" + lbfila.Text + "&sol=" + lbsol.Text + "', null, 'height=400,width=800,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Fechar", "window.close()", true);
    }
    
}
