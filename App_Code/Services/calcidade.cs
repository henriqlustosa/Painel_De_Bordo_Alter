using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class calcidade
{
    /*************************************************/
    /* cálculo da idade da pessoa                    */
    /*************************************************/
    public calcidade()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static int getIdade(String data)
    {
        
        DateTime dt = DateTime.Now;
        int ano = Convert.ToInt32(data.Substring(0, 4));
        int mes = Convert.ToInt32(data.Substring(4, 2));
        int dia = Convert.ToInt32(data.Substring(6));
        DateTime dtnasc = new DateTime(ano, mes, dia);
        int dif = dt.Year - dtnasc.Year;
        if (dtnasc.AddYears(dif) > dt)
            dif--;
        return dif;
    }

    public static String DataFormatada(string dt)
    {
        string data = dt;
        int ano = Convert.ToInt32(data.Substring(0, 4));
        int mes = Convert.ToInt32(data.Substring(4, 2));
        int dia = Convert.ToInt32(data.Substring(6, 2));
        data = dia + "/" + mes + "/" + ano;

        return data;
    }
}
