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

            if(korisnikSesija.Uloga == Enums.Uloga.KUPAC)
            {
                Kupac k = (Kupac)korisnikSesija;
                KupacProfilDTO kupacdto = new KupacProfilDTO(korisnikSesija.Id, korisnikSesija.KorisnickoIme, korisnikSesija.Ime, korisnikSesija.Prezime, korisnikSesija.Pol, korisnikSesija.DatumRodjenja, k.BrojSakupljenihBodova, k.TipKorisn ,korisnikSesija.Uloga, korisnikSesija.IsDeleted);
                return Ok(kupacdto);
            }
            
            KorisnikProfilDTO kpdto = new KorisnikProfilDTO(korisnikSesija.Id, korisnikSesija.KorisnickoIme, korisnikSesija.Ime, korisnikSesija.Prezime, korisnikSesija.Pol, korisnikSesija.DatumRodjenja);

            return Ok(kpdto);
        }

        [HttpGet]
        [Route("korisnik/{id}")]
        public IHttpActionResult PrikaziKupcaKarte(string id)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            Kupac k = (Kupac)bp.listaKorisnika[id];
            
            KupacProfilDTO kupacdto = new KupacProfilDTO(k.Id, k.KorisnickoIme, k.Ime, k.Prezime, k.Pol, k.DatumRodjenja, k.BrojSakupljenihBodova, k.TipKorisn, korisnikSesija.Uloga, korisnikSesija.IsDeleted);
            return Ok(kupacdto);
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
                bp.listaKorisnika[kpdto.Id].Pol = kpdto.Pol;
                bp.listaKorisnika[kpdto.Id].DatumRodjenja = kpdto.DatumRodjenja;
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

        [HttpGet]
        [Route("filter-korisnici")]
        public IHttpActionResult PretragaSortFilter(string ime, string prezime, string korisnickoIme, string opcijaSort, string opcijaFilter)
        {
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];

            List<Korisnik> filter = new List<Korisnik>();

            foreach (string key in bp.listaKorisnika.Keys)
            {
                string imePar = (ime == null) ? "" : ime;
                string prezimePar = (prezime == null) ? "" : prezime;
                string korImePar = (korisnickoIme == null) ? "" : korisnickoIme;

                //if (proizvodi.list[key].RaspolozivaKolicina != 0 && proizvodi.list[key].Naziv.Equals(naziv))
                if (bp.listaKorisnika[key].Ime.Contains(imePar) && bp.listaKorisnika[key].Prezime.Contains(prezimePar) && bp.listaKorisnika[key].KorisnickoIme.Contains(korImePar))
                {
                    filter.Add(bp.listaKorisnika[key]);
                }
            }

            if (opcijaSort == "poImenuAZ")
                filter = filter.OrderBy(p => p.Ime).ToList();
            else if (opcijaSort == "poImenuZA")
                filter = filter.OrderByDescending(p => p.Ime).ToList();
            else if (opcijaSort == "poPrezimenuAZ")
                filter = filter.OrderBy(p => p.Prezime).ToList();
            else if (opcijaSort == "poPrezimenuZA")
                filter = filter.OrderByDescending(p => p.Prezime).ToList();
            else if (opcijaSort == "poKorImenuAZ")
                filter = filter.OrderBy(p => p.KorisnickoIme).ToList();
            else if (opcijaSort == "poKorImenuZA")
                filter = filter.OrderByDescending(p => p.KorisnickoIme).ToList();

            // DODATI JOS ZA FILTRIRANJE 

            return Ok(filter);
        }


    }
}
