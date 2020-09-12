using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpGet]
        [Route("korisnici")]
        public IHttpActionResult PribaviKorisnike()
        {
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            string json = JsonConvert.SerializeObject(bp.listaKorisnika.Values.ToList());
            return Ok(json);
        }

        [HttpPost]
        [Route("kreirajprodavca")]
        public IHttpActionResult KreirajProdavca(Prodavac p)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];

            bp.listaKorisnika.Add(p.KorisnickoIme, p);
            p.SacuvajKorisnika();

            return Ok();
        }
    }
}
