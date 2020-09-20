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
            bp.listaLokacija = (Dictionary<string, Lokacija>)HttpContext.Current.Application["Lokacije"];

            foreach (var item in bp.listaManifestacija.Values)
            {
                if(item.DatumVremeOdrzavanja == mdto.DatumVremeOdrzavanja)
                {
                    foreach (var lokac in bp.listaLokacija.Values)
                    {
                        if (lokac.GeografskaDuzina == double.Parse(mdto.GeografskaDuzina) && lokac.GeografskaSirina == double.Parse(mdto.GeografskaSirina))
                            return BadRequest();
                    }
                }
            }


            Guid guid = Guid.NewGuid();
            string strId = guid.ToString();

            DateTime dt = DateTime.ParseExact(mdto.DatumVremeOdrzavanja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            MestoOdrzavanja mestoOdrz = new MestoOdrzavanja(mdto.Ulica, mdto.Grad, mdto.Drzava, int.Parse(mdto.PostanskiBroj));
            Lokacija lok = new Lokacija(Guid.NewGuid().ToString(), mdto.GeografskaDuzina, mdto.GeografskaSirina, mestoOdrz.Ulica, mestoOdrz.Grad, mestoOdrz.Drzava, mestoOdrz.PostanskiBroj.ToString());

            bp.listaLokacija = (Dictionary<string, Lokacija>)HttpContext.Current.Application["Lokacije"];
            bp.listaLokacija.Add(lok.Id, lok);
            lok.SacuvajLokaciju();

            Manifestacija m = new Manifestacija(strId, mdto.Naziv, mdto.Tip.ToString(), mdto.BrojMesta.ToString(), mdto.BrojRegularKarata.ToString(), mdto.BrojVipKarata.ToString(), mdto.BrojFanpitKarata.ToString(), datumString, mdto.CenaRegularKarte.ToString(), mdto.Status.ToString(), lok.Id, mdto.PosterManifestacije, mdto.IsDeleted.ToString());

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
            bp.listaLokacija = (Dictionary<string, Lokacija>)HttpContext.Current.Application["Lokacije"];
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            List<ManifestacijePocetnaDTO> pomocnaLista = new List<ManifestacijePocetnaDTO>();

            int prosecnaOcena = 0;
            int brojac = 0;
            int ocena;

            foreach(var item in bp.listaManifestacija.Values)
            {
                if (item.Status == Enums.StatusManifestacije.AKTIVNO)
                {
                    foreach (var komentar in bp.listaKomentara.Values)
                    {
                        if(komentar.ManifestacijaID == item.Id)
                        {
                            brojac++;
                            if(komentar.Ocena == Enums.Ocena.JEDAN)
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

                    if(prosecnaOcena == 0)
                        pomocnaLista.Add(new ManifestacijePocetnaDTO(item.Id, item.Naziv, item.Tip, item.BrojMesta, item.BrojRegularKarata, item.BrojVipKarata, item.BrojFanpitKarata, item.DatumVremeOdrzavanja, item.CenaRegularKarte, item.Status, item.LokacijaId, item.PosterManifestacije, bp.listaLokacija[item.LokacijaId].MestoOdrzavanja, prosecnaOcena, item.IsDeleted));
                    else
                        pomocnaLista.Add(new ManifestacijePocetnaDTO(item.Id, item.Naziv, item.Tip, item.BrojMesta, item.BrojRegularKarata, item.BrojVipKarata, item.BrojFanpitKarata, item.DatumVremeOdrzavanja, item.CenaRegularKarte, item.Status, item.LokacijaId, item.PosterManifestacije, bp.listaLokacija[item.LokacijaId].MestoOdrzavanja, prosecnaOcena/brojac, item.IsDeleted));

                }
                brojac = 0;
                prosecnaOcena = 0;
            }

            string json = JsonConvert.SerializeObject(pomocnaLista);
            return Ok(json);
        }

        [HttpGet]
        [Route("manifestacije/{id}")]
        public IHttpActionResult PrikazManifestacije(string id)
        {
            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            bp.listaLokacija = (Dictionary<string, Lokacija>)HttpContext.Current.Application["Lokacije"];
            bp.listaKomentara = (Dictionary<string, Komentar>)HttpContext.Current.Application["Komentari"];

            ManifestacijaDTO mdto = new ManifestacijaDTO();
            int prosecnaOcena = 0;
            int brojac = 0;
            int ocena;

            foreach (var komentar in bp.listaKomentara.Values)
            {
                if (komentar.ManifestacijaID == id)
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

            if (bp.listaManifestacija.ContainsKey(id))
            {
                mdto.Naziv = bp.listaManifestacija[id].Naziv;
                mdto.Tip = bp.listaManifestacija[id].Tip;
                mdto.BrojMesta = bp.listaManifestacija[id].BrojMesta;
                mdto.BrojRegularKarata = bp.listaManifestacija[id].BrojRegularKarata;
                mdto.BrojVipKarata = bp.listaManifestacija[id].BrojVipKarata;
                mdto.BrojFanpitKarata = bp.listaManifestacija[id].BrojFanpitKarata;
                mdto.DatumVremeOdrzavanja = bp.listaManifestacija[id].DatumVremeOdrzavanja;
                mdto.CenaRegularKarte = bp.listaManifestacija[id].CenaRegularKarte;
                mdto.Status = bp.listaManifestacija[id].Status;
                mdto.GeografskaSirina = bp.listaLokacija[bp.listaManifestacija[id].LokacijaId].GeografskaSirina.ToString();
                mdto.GeografskaDuzina = bp.listaLokacija[bp.listaManifestacija[id].LokacijaId].GeografskaDuzina.ToString();
                mdto.Ulica = bp.listaLokacija[bp.listaManifestacija[id].LokacijaId].MestoOdrzavanja.Ulica.ToString();
                mdto.Grad = bp.listaLokacija[bp.listaManifestacija[id].LokacijaId].MestoOdrzavanja.Grad.ToString();
                mdto.Drzava = bp.listaLokacija[bp.listaManifestacija[id].LokacijaId].MestoOdrzavanja.Drzava.ToString();
                mdto.PostanskiBroj = bp.listaLokacija[bp.listaManifestacija[id].LokacijaId].MestoOdrzavanja.PostanskiBroj.ToString();
                mdto.PosterManifestacije = bp.listaManifestacija[id].PosterManifestacije;
                if (prosecnaOcena != 0)
                    mdto.OcenaManifestacije = prosecnaOcena / brojac;
                else
                    mdto.OcenaManifestacije = prosecnaOcena;
                mdto.IsDeleted = bp.listaManifestacija[id].IsDeleted;


                return Ok(mdto);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("manifestacije")]
        public IHttpActionResult IzmeniManifestaciju(IzmenaManifestacijeDTO imdto)
        {
            Korisnik korisnikSesija = (Korisnik)HttpContext.Current.Session["Korisnik"];
            if (korisnikSesija == null)
            {
                korisnikSesija = new Korisnik();
                HttpContext.Current.Session["Korisnik"] = korisnikSesija;
            }

            bp.listaManifestacija = (Dictionary<string, Manifestacija>)HttpContext.Current.Application["Manifestacije"];
            if (bp.listaManifestacija.ContainsKey(imdto.Id))
            {
                bp.listaManifestacija[imdto.Id].Naziv = imdto.Naziv;
                bp.listaManifestacija[imdto.Id].BrojMesta = imdto.BrojMesta;
                bp.listaManifestacija[imdto.Id].BrojRegularKarata = imdto.BrojRegularKarata;
                bp.listaManifestacija[imdto.Id].BrojVipKarata = imdto.BrojVipKarata;
                bp.listaManifestacija[imdto.Id].BrojFanpitKarata = imdto.BrojFanpitpKarata;
                bp.listaManifestacija[imdto.Id].DatumVremeOdrzavanja = imdto.DatumVremeOdrzavanja;
                bp.listaManifestacija[imdto.Id].CenaRegularKarte = imdto.CenaRegularKarte;
                
                bp.AzurirajManifestacije();
                return Ok();
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

        [HttpPost]
        [Route("upload-slike")]
        public HttpResponseMessage UploadSlike()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            // Check if files are available
            if (httpRequest.Files.Count > 0)
            {
                var files = new List<string>();

                // interate the files and save on the server
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    files.Add(filePath);
                }

                // return result
                result = Request.CreateResponse(HttpStatusCode.Created, files);
            }
            else
            {
                // return BadRequest (no file(s) available)
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }

    }
}
