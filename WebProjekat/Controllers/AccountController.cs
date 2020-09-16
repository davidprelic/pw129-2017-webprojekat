using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebProjekat.Models;
using WebProjekat.Models.DTOs;

namespace WebProjekat.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginDTO logDto)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];

            Korisnik k = null;

            List<Korisnik> korisnici = bp.listaKorisnika.Values.ToList();


            k = korisnici.FirstOrDefault(cus => cus.KorisnickoIme == logDto.KorisnickoIme);
            if (k != null)
            {
                if(k.Lozinka != logDto.Lozinka)
                {
                    // Vrati poruku greske da lozinka nije ispravna
                    return BadRequest();
                }

                korisnikSesija = bp.listaKorisnika[k.Id];
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;

                // Redirektuj korisnika na home page zbog uspesne prijave 
                return Ok();
            }

            // Vrati poruku greske da korisnik sa datim username-om ne postoji u bazi
            return BadRequest();
        }

        [HttpGet]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija != null)
            {
                HttpContext.Current.Session.Abandon();
            }

            return Ok();
        }
        
    }
}
