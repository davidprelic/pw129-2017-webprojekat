using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Manifestacija
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public Enums.TipManifestacije Tip { get; set; }
        public int BrojMesta { get; set; }
        public int BrojRegularKarata { get; set; }
        public int BrojVipKarata { get; set; }
        public int BrojFanpitKarata { get; set; }
        public DateTime DatumVremeOdrzavanja { get; set; }
        public decimal CenaRegularKarte { get; set; }
        public Enums.StatusManifestacije Status { get; set; }
        public string LokacijaId { get; set; }
        public string PosterManifestacije { get; set; } // SLIKA
        public bool IsDeleted { get; set; }

        public Manifestacija() {}

        public Manifestacija(string id, string naziv, string tip, string brojMesta, string brojRegularKarata, string brojVipKarata, string brojFanpitKarata, string datumVremeOdrzavanja, string cenaRegularKarte, string status, string lokacijaId, string posterManifestacije, string isDeleted)
        {
            Id = id;
            Naziv = naziv;
            Enums.TipManifestacije tm;
            Enum.TryParse(tip, out tm);
            Tip = tm;
            BrojMesta = Int32.Parse(brojMesta);
            DatumVremeOdrzavanja = DateTime.ParseExact(datumVremeOdrzavanja, "d/M/yyyy", CultureInfo.InvariantCulture);
            CenaRegularKarte = Decimal.Parse(cenaRegularKarte);
            BrojRegularKarata = Int32.Parse(brojRegularKarata);
            BrojVipKarata = Int32.Parse(brojVipKarata);
            BrojFanpitKarata = Int32.Parse(brojFanpitKarata);
            Enums.StatusManifestacije sm;
            Enum.TryParse(status, out sm);
            Status = sm;
            LokacijaId = lokacijaId;
            PosterManifestacije = posterManifestacije;
            IsDeleted = bool.Parse(isDeleted);
        }

        public override string ToString()
        {
            DateTime dt = DateTime.ParseExact(DatumVremeOdrzavanja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return $"{Id};{Naziv};{Tip.ToString()};{BrojMesta.ToString()};{BrojRegularKarata.ToString()};{BrojVipKarata.ToString()};{BrojFanpitKarata.ToString()};{datumString};{CenaRegularKarte.ToString()};{Status.ToString()};{LokacijaId};{PosterManifestacije};{IsDeleted}";
        }

        public string SacuvajManifestaciju()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/manifestacije.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(this.ToString());

            sw.Close();
            stream.Close();

            return Id;
        }

    }
}