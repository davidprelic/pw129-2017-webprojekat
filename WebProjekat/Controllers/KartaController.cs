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

                Karta k = new Karta(idKarte, gdto.ManifestacijaID, datumString, gdto.Cena.ToString(), gdto.KupacID, Enums.StatusKarte.REZERVISANA.ToString(), gdto.Tip.ToString(), "False");
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

                if (k.SveMojeKarteBezObziraNaStatus.Contains(""))
                    k.SveMojeKarteBezObziraNaStatus.Remove("");

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

        [HttpGet]
        [Route("rezervisane-karte")]
        public IHttpActionResult PribaviRezervisaneKarteProdavca()
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

            List<RezKarteProdavacDTO> pomocnaLista = new List<RezKarteProdavacDTO>();

            if (korisnikSesija.Uloga == Enums.Uloga.PRODAVAC)
            {
                Prodavac p = (Prodavac)korisnikSesija;

                if (p.SveMojeManifestacije.Contains(""))
                    p.SveMojeManifestacije.Remove("");

                foreach (var item in bp.listaKarata.Values)
                {
                    if (p.SveMojeManifestacije.Contains(item.ManifestacijaID) && item.Status == Enums.StatusKarte.REZERVISANA)
                        pomocnaLista.Add(new RezKarteProdavacDTO(item.Id, item.ManifestacijaID, bp.listaManifestacija[item.ManifestacijaID].Naziv, item.DatumVremeManifestacije, item.Cena, item.KupacID, bp.listaKorisnika[item.KupacID].KorisnickoIme, item.Tip, item.IsDeleted));
                }
            }

            return Ok(pomocnaLista);
        }

        [HttpGet]
        [Route("sve-karte")]
        public IHttpActionResult SveKarte()
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

            List<KupacKarteDTO> pomocnaLista = new List<KupacKarteDTO>();

            if (korisnikSesija.Uloga == Enums.Uloga.ADMINISTRATOR)
            {
                foreach (var item in bp.listaKarata.Values)
                {
                    pomocnaLista.Add(new KupacKarteDTO(item.Id, item.ManifestacijaID, bp.listaManifestacija[item.ManifestacijaID].Naziv, item.DatumVremeManifestacije, item.Cena, item.KupacID, bp.listaKorisnika[item.KupacID].KorisnickoIme, item.Tip, item.Status, item.IsDeleted));
                }
            }

            return Ok(pomocnaLista);
        }

        [HttpGet]
        [Route("karte")]
        public IHttpActionResult SveKarteKupca()
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

            List<KupacKarteDTO> pomocnaLista = new List<KupacKarteDTO>();
            
            if (korisnikSesija.Uloga == Enums.Uloga.KUPAC)
            {
                Kupac k = (Kupac)korisnikSesija;

                if (k.SveMojeKarteBezObziraNaStatus.Contains(""))
                    k.SveMojeKarteBezObziraNaStatus.Remove("");

                foreach (var item in bp.listaKarata.Values)
                {
                    if (k.SveMojeKarteBezObziraNaStatus.Contains(item.Id))
                        pomocnaLista.Add(new KupacKarteDTO(item.Id, item.ManifestacijaID, bp.listaManifestacija[item.ManifestacijaID].Naziv, item.DatumVremeManifestacije, item.Cena, item.KupacID, bp.listaKorisnika[item.KupacID].KorisnickoIme, item.Tip, item.Status, item.IsDeleted));

                }
            }

            return Ok(pomocnaLista);
        }

        [HttpGet]
        [Route("filterkarte")]
        public IHttpActionResult PretragaSortFilter(string manifestacija, DateTime? datumOd, DateTime? datumDo, decimal? cenaOd, decimal? cenaDo, string opcijaSort, string opcijaFilter)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            Kupac k = (Kupac)korisnikSesija;

            if (k.SveMojeKarteBezObziraNaStatus.Contains(""))
                k.SveMojeKarteBezObziraNaStatus.Remove("");

            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            bp.listaLokacija = (Dictionary<string, Lokacija>)HttpContext.Current.Application["Lokacije"];
            bp.listaKarata = (Dictionary<string, Karta>)HttpContext.Current.Application["Karte"];
            bp.listaKorisnika = (Dictionary<string, Korisnik>)HttpContext.Current.Application["Korisnici"];

            List<KupacKarteDTO> filter = new List<KupacKarteDTO>();

            foreach (var item in bp.listaKarata.Values)
            {
                string manifestacijaPar = (manifestacija == null) ? "" : manifestacija;

                foreach (var mojaKarta in k.SveMojeKarteBezObziraNaStatus)
                {
                    if(mojaKarta == item.Id)
                    {
                        if (bp.listaManifestacija[bp.listaKarata[mojaKarta].ManifestacijaID].Naziv.Contains(manifestacijaPar))
                        {
                            filter.Add(new KupacKarteDTO(item.Id, item.ManifestacijaID, bp.listaManifestacija[item.ManifestacijaID].Naziv, item.DatumVremeManifestacije, item.Cena, item.KupacID, bp.listaKorisnika[item.KupacID].KorisnickoIme, item.Tip, item.Status, item.IsDeleted));
                        }
                    }
                }
            }

            List<KupacKarteDTO> datumFilter = new List<KupacKarteDTO>();

            foreach (var item in filter)
            {
                if (datumOd == null && datumDo == null)
                {
                    datumFilter.Add(item);
                }
                else if (datumOd == null)
                {
                    if (DateTime.Compare(item.DatumVremeManifestacije, datumDo.Value) < 0)
                    {
                        datumFilter.Add(item);
                    }
                }
                else if (datumDo == null)
                {
                    if (DateTime.Compare(item.DatumVremeManifestacije, datumOd.Value) > 0)
                    {
                        datumFilter.Add(item);
                    }
                }
                else if (DateTime.Compare(item.DatumVremeManifestacije, datumOd.Value) > 0 && DateTime.Compare(item.DatumVremeManifestacije, datumDo.Value) < 0)
                {
                    datumFilter.Add(item);
                }
            }

            List<KupacKarteDTO> cenaFilter = new List<KupacKarteDTO>();

            foreach (var item in datumFilter)
            {
                if (cenaOd == null && cenaDo == null)
                {
                    cenaFilter.Add(item);
                }
                else if (cenaOd == null)
                {
                    if (item.Cena <= cenaDo)
                    {
                        cenaFilter.Add(item);
                    }
                }
                else if (cenaDo == null)
                {
                    if (item.Cena >= cenaOd)
                    {
                        cenaFilter.Add(item);
                    }
                }
                else if (item.Cena >= cenaOd && item.Cena <= cenaDo)
                {
                    cenaFilter.Add(item);
                }
            }

            if (opcijaSort == "poManifAZ")
                cenaFilter = cenaFilter.OrderBy(p => p.NazivManif).ToList();
            else if (opcijaSort == "poManifZA")
                cenaFilter = cenaFilter.OrderByDescending(p => p.NazivManif).ToList();
            else if (opcijaSort == "poDatumuRastuce")
                cenaFilter.Sort((x, y) => DateTime.Compare(x.DatumVremeManifestacije, y.DatumVremeManifestacije));
            else if (opcijaSort == "poDatumuOpadajuce")
                cenaFilter.Sort((x, y) => DateTime.Compare(y.DatumVremeManifestacije, x.DatumVremeManifestacije));
            else if (opcijaSort == "poCeniRastuce")
                cenaFilter = cenaFilter.OrderBy(p => p.Cena).ToList();
            else if (opcijaSort == "poCeniOpadajuce")
                cenaFilter = cenaFilter.OrderByDescending(p => p.Cena).ToList();
            

            List<KupacKarteDTO> filterOpcija = new List<KupacKarteDTO>();

            if (opcijaFilter == "vip")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipKarte.VIP)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "regular")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipKarte.REGULAR)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "fanpit")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipKarte.FANPIT)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "rezervisana")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Status == Enums.StatusKarte.REZERVISANA)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }
            else if (opcijaFilter == "odustanak")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Status == Enums.StatusKarte.ODUSTANAK)
                        filterOpcija.Add(item);
                }
                return Ok(filterOpcija);
            }

            return Ok(cenaFilter);
        }

    }
}
