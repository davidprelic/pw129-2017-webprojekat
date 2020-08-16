using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string KomentarID { get; set; }
    }
}