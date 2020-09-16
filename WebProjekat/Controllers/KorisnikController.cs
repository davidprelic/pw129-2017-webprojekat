using Newtonsoft.Json;
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
    [RoutePrefix("")]
    public class KorisnikController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpPost]
        [Route("korisnik")]
        public IHttpActionResult Register(KupacRegisterDTO kdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //BazaPodataka bp = new BazaPodataka();
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];


            Guid guid = Guid.NewGuid();
            string strId = guid.ToString();

            DateTime dt = DateTime.ParseExact(kdto.DatumRodjenja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            Kupac k = new Kupac(strId, kdto.KorisnickoIme, kdto.Lozinka, kdto.Ime, kdto.Prezime, kdto.Pol.ToString(), datumString, kdto.Uloga.ToString(), "", kdto.BrojSakupljenihBodova.ToString(), "", kdto.IsDeleted.ToString());
            TipKorisnika tk = new TipKorisnika();
            k.TipKorisn = tk;

            bp.listaKorisnika.Add(k.Id, k);
            k.SacuvajKorisnika();

            return Ok();
        }

        [HttpGet]
        [Route("korisnik")]
        public IHttpActionResult PrikaziProfil()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            KorisnikProfilDTO kpdto = new KorisnikProfilDTO(korisnikSesija.Id, korisnikSesija.KorisnickoIme, korisnikSesija.Ime, korisnikSesija.Prezime, korisnikSesija.Pol, korisnikSesija.DatumRodjenja, korisnikSesija.Uloga, korisnikSesija.IsDeleted);

            return Ok(kpdto);
        }

        [HttpGet]
        [Route("korisnici")]
        public IHttpActionResult PribaviKorisnike()
        {
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            string json = JsonConvert.SerializeObject(bp.listaKorisnika.Values.ToList());
            return Ok(json);
        }

        [HttpPut]
        [Route("korisnik")]
        public IHttpActionResult IzmeniProfil(KorisnikProfilDTO kpdto)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            if (bp.listaKorisnika.ContainsKey(kpdto.Id))
            {
                bp.listaKorisnika[kpdto.Id].KorisnickoIme = kpdto.KorisnickoIme;
                bp.listaKorisnika[kpdto.Id].Ime = kpdto.Ime;
                bp.listaKorisnika[kpdto.Id].Prezime = kpdto.Prezime;
                bp.AzurirajKorisnike();
                return Ok();
            }

            return BadRequest();
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

            Guid guid = Guid.NewGuid();
            string strId = guid.ToString();

            p.Id = strId;

            bp.listaKorisnika.Add(p.Id, p);
            p.SacuvajKorisnika();

            return Ok();
        }

        [HttpGet]
        [Route("provera-popusta")]
        public IHttpActionResult ProveriPopustZaKorisnika()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            Kupac k = (Kupac)korisnikSesija;

            KupacIdPopustDTO kid = new KupacIdPopustDTO(k.Id, k.TipKorisn.Popust);

            return Ok(kid);
        }

    }
}
