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
/// Summary description for PedidoDetail
/// </summary>
public class PedidoDetail
{
	public PedidoDetail()
	{		
	}


    int numped;

    public int Numped
    {
        get { return numped; }
        set { numped = value; }
    }

    int id_pac;

    public int Id_pac
    {
        get { return id_pac; }
        set { id_pac = value; }
    }

    String tipo_id;

    public String Tipo_id
    {
        get { return tipo_id; }
        set { tipo_id = value; }
    }

    String paciente;

    public String Paciente
    {
        get { return paciente; }
        set { paciente = value; }
    }

    String solicitante;

    public String Solicitante
    {
        get { return solicitante; }
        set { solicitante = value; }
    }

    String procedencia;

    public String Procedencia
    {
        get { return procedencia; }
        set { procedencia = value; }
    }

    String status;

    public String Status
    {
        get { return status; }
        set { status = value; }
    }

    String setor;

    public String Setor
    {
        get { return setor; }
        set { setor = value; }
    }

   String exame;

   public String Exame
   {
       get { return exame; }
       set { exame = value; }
   }

   DateTime data;

   public DateTime Data
   {
       get { return data; }
       set { data = value; }
   }

}
