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
/// Summary description for Proffs
/// </summary>
/// 
[Serializable]
public class Prof
{
	public Prof()
	{
	}

    int cod;
    public int Cod
    {
        get { return cod; }
        set { cod = value; }
    }


    String nome;
    public String Nome
    {
        get { return nome; }
        set { nome = value; }
    }

}
