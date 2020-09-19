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
    [Route("")]
    public class KartaController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpPost]
        [Route("karte")]
        public IHttpActionResult RezervisiKarte(GenerisanjeKarataDTO gdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //BazaPodataka bp = new BazaPodataka();
            bp.listaKarata = (Dictionary<string, Karta>)HttpContext.Current.Application["Karte"];

            Guid guid = Guid.NewGuid();
            string idKarte;

            DateTime dt = DateTime.ParseExact(gdto.DatumVremeManifestacije.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];
            Kupac kup = (Kupac)bp.listaKorisnika[gdto.KupacID];
            if (kup.SveMojeKarteBezObziraNaStatus.Contains(""))
                kup.SveMojeKarteBezObziraNaStatus.Remove("");


            for (int i = 0; i < gdto.BrojRezervisanihKarata; i++)
            {
                guid = Guid.NewGuid();
                idKarte = guid.ToString();

                Karta k = new Karta(idKarte, gdto.ManifestacijaID, datumString, gdto.Cena.ToString(), gdto.KupacID, Enums.StatusKarte.REZERVISANA.ToString(), gdto.Tip.ToString());
                bp.listaKarata.Add(k.Id, k);
                k.SacuvajKartu();

                kup.SveMojeKarteBezObziraNaStatus.Add(idKarte);

                kup.BrojSakupljenihBodova += gdto.BrojDodatnihBodova;

                if(kup.BrojSakupljenihBodova >= kup.TipKorisn.PotrebanBrojBodovaZlato)
                {
                    kup.TipKorisn.ImeTipa = Enums.TipKorisnika.ZLATNI;
                    kup.TipKorisn.Popust = 20; 
                }
                else if (kup.BrojSakupljenihBodova >= kup.TipKorisn.PotrebanBrojBodovaSrebro)
                {
                    kup.TipKorisn.ImeTipa = Enums.TipKorisnika.SREBRNI;
                    kup.TipKorisn.Popust = 10;
                }

                bp.AzurirajKorisnike();
            }

            return Ok();
        }

        [HttpGet]
        [Route("kupac-poseduje-kartu")]
        public IHttpActionResult ProveriPosedujeLiKupacKartu(string id)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaKarata = (Dictionary<string, Karta>)HttpContext.Current.Application["Karte"];
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];

            KupacPosedujeKartuDTO kpk = new KupacPosedujeKartuDTO(false, false);

            if (korisnikSesija.Uloga == Enums.Uloga.KUPAC)
            {
                Kupac k = (Kupac)korisnikSesija;
                kpk.KorisnikJeKupac = true;
                foreach (var item in k.SveMojeKarteBezObziraNaStatus)
                {
                    if (bp.listaKarata[item].ManifestacijaID == id && bp.listaKarata[item].Status == Enums.StatusKarte.REZERVISANA)
                    {
                        kpk.KorisnikPosedujeKartuManifestacije = true;
                        return Ok(kpk);
                    }
                }
            }

            return Ok(kpk);
        }

    }
}
