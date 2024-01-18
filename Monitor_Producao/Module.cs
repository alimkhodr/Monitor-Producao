using System;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;


namespace Monitor_Producao
{
    public class Module : IHttpModule
    {
        public static DateTime Ndata;
        public static DateTime DataAtual;
        public static string ct, maquina;
        public static int tgtFCST, tgtAci, tgtAbs, tgtFTQ, tgtScrap, tgtValorScrap, tgtRec, tgtPecas, tgtEfi, tgtHG, tgtMOV, tgtProd, tgtParada;
        public void Dispose()
        {
            // Clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        public void OnLogRequest(Object source, EventArgs e)
        {
            // Custom logging logic can go here.
        }
        public static string Modal()
        {
            return @"
                <script>
                    function exibirModal() {
                        var modal = document.querySelector('.modal');
                        if (modal) {
                            modal.style.display = 'block';
                        }
                    }
                    exibirModal();
                </script>
            ";
        }



    }
}