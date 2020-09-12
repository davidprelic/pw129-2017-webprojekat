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
        public DateTime DatumVremeOdrzavanja { get; set; }
        public decimal CenaRegularKarte { get; set; }
        public Enums.StatusManifestacije Status { get; set; }
        public string MestoOdrzavanjaID { get; set; }
        public string PosterManifestacije { get; set; } // SLIKA
        public bool IsDeleted { get; set; }

        public Manifestacija() {}

        public Manifestacija(string id, string naziv, string tip, string brojMesta, string datumVremeOdrzavanja, string cenaRegularKarte, string status, string mestoOdrzavanjaID, string posterManifestacije, string isDeleted)
        {
            Id = id;
            Naziv = naziv;
            Enums.TipManifestacije tm;
            Enum.TryParse(tip, out tm);
            Tip = tm;
            BrojMesta = Int32.Parse(brojMesta);
            DatumVremeOdrzavanja = DateTime.ParseExact(datumVremeOdrzavanja, "d/M/yyyy", CultureInfo.InvariantCulture);
            CenaRegularKarte = Decimal.Parse(cenaRegularKarte);
            Enums.StatusManifestacije sm;
            Enum.TryParse(status, out sm);
            Status = sm;
            MestoOdrzavanjaID = mestoOdrzavanjaID;
            PosterManifestacije = posterManifestacije;
            IsDeleted = bool.Parse(isDeleted);
        }

        public override string ToString()
        {
            DateTime dt = DateTime.ParseExact(DatumVremeOdrzavanja.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = dt.Date;
            string datumString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return $"{Id};{Tip.ToString()};{BrojMesta.ToString()};{datumString};{CenaRegularKarte.ToString()};{Status.ToString()};{MestoOdrzavanjaID};{PosterManifestacije};{IsDeleted}";
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