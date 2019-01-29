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
/// Summary description for SQLInsert
/// </summary>
public class SQLInsert
{
	public SQLInsert()
	{
	}

    int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    String erro;

    public String Erro
    {
        get { return erro; }
        set { erro = value; }
    }
    bool success;

    public bool Success
    {
        get { return success; }
        set { success = value; }
    }


}
