using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Karta
    {
        public string Id { get; set; }
        public string ManifestacijaID { get; set; }
        public DateTime DatumVremeManifestacije { get; set; }
        public decimal Cena { get; set; }
        public string KupacID { get; set; }
        public Enums.StatusKarte Status { get; set; }
        public Enums.TipKarte Tip { get; set; }
        public bool IsDeleted { get; set; }

        public Karta() { }

        public Karta(string id, string manifestacijaID, string datumVremeManifestacije, string cena, string kupacID, string status, string tip, string isDeleted)
        {
            Id = id;
            ManifestacijaID = manifestacijaID;
            DatumVremeManifestacije = DateTime.ParseExact(datumVremeManifestacije, "d/M/yyyy", CultureInfo.InvariantCulture);
            Cena = decimal.Parse(cena);
            KupacID = kupacID;
            Enums.StatusKarte sk;
            Enum.TryParse(status, out sk);
            Status = sk;
            Enums.TipKarte tk;
            Enum.TryParse(tip, out tk);
            Tip = tk;
            IsDeleted = bool.Parse(isDeleted);
        }

        public override string ToString()
        {
            DateTime dt = DateTime.ParseExact(DatumVremeManifestacije.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return $"{Id};{ManifestacijaID};{datumString};{Cena.ToString()};{KupacID};{Status.ToString()};{Tip.ToString()};{IsDeleted.ToString()}";
        }

        public string SacuvajKartu()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/karte.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(this.ToString());

            sw.Close();
            stream.Close();

            return Id;
        }

    }
}