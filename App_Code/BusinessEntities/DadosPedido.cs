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
/// Summary description for DadosPedido
/// </summary>
/// 
[Serializable]
public class DadosPedido
{
	public DadosPedido()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    int numpedido;

    public int Numpedido
    {
        get { return numpedido; }
        set { numpedido = value; }
    }


    String exame;

    public String Exame
    {
        get { return exame; }
        set { exame = value; }
    }


    int cod_status;

    public int Cod_status
    {
        get { return cod_status; }
        set { cod_status = value; }
    }

    int tip_id;
    public int Tip_id
    {
        get { return tip_id; }
        set { tip_id = value; }
    }

    String nome;
    public String Nome
    {
        get { return nome; }
        set { nome = value; }
    }

    int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    String cpf_med;
    public String Cpf_med
    {
        get { return cpf_med; }
        set { cpf_med = value; }
    }

    int setor_tip;
    public int Tipo_Setor
    {
        get { return setor_tip; }
        set { setor_tip = value; }
    }

    int exm_id;
    public int Exm_id
    {
        get { return exm_id; }
        set { exm_id = value; }
    }

    int cod_setor;
    public int Cod_setor
    {
        get { return cod_setor; }
        set { cod_setor = value; }
    }

    DateTime dt;

    public DateTime Dt
    {
        get { return dt; }
        set { dt = value; }
    }


}
