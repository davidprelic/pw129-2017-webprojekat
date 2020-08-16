using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebProjekat.Models;
using WebProjekat.Models.DTOs;

namespace WebProjekat.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(Kupac k)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //BazaPodataka bp = new BazaPodataka();
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];

            TipKorisnika tk = new TipKorisnika();
            k.TipKorisn = tk;

            bp.listaKorisnika.Add(k.KorisnickoIme, k);
            k.SacuvajKorisnika();

            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(LoginDTO logDto)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            if (bp.listaKorisnika.ContainsKey(logDto.KorisnickoIme))
            {
                if (bp.listaKorisnika[logDto.KorisnickoIme].Lozinka != logDto.Lozinka)
                {
                    //        // Vrati poruku greske da lozinka nije ispravna
                    return BadRequest();
                }


                korisnikSesija = bp.listaKorisnika[logDto.KorisnickoIme];
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;

                //    // Redirektuj korisnika na home page zbog uspesne prijave 
                return Ok();
            }

            // Vrati poruku greske da korisnik sa datim username-om ne postoji u bazi
            return BadRequest();
        }


        [HttpGet]
        [Route("Logout")]
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
