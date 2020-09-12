using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Korisnik
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string KorisnickoIme { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 8)]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string Lozinka { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public Enums.Pol Pol { get; set; }
        [Required]
        public DateTime DatumRodjenja { get; set; }
        public Enums.Uloga Uloga { get; set; }
        public bool IsDeleted { get; set; }


        public Korisnik()
        {
            KorisnickoIme = "";
            Lozinka = "";
            Ime = "";
            Prezime = "";
            Pol = Enums.Pol.MUSKI;
            DatumRodjenja = DateTime.Now;
            Uloga = Enums.Uloga.NEULOGOVAN;
            IsDeleted = false;
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string datumRodjenja, string uloga, string isDeleted)
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
            IsDeleted = bool.Parse(isDeleted);
        }

        public override string ToString()
        {
            DateTime dt = DateTime.ParseExact(DatumRodjenja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return $"{KorisnickoIme};{Lozinka};{Ime};{Prezime};{Pol.ToString()};{datumString};{Uloga.ToString()};{IsDeleted.ToString()}";
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