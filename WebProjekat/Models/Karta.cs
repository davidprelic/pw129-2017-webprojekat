using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}