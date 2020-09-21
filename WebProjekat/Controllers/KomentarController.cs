using Newtonsoft.Json;
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
    [Route("")]
    public class KomentarController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpPost]
        [Route("komentari")]
        public IHttpActionResult KreirajKomentar(KomentarDTO kdto)
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

            //BazaPodataka bp = new BazaPodataka();
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            Guid guid = Guid.NewGuid();
            string strId = guid.ToString();

            Komentar k = new Komentar(strId, korisnikSesija.Id, kdto.ManifestacijaID, kdto.Tekst, kdto.Ocena.ToString(), kdto.Status.ToString(), kdto.IsDeleted.ToString());

            bp.listaKomentara.Add(k.Id, k);
            k.SacuvajKomentar();

            return Ok();
        }

        [HttpGet]
        [Route("komentari")]
        public IHttpActionResult PribaviSveKomentare()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            List<SviKomentariDTO> pomocnaLista = new List<SviKomentariDTO>();

            if(korisnikSesija.Uloga == Enums.Uloga.ADMINISTRATOR)
            {
                foreach (var komentar in bp.listaKomentara.Values)
                {
                    if(komentar.Status != Enums.StatusKomentara.NACEKANJU && !komentar.IsDeleted)
                        pomocnaLista.Add(new SviKomentariDTO(komentar.Id, bp.listaKorisnika[komentar.KupacID].KorisnickoIme, bp.listaManifestacija[komentar.ManifestacijaID].Naziv, komentar.Tekst, komentar.Ocena, komentar.Status));
                }
            }
            else if (korisnikSesija.Uloga == Enums.Uloga.PRODAVAC)
            {
                Prodavac p = (Prodavac)korisnikSesija;
                foreach (var komentar in bp.listaKomentara.Values)
                {
                    foreach (var manif in p.SveMojeManifestacije)
                    {
                        if(komentar.ManifestacijaID == manif && !komentar.IsDeleted)
                            pomocnaLista.Add(new SviKomentariDTO(komentar.Id, bp.listaKorisnika[komentar.KupacID].KorisnickoIme, bp.listaManifestacija[komentar.ManifestacijaID].Naziv, komentar.Tekst, komentar.Ocena, komentar.Status));
                    }
                }
            }

            string json = JsonConvert.SerializeObject(pomocnaLista);
            return Ok(json);
        }

        [HttpGet]
        [Route("komentari-jedne-manif")]
        public IHttpActionResult PribaviKomentareJedneManifestacije(string manifestacijaId)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            List<SviKomentariDTO> pomocnaLista = new List<SviKomentariDTO>();

            foreach (var komentar in bp.listaKomentara.Values)
            {
                if(komentar.ManifestacijaID == manifestacijaId && komentar.Status == Enums.StatusKomentara.PRIHVACEN && !komentar.IsDeleted)
                {
                    pomocnaLista.Add(new SviKomentariDTO(komentar.Id, bp.listaKorisnika[komentar.KupacID].KorisnickoIme, bp.listaManifestacija[komentar.ManifestacijaID].Naziv, komentar.Tekst, komentar.Ocena, komentar.Status));
                }
            }

            string json = JsonConvert.SerializeObject(pomocnaLista);
            return Ok(json);
        }

        [HttpGet]
        [Route("odobri-komentar")]
        public IHttpActionResult OdobriKomentar(string id)
        {
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            bp.listaKomentara[id].Status = Enums.StatusKomentara.PRIHVACEN;
            bp.AzurirajKomentare();

            return Ok();
        }

        [HttpGet]
        [Route("odbij-komentar")]
        public IHttpActionResult OdbijKomentar(string id)
        {
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            bp.listaKomentara[id].Status = Enums.StatusKomentara.ODBIJEN;
            bp.AzurirajKomentare();

            return Ok();
        }

        [HttpDelete]
        [Route("obrisi-komentar")]
        public IHttpActionResult ObrisiKomentar(string id)
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            bp.listaKarata = (Dictionary<string, Karta>)HttpContext.Current.Application["Karte"];
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            if (korisnikSesija.Uloga == Enums.Uloga.ADMINISTRATOR)
            {
                bp.listaKomentara[id].IsDeleted = bool.Parse("True");
                bp.AzurirajKomentare();
            }

            return Ok();
        }
    }
}
