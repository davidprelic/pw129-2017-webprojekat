using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Prodavac : Korisnik
    {
        public List<string> SveMojeManifestacije { get; set; }

        public Prodavac() { }

        public Prodavac(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string datumRodjenja, string uloga, string sveMojeManifestacije, string isDeleted)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            if (pol == "MUSKI")
                Pol = Enums.Pol.MUSKI;
            else
                Pol = Enums.Pol.ZENSKI;
            DatumRodjenja = DateTime.ParseExact(datumRodjenja, "d/M/yyyy", CultureInfo.InvariantCulture);
            Enums.Uloga u;
            Enum.TryParse(uloga, out u);
            Uloga = u;

            //PROVERITI DA LI MOZE OVAKO
            SveMojeManifestacije = sveMojeManifestacije.Split(',').ToList();

            IsDeleted = bool.Parse(isDeleted);
        }

        public override string ToString()
        {
            DateTime dt = DateTime.ParseExact(DatumRodjenja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            string ret = $"{KorisnickoIme};{Lozinka};{Ime};{Prezime};{Pol.ToString()};{datumString};{Uloga.ToString()};|";
            foreach (string item in SveMojeManifestacije)
            {
                ret += $"{item},";
            }
            ret = ret.Remove(ret.Length - 1);
            ret += $"|;{IsDeleted}";

            return ret;
        }

        public string SacuvajKorisnika()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(this.ToString());

            sw.Close();
            stream.Close();

            return KorisnickoIme;
        }
    }
}