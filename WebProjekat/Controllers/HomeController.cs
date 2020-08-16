using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public RedirectResult Index()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            var requestUri = Request.RequestUri;
            return Redirect(requestUri.AbsoluteUri + "Htmls/index.html");
        }

        [HttpGet]
        [Route("sesija")]
        public IHttpActionResult ProveriUloguIzSesije()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            return Ok(korisnikSesija.Uloga.ToString());
        }

    }
}
