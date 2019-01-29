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
using System.Text;

/// <summary>
/// Summary description for ImprimirEtiqueta
/// </summary>
public class ImprimirEtiqueta
{
    public ImprimirEtiqueta()
    {
    }

    public static void Imprimir(int _numped, int _count, String _host)
    {
        /*    DadosEtiqueta etiq = SqlSv.getDadosEtiqueta(_numped);
       
            System.IntPtr lhPrinter = new System.IntPtr();
            DOCINFO di = new DOCINFO();
            int pcWritten = 0;
            string st1;
            // text to print with a form feed character
      
            di.pDocName = "my test document";
            di.pDataType = "RAW";
       
            //lhPrinter contains the handle for the printer opened
            //If lhPrinter is 0 then an error has occured
         //   PrintDirect.OpenPrinter("\\\\"+_host+"\\Etiq", ref lhPrinter, 0);
            PrintDirect.OpenPrinter("\\\\hspmcac005\\Etiq", ref lhPrinter, 0);
            PrintDirect.StartDocPrinter(lhPrinter, 1, ref di);
            PrintDirect.StartPagePrinter(lhPrinter);
         //   for (int i = 0; i < _count; i++)
            {
                try
                {
                    st1 = "HSPM - CENTRO DIAGNOSTICO\r\n\n";
                    PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, ref pcWritten);

                    st1 = "PEDIDO: " + etiq.Numped.ToString() + "\r\n";
                    PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, ref pcWritten);
                    if (etiq.CodTipId == 1)
                        st1 = "RH: " + etiq.Id.ToString() + "\r\n";
                    else
                        st1 = "BE: " + etiq.Id.ToString() + "\r\n";
                    PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, ref pcWritten);

                    st1 = "NOME: " + etiq.Nome + "\r\n";
                    PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, ref pcWritten);

                    st1 = "DATA: " + etiq.Dt.ToShortDateString() + "\r\n";
                    PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, ref pcWritten);

                    st1 = etiq.Procedencia + "\n\n\n\n";
                    PrintDirect.WritePrinter(lhPrinter, st1, st1.Length, ref pcWritten);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            PrintDirect.EndPagePrinter(lhPrinter);
            PrintDirect.EndDocPrinter(lhPrinter);
            PrintDirect.ClosePrinter(lhPrinter);
        }*/
        DadosEtiqueta etiq = SqlSv.getDadosEtiqueta(_numped);
       
        StringBuilder st1 = new StringBuilder();
        for (int i = 0; i < _count; i++)
        {
            st1.Append("   HSPM - CENTRO DIAGNOSTICO\r\n\n");


            st1.Append("PEDIDO: " + etiq.Numped.ToString() + "      ");

            if (etiq.CodTipId == 1)
                st1.Append("RH: " + etiq.Id.ToString() + "\r\n");
            else
                st1.Append("BE: " + etiq.Id.ToString() + "\r\n");

            if (etiq.Nome.Length > 31)
              etiq.Nome = etiq.Nome.Substring(0, 24);
            st1.Append("NOME: " + etiq.Nome + "\r\n");


            st1.Append("DATA: " + etiq.Dt.ToShortDateString() + "\r\n");

            st1.Append("EXAME: " + etiq.TipExm + "\r\n");
            if (etiq.Procedencia.Length > 31)
            etiq.Procedencia = etiq.Procedencia.Substring(0, 30);
            st1.Append(etiq.Procedencia + "\n\n\n");
        }

        RawPrinterHelper.SendStringToPrinter("\\\\"+_host+"\\etiq", st1.ToString());

    }

}