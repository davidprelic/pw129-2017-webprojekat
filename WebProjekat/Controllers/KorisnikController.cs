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

            if (!k.IsDeleted)
            {
                KupacProfilDTO kupacdto = new KupacProfilDTO(k.Id, k.KorisnickoIme, k.Ime, k.Prezime, k.Pol, k.DatumRodjenja, k.BrojSakupljenihBodova, k.TipKorisn, korisnikSesija.Uloga, korisnikSesija.IsDeleted);
                return Ok(kupacdto);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("korisnici")]
        public IHttpActionResult PribaviProdavceAdmine()
        {
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            List<Korisnik> adminProdavac = new List<Korisnik>();
            foreach (var item in bp.listaKorisnika.Values)
            {
                if ((item.Uloga == Enums.Uloga.ADMINISTRATOR || item.Uloga == Enums.Uloga.PRODAVAC) && !item.IsDeleted)
                    adminProdavac.Add(item);
            }
            string json = JsonConvert.SerializeObject(adminProdavac);
            return Ok(json);
        }

        [HttpGet]
        [Route("korisnici-kupci")]
        public IHttpActionResult PribaviKupce()
        {
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            List<Kupac> kupci = new List<Kupac>();
            foreach (var item in bp.listaKorisnika.Values)
            {
                if (item.Uloga == Enums.Uloga.KUPAC && !item.IsDeleted)
                    kupci.Add((Kupac)item);
            }
            string json = JsonConvert.SerializeObject(kupci);
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            if (bp.listaKorisnika.ContainsKey(kpdto.Id))
            {
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
                if (bp.listaKorisnika[key].Ime.Contains(imePar) && bp.listaKorisnika[key].Prezime.Contains(prezimePar) && bp.listaKorisnika[key].KorisnickoIme.Contains(korImePar) && !bp.listaKorisnika[key].IsDeleted)
                {
                    filter.Add(bp.listaKorisnika[key]);
                }
            }

            Kupac k;
            List<Kupac> kupci = new List<Kupac>();

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
            else if (opcijaSort == "poBrojuBodovaRastuce")
            {
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.KUPAC)
                        kupci.Add((Kupac)item);
                }
                kupci = kupci.OrderBy(p => p.BrojSakupljenihBodova).ToList();
            }
            else if (opcijaSort == "poBrojuBodovaOpadajuce")
            {
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.KUPAC)
                        kupci.Add((Kupac)item);
                }
                kupci = kupci.OrderByDescending(p => p.BrojSakupljenihBodova).ToList();
            }
            // DODATI JOS ZA FILTRIRANJE 
            List<Korisnik> filterOpcija = new List<Korisnik>();
            

            if (opcijaFilter == "admin")
            {
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.ADMINISTRATOR)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "kupac")
            {
                if (kupci.Count > 0)
                    return Ok(kupci);

                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.KUPAC)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "prodavac")
            {
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.PRODAVAC)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "bronzani")
            {
                if (kupci.Count > 0)
                {
                    List<Kupac> kupacFinal = new List<Kupac>();
                    foreach (var item in kupci)
                    {
                        if (item.TipKorisn.ImeTipa == Enums.TipKorisnika.BRONZANI)
                            kupacFinal.Add(item);
                    }
                    return Ok(kupacFinal);
                }

                List<Kupac> kupacFinal2 = new List<Kupac>();
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.KUPAC)
                    {
                        k = (Kupac)item;
                        if(k.TipKorisn.ImeTipa == Enums.TipKorisnika.BRONZANI)
                            kupacFinal2.Add(k);
                    }
                }
                return Ok(kupacFinal2);
            }
            else if (opcijaFilter == "srebrni")
            {
                if (kupci.Count > 0)
                {
                    List<Kupac> kupacFinal = new List<Kupac>();
                    foreach (var item in kupci)
                    {
                        if (item.TipKorisn.ImeTipa == Enums.TipKorisnika.SREBRNI)
                            kupacFinal.Add(item);
                    }
                    return Ok(kupacFinal);
                }

                List<Kupac> kupacFinal2 = new List<Kupac>();
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.KUPAC)
                    {
                        k = (Kupac)item;
                        if (k.TipKorisn.ImeTipa == Enums.TipKorisnika.SREBRNI)
                            kupacFinal2.Add(k);
                    }
                }
                return Ok(kupacFinal2);
            }
            else if (opcijaFilter == "zlatni")
            {
                if (kupci.Count > 0)
                {
                    List<Kupac> kupacFinal = new List<Kupac>();
                    foreach (var item in kupci)
                    {
                        if (item.TipKorisn.ImeTipa == Enums.TipKorisnika.ZLATNI)
                            kupacFinal.Add(item);
                    }
                    return Ok(kupacFinal);
                }

                List<Kupac> kupacFinal2 = new List<Kupac>();
                foreach (var item in filter)
                {
                    if (item.Uloga == Enums.Uloga.KUPAC)
                    {
                        k = (Kupac)item;
                        if (k.TipKorisn.ImeTipa == Enums.TipKorisnika.ZLATNI)
                            kupacFinal2.Add(k);
                    }
                }
                return Ok(kupacFinal2);
            }

            if (kupci.Count > 0)
                return Ok(kupci);

            return Ok(filter);
        }

        [HttpDelete]
        [Route("obrisi-korisnika")]
        public IHttpActionResult ObrisiKorisnika(string id)
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            bp.listaKarata = (Dictionary<string, Karta>)HttpContext.Current.Application["Karte"];

            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            if (korisnikSesija.Uloga == Enums.Uloga.ADMINISTRATOR)
            {
                bp.listaKorisnika[id].IsDeleted = bool.Parse("True");

                if(bp.listaKorisnika[id].Uloga == Enums.Uloga.KUPAC)
                {
                    Kupac k = (Kupac)bp.listaKorisnika[id];
                    foreach (var item in k.SveMojeKarteBezObziraNaStatus)
                    {
                        bp.listaKarata[item].IsDeleted = bool.Parse("True");
                    }
                    bp.AzurirajKarte();
                }
                else if(bp.listaKorisnika[id].Uloga == Enums.Uloga.PRODAVAC)
                {
                    Prodavac p = (Prodavac)bp.listaKorisnika[id];

                    foreach (var item in p.SveMojeManifestacije)
                    {
                        bp.listaManifestacija[item].IsDeleted = bool.Parse("True");
                    }
                    bp.AzurirajManifestacije();
                }
                
                bp.AzurirajKorisnike();
            }

            return Ok();
        }

    }
}
