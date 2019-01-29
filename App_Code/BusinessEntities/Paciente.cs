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

namespace CD.DAL.BusinessEntities
{
    public class Paciente
    {
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

        int rf;
        public int Rf
        {
            get { return rf; }
            set { rf = value; }
        }

    }
}
