using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using WebProjekat.Models;

namespace WebProjekat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // globalno isključivanje xml formatera
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            BazaPodataka bPodataka = new BazaPodataka();

            bPodataka.UcitajKorisnike("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["Korisnici"] = bPodataka.listaKorisnika;

            bPodataka.UcitajManifestacije("~/App_Data/manifestacije.txt");
            HttpContext.Current.Application["Manifestacije"] = bPodataka.listaManifestacija;

            bPodataka.UcitajKarte("~/App_Data/karte.txt");
            HttpContext.Current.Application["Karte"] = bPodataka.listaKarata;

            bPodataka.UcitajKomentare("~/App_Data/komentari.txt");
            HttpContext.Current.Application["Komentari"] = bPodataka.listaKomentara;

            bPodataka.UcitajLokacije("~/App_Data/lokacije.txt");
            HttpContext.Current.Application["Lokacije"] = bPodataka.listaLokacija;
        }

        // Ukljucivanje podrske za Session
        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }
}
