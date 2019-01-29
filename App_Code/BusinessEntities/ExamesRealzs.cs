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
/// Summary description for ExamesRealzs
/// </summary>
/// 
[Serializable]
public class ExamesRealzs
{
	public ExamesRealzs()
	{
        items = new Dictionary<int,ExameRealz>();
	}

    Dictionary<int, ExameRealz> items;

    public Dictionary<int, ExameRealz> Items
    {
        get { return items; }
        set { items = value; }
    } 
  
}
