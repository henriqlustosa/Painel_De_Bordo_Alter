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
using System.Collections.Generic;

/// <summary>
/// Summary description for PedidosDetails
/// </summary>
public class PedidosDetails
{
	public PedidosDetails()
	{
        items = new List<PedidoDetail>();
	}

    List<PedidoDetail> items;

    public List<PedidoDetail> Items
    {
        get { return items; }
        set { items = value; }
    }
}