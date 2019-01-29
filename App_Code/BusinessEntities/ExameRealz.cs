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
/// Summary description for ExameRealz
/// </summary>
/// 
[Serializable]
public class ExameRealz
{
	public ExameRealz()
	{		
	}

    int cod_exame;

    public int Cod_exame
    {
        get { return cod_exame; }
        set { cod_exame = value; }
    }
    String descr;

    public String Descr
    {
        get { return descr; }
        set { descr = value; }
    }
}
