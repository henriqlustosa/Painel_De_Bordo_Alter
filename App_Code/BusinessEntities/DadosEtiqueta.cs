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
/// Summary description for DadosEtiqueta
/// </summary>
/// 
[Serializable]
public class DadosEtiqueta
{
	public DadosEtiqueta()
	{
       
	}

    int codtipid;

    public int CodTipId
    {
        get { return codtipid; }
        set { codtipid = value; }
    }


    int numped;

    public int Numped
    {
        get { return numped; }
        set { numped = value; }
    }
    int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    String nome;

    public String Nome
    {
        get { return nome; }
        set { nome = value; }
    }
    String procedencia;

    public String Procedencia
    {
        get { return procedencia; }
        set { procedencia = value; }
    }
    DateTime dt;

    public DateTime Dt
    {
        get { return dt; }
        set { dt = value; }
    }

    String tipExm;

    public String TipExm
    {
        get { return tipExm; }
        set { tipExm = value; }
    }

    int codExm;

    public int CodExm
    {
        get { return codExm; }
        set { codExm = value; }
    }


}
