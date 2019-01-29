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
/// Summary description for MateriaisUtils
/// </summary>
/// 
[Serializable]
public class MateriaisUtils
{
	public MateriaisUtils()
	{
        items = new Dictionary<int, MaterialUtil>();
	}

    Dictionary<int, MaterialUtil> items;

    public Dictionary<int, MaterialUtil> Items
    {
        get { return items; }
        set { items = value; }
    }
}

