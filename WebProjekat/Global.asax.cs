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

        }

        // Ukljucivanje podrske za Session
        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }
}
