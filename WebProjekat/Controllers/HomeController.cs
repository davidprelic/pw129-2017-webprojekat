using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebProjekat.Models;
using WebProjekat.Models.DTOs;

namespace WebProjekat.Controllers
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        BazaPodataka bp = new BazaPodataka();

        [HttpGet]
        [Route("")]
        public RedirectResult Index()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            var requestUri = Request.RequestUri;
            return Redirect(requestUri.AbsoluteUri + "Htmls/index.html");
        }

        [HttpGet]
        [Route("sesija")]
        public IHttpActionResult ProveriUloguIzSesije()
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            return Ok(korisnikSesija.Uloga.ToString());
        }

        [HttpGet]
        [Route("filtermanifestacije")]
        public IHttpActionResult PretragaSortFilter(string naziv, string mesto, DateTime? datumOd, DateTime? datumDo, decimal? cenaOd, decimal? cenaDo, string opcijaSort, string opcijaFilter)
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            bp.listaLokacija = (Dictionary<string, Lokacija>)HttpContext.Current.Application["Lokacije"];
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            List<Manifestacija> filter = new List<Manifestacija>();

            foreach (string key in bp.listaManifestacija.Keys)
            {
                string nazivPar = (naziv == null) ? "" : naziv;
                string mestoPar = (mesto == null) ? "" : mesto;



                //if (proizvodi.list[key].RaspolozivaKolicina != 0 && proizvodi.list[key].Naziv.Equals(naziv))
                if (bp.listaManifestacija[key].Naziv.Contains(nazivPar) && (bp.listaLokacija[bp.listaManifestacija[key].LokacijaId].MestoOdrzavanja.Grad.Contains(mestoPar) || bp.listaLokacija[bp.listaManifestacija[key].LokacijaId].MestoOdrzavanja.Drzava.Contains(mestoPar)))
                {
                    filter.Add(bp.listaManifestacija[key]);
                }
            }

            List<Manifestacija> datumFilter = new List<Manifestacija>();

            foreach (var item in filter)
            {
                if (datumOd == null && datumDo == null)
                {
                    datumFilter.Add(item);
                }
                else if (datumOd == null)
                {
                    if (DateTime.Compare(item.DatumVremeOdrzavanja, datumDo.Value) < 0)
                    {
                        datumFilter.Add(item);
                    }
                }
                else if (datumDo == null)
                {
                    if (DateTime.Compare(item.DatumVremeOdrzavanja, datumOd.Value) > 0)
                    {
                        datumFilter.Add(item);
                    }
                }
                else if (DateTime.Compare(item.DatumVremeOdrzavanja, datumOd.Value) > 0 && DateTime.Compare(item.DatumVremeOdrzavanja, datumDo.Value) < 0)
                {
                    datumFilter.Add(item);
                }
            }

            List<Manifestacija> cenaFilter = new List<Manifestacija>();

            foreach (var item in datumFilter)
            {
                if (cenaOd == null && cenaDo == null)
                {
                    cenaFilter.Add(item);
                }
                else if (cenaOd == null)
                {
                    if (item.CenaRegularKarte <= cenaDo)
                    {
                        cenaFilter.Add(item);
                    }
                }
                else if (cenaDo == null)
                {
                    if (item.CenaRegularKarte >= cenaOd)
                    {
                        cenaFilter.Add(item);
                    }
                }
                else if (item.CenaRegularKarte >= cenaOd && item.CenaRegularKarte <= cenaDo)
                {
                    cenaFilter.Add(item);
                }
            }

            if (opcijaSort == "poNazivuAZ")
                cenaFilter = cenaFilter.OrderBy(p => p.Naziv).ToList();
            else if (opcijaSort == "poNazivuZA")
                cenaFilter = cenaFilter.OrderByDescending(p => p.Naziv).ToList();
            else if (opcijaSort == "poDatumuRastuce")
                cenaFilter.Sort((x, y) => DateTime.Compare(x.DatumVremeOdrzavanja, y.DatumVremeOdrzavanja));
            else if (opcijaSort == "poDatumuOpadajuce")
                cenaFilter.Sort((x, y) => DateTime.Compare(y.DatumVremeOdrzavanja, x.DatumVremeOdrzavanja));
            else if (opcijaSort == "poCeniRastuce")
                cenaFilter = cenaFilter.OrderBy(p => p.CenaRegularKarte).ToList();
            else if (opcijaSort == "poCeniOpadajuce")
                cenaFilter = cenaFilter.OrderByDescending(p => p.CenaRegularKarte).ToList();
            else if (opcijaSort == "poMestuAZ")
                cenaFilter = cenaFilter.OrderBy(p => bp.listaLokacija[p.LokacijaId].MestoOdrzavanja.Grad).ToList();
            else if (opcijaSort == "poMestuZA")
                cenaFilter = cenaFilter.OrderByDescending(p => bp.listaLokacija[p.LokacijaId].MestoOdrzavanja.Grad).ToList();

            List<Manifestacija> filterOpcija = new List<Manifestacija>();

            if (opcijaFilter == "koncert")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipManifestacije.KONCERT)
                        filterOpcija.Add(item);
                }
                return Ok(DodajProsecnuOcenu(filterOpcija));
            }
            else if (opcijaFilter == "festival")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipManifestacije.FESTIVAL)
                        filterOpcija.Add(item);
                }
                return Ok(DodajProsecnuOcenu(filterOpcija));
            }
            else if (opcijaFilter == "pozoriste")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipManifestacije.POZORISTE)
                        filterOpcija.Add(item);
                }
                return Ok(DodajProsecnuOcenu(filterOpcija));
            }
            else if (opcijaFilter == "sport")
            {
                foreach (var item in cenaFilter)
                {
                    if (item.Tip == Enums.TipManifestacije.SPORT)
                        filterOpcija.Add(item);
                }
                return Ok(DodajProsecnuOcenu(filterOpcija));
            }
            else if (opcijaFilter == "nerasprodate")
            {
                foreach (var item in cenaFilter)
                {
                    if ((item.BrojFanpitKarata + item.BrojRegularKarata + item.BrojVipKarata) > 0)
                        filterOpcija.Add(item);
                }
                return Ok(DodajProsecnuOcenu(filterOpcija));
            }

            return Ok(DodajProsecnuOcenu(cenaFilter));
        }


        public List<ManifestacijePocetnaDTO> DodajProsecnuOcenu(List<Manifestacija> lista)
        {
            List<ManifestacijePocetnaDTO> pocetnaLista = new List<ManifestacijePocetnaDTO>();

            int prosecnaOcena = 0;
            int brojac = 0;
            int ocena;
            foreach (var item in lista)
            {
                if (item.Status == Enums.StatusManifestacije.AKTIVNO && !item.IsDeleted)
                {
                    foreach (var komentar in bp.listaKomentara.Values)
                    {
                        if (komentar.ManifestacijaID == item.Id && !komentar.IsDeleted)
                        {
                            brojac++;
                            if (komentar.Ocena == Enums.Ocena.JEDAN)
                                ocena = 1;
                            else if (komentar.Ocena == Enums.Ocena.DVA)
                                ocena = 2;
                            else if (komentar.Ocena == Enums.Ocena.TRI)
                                ocena = 3;
                            else if (komentar.Ocena == Enums.Ocena.CETIRI)
                                ocena = 4;
                            else
                                ocena = 5;

                            prosecnaOcena += ocena;
                        }
                    }
                    if (prosecnaOcena == 0)
                        pocetnaLista.Add(new ManifestacijePocetnaDTO(item.Id, item.Naziv, item.Tip, item.BrojMesta, item.BrojRegularKarata, item.BrojVipKarata, item.BrojFanpitKarata, item.DatumVremeOdrzavanja, item.CenaRegularKarte, item.Status, item.LokacijaId, item.PosterManifestacije, bp.listaLokacija[item.LokacijaId].MestoOdrzavanja, prosecnaOcena, item.IsDeleted));
                    else
                        pocetnaLista.Add(new ManifestacijePocetnaDTO(item.Id, item.Naziv, item.Tip, item.BrojMesta, item.BrojRegularKarata, item.BrojVipKarata, item.BrojFanpitKarata, item.DatumVremeOdrzavanja, item.CenaRegularKarte, item.Status, item.LokacijaId, item.PosterManifestacije, bp.listaLokacija[item.LokacijaId].MestoOdrzavanja, prosecnaOcena / brojac, item.IsDeleted));
                }
                brojac = 0;
                prosecnaOcena = 0;
            }

            return pocetnaLista;
        }

    }
}
