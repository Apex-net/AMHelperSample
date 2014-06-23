using System;
using System.Collections.Generic;
using System.Text;
using AMHelper.WS;


namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ExpOrders();
        }



        static void ExpOrders()
        {

            try
            {
                // Chiavi
                string AuthKeyLM = "3405D863-C49C-4D4B-B1FF-35D6231C61D9";
                string AuthKeyAM = "AAB993AE-92B7-4E88-BC59-B231F0CDAD7C";


                // Dove è situato il mio AM ?
                GetDataLM lmdata = new GetDataLM(AuthKeyLM);

                // Quali dati contiene il mio AM ?
                ws_rec_lmparam AMData = null;
                bool lmRetVal = lmdata.get_am_par(ref AMData);


                string wsURL = "";
                if (lmRetVal && AMData != null)
                {
                    wsURL = AMData.url_am_api + "/" + AMData.cod_prog;
                }
                else
                {
                    Console.WriteLine("Qualcosa è andato male");
                    return;
                }



                // Leggo l'ID dell'ultimo ordine recuperato dal WS. Se è la prima volta tornerà 0 (zero)

                int LastStoredID = 30;
                //int LastStoredID = 12484;
                // Istanzio l'oggetto Export dell'AMHelper
                GetDataAM ed = new GetDataAM(AuthKeyAM, wsURL);

                // Chiamo il WS per farmi dare la lista dei leads
                ws_rec_orders OrdersData = null;
                bool RetVal = ed.exp_orders(LastStoredID, ref OrdersData);

                if (RetVal && OrdersData != null)
                {
                    Console.WriteLine(String.Format("... found {0} record ", OrdersData.testate.Count.ToString()));

                    System.Globalization.CultureInfo itIT = System.Globalization.CultureInfo.CreateSpecificCulture("it-IT");

                    foreach (var t in OrdersData.testate)
                    {
                        Console.WriteLine("Codclifor:" + t.cod_clifor);
                        Console.WriteLine("Data consegna:" + t.data_consegna);

                        foreach (var r in t.righe)
                        {
                            Console.WriteLine(">>> Riga: " + r.id);
                            Console.WriteLine(">>> Cod art: " + r.codice_articolo);
                            // .... etc..
                        }
                    }
                }
                else
                {
                    Console.WriteLine("... booh! ");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("..error:" + ex.Message);
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

        }

    }
}
