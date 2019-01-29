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

/// <summary>
/// Summary description for TipoPedido
/// </summary>
public class Tipos
{
	public Tipos()
	{
	}

    public enum Pedido
    {
        Agendado=1,Extra=2
    }

    public enum Status
    { 
        Cadastrado=1,Checkin=2,Realizado=3
    }

}
