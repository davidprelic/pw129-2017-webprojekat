using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class GenerisanjeKarataDTO
    {
        public string ManifestacijaID { get; set; }
        public DateTime DatumVremeManifestacije { get; set; }
        public decimal Cena { get; set; }
        public int BrojRezervisanihKarata { get; set; }
        public string KupacID { get; set; }
        public Enums.TipKarte Tip { get; set; }
        public double BrojDodatnihBodova { get; set; }

        public GenerisanjeKarataDTO() { }

        public GenerisanjeKarataDTO(string manifestacijaID, DateTime datumVremeManifestacije, decimal cena, int brojRezervisanihKarata, string kupacID, Enums.TipKarte tip, double brojDodatnihBodova)
        {
            ManifestacijaID = manifestacijaID;
            DatumVremeManifestacije = datumVremeManifestacije;
            Cena = cena;
            BrojRezervisanihKarata = brojRezervisanihKarata;
            KupacID = kupacID;
            Tip = tip;
            BrojDodatnihBodova = brojDodatnihBodova;
        }
    }
}