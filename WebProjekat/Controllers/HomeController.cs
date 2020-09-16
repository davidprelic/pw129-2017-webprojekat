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

            List<Manifestacija> filter = new List<Manifestacija>();

            foreach (string key in bp.listaManifestacija.Keys)
            {
                string nazivPar = (naziv == null) ? "" : naziv;
                string mestoPar = (mesto == null) ? "" : mesto;

                //if (proizvodi.list[key].RaspolozivaKolicina != 0 && proizvodi.list[key].Naziv.Equals(naziv))
                if (bp.listaManifestacija[key].Naziv.Contains(nazivPar) && bp.listaManifestacija[key].MestoOdrzavanjaID.Contains(mestoPar))
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
                cenaFilter = cenaFilter.OrderBy(p => p.MestoOdrzavanjaID).ToList();
            else if (opcijaSort == "poMestuZA")
                cenaFilter = cenaFilter.OrderByDescending(p => p.MestoOdrzavanjaID).ToList();

            // DODATI JOS ZA FILTRIRANJE 

            return Ok(cenaFilter);
        }

    }
}
