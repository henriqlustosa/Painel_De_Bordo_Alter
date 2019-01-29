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
/// Summary description for MaterialUtil
/// </summary>
/// 
[Serializable]
public class MaterialUtil
{
	public MaterialUtil()
	{		
	}

    int cod;

    public int Cod
    {
        get { return cod; }
        set { cod = value; }
    }
    int qtd;

    public int Qtd
    {
        get { return qtd; }
        set { qtd = value; }
    }
    String descr;

    public String Descr
    {
        get { return descr; }
        set { descr = value; }
    }


}
