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
/// Summary description for Medico
/// </summary>
public class Medico
{
	public Medico()
	{
       
	}

    String nome;

    public String Nome
    {
        get { return nome; }
        set { nome = value; }
    }

    long cpf;

    public long Cpf
    {
        get { return cpf; }
        set { cpf = value; }
    }

}
