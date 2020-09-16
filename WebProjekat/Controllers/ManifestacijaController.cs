using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using WebProjekat.Models;
using WebProjekat.Models.DTOs;

namespace WebProjekat.Controllers
{
    [RoutePrefix("")]
    public class ManifestacijaController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpPost]
        [Route("manifestacija")]
        public IHttpActionResult KreirajManifestaciju(ManifestacijaDTO mdto)
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
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            Guid guid = Guid.NewGuid();
            string strId = guid.ToString();

            DateTime dt = DateTime.ParseExact(mdto.DatumVremeOdrzavanja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            Manifestacija m = new Manifestacija(strId, mdto.Naziv, mdto.Tip.ToString(), mdto.BrojMesta.ToString(), mdto.BrojRegularKarata.ToString(), mdto.BrojVipKarata.ToString(), mdto.BrojFanpitKarata.ToString(), datumString, mdto.CenaRegularKarte.ToString(), mdto.Status.ToString(), mdto.MestoOdrzavanjaID, mdto.PosterManifestacije, mdto.IsDeleted.ToString());

            bp.listaManifestacija.Add(m.Id, m);
            m.SacuvajManifestaciju();

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            Prodavac p = null;
            p = (Prodavac)bp.listaKorisnika[korisnikSesija.Id];
            if(p.SveMojeManifestacije.Contains(""))
                p.SveMojeManifestacije.Remove("");

            p.SveMojeManifestacije.Add(m.Id);
            korisnikSesija = p;
            bp.AzurirajKorisnike();

            return Ok();
        }

        [HttpGet]
        [Route("manifestacije")]
        public IHttpActionResult ManifestacijePocetna()
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            List<Manifestacija> pomocnaLista = new List<Manifestacija>();

            foreach(var item in bp.listaManifestacija.Values)
            {
                if (item.Status == Enums.StatusManifestacije.AKTIVNO)
                    pomocnaLista.Add(item);
            }

            string json = JsonConvert.SerializeObject(pomocnaLista);
            return Ok(json);
        }

        [HttpGet]
        [Route("manifestacije/{id}")]
        public IHttpActionResult PrikazManifestacije(string id)
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            if (bp.listaManifestacija.ContainsKey(id))
            {
                return Ok(bp.listaManifestacija[id]);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("manifestacijeprodavca")]
        public IHttpActionResult ManifestacijeProdavca()
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];

            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            Prodavac p = null;

            foreach (string key in bp.listaKorisnika.Keys)
            {
                if (key == korisnikSesija.Id)
                {
                    p = (Prodavac)bp.listaKorisnika[key];
                    break;
                }
            }

            if (p.SveMojeManifestacije.Contains(""))
                p.SveMojeManifestacije.Remove("");

            List<Manifestacija> pomLista = new List<Manifestacija>();

            foreach (string item in p.SveMojeManifestacije)
            {
                pomLista.Add(bp.listaManifestacija[item]);
            }
            
            return Ok(pomLista);
        }

        [HttpGet]
        [Route("prikaz-manifestacije-potvrda")]
        public IHttpActionResult PrikazManifestacijePotvrda()
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            List<Manifestacija> pomocnaLista = new List<Manifestacija>();

            foreach (var item in bp.listaManifestacija.Values)
            {
                if (item.Status == Enums.StatusManifestacije.NEAKTIVNO)
                    pomocnaLista.Add(item);
            }
            
            return Ok(pomocnaLista);
        }

        [HttpGet]
        [Route("manifestacije-potvrda")]
        public IHttpActionResult PotvrdaManifestacije(string id)
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            bp.listaManifestacija[id].Status = Enums.StatusManifestacije.AKTIVNO;
            bp.AzurirajManifestacije();

            return Ok();
        }

    }
}
