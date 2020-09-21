using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Kupac : Korisnik
    {
        public List<string> SveMojeKarteBezObziraNaStatus { get; set; }
        public double BrojSakupljenihBodova { get; set; }
        public TipKorisnika TipKorisn { get; set; }
        public bool SumnjivKupac { get; set; }

        public Kupac() { }

        public Kupac(string id, string korisnickoIme, string lozinka, string ime, string prezime, string pol, string datumRodjenja, string uloga, string sveMojeKarteBezObziraNaStatus, string brojSakupljenihBodova, string tipKorisnika, string sumnjivKupac,string isDeleted)
        {
            Id = id;
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
            if (sveMojeKarteBezObziraNaStatus == "")
            {
                List<string> init = new List<string>();
                init.Add("");
                SveMojeKarteBezObziraNaStatus = init;
            }
            else
            {
                List<string> sveMojeKarteBezObziraNaStatusLista = sveMojeKarteBezObziraNaStatus.Split('|')[1].Split(',').ToList();
                SveMojeKarteBezObziraNaStatus = sveMojeKarteBezObziraNaStatusLista;
            }    

            BrojSakupljenihBodova = double.Parse(brojSakupljenihBodova);

            //PROVERITI DA LI MOZE OVAKO, i DA LI MI TREBA TIP KORISN KAO PARAMETAR KONSTRUKTORA
            TipKorisn = new TipKorisnika();

            SumnjivKupac = bool.Parse(sumnjivKupac);

            IsDeleted = bool.Parse(isDeleted);
        }


        public override string ToString()
        {
            DateTime dt = DateTime.ParseExact(DatumRodjenja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            string ret = $"{Id};{KorisnickoIme};{Lozinka};{Ime};{Prezime};{Pol.ToString()};{datumString};{Uloga.ToString()};|";
            foreach (string item in SveMojeKarteBezObziraNaStatus)
            {
                ret += $"{item},";
            }
            ret = ret.Remove(ret.Length - 1);
            ret += $"|;{BrojSakupljenihBodova.ToString()};{TipKorisn.ImeTipa.ToString()};{SumnjivKupac.ToString()};{IsDeleted}";

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