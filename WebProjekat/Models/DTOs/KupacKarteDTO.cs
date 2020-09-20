using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KupacKarteDTO
    {
        public string Id { get; set; }
        public string ManifestacijaID { get; set; }
        public string NazivManif { get; set; }
        public DateTime DatumVremeManifestacije { get; set; }
        public decimal Cena { get; set; }
        public string KupacID { get; set; }
        public string KorImeKupca { get; set; }
        public Enums.TipKarte Tip { get; set; }
        public Enums.StatusKarte Status { get; set; }
        public bool IsDeleted { get; set; }

        public KupacKarteDTO() { }

        public KupacKarteDTO(string id, string manifestacijaID, string nazivManif, DateTime datumVremeManifestacije, decimal cena, string kupacID, string korImeKupca, Enums.TipKarte tip, Enums.StatusKarte status, bool isDeleted)
        {
            Id = id;
            ManifestacijaID = manifestacijaID;
            NazivManif = nazivManif;
            DatumVremeManifestacije = datumVremeManifestacije;
            Cena = cena;
            KupacID = kupacID;
            KorImeKupca = korImeKupca;
            Tip = tip;
            Status = status;
            IsDeleted = isDeleted;
        }
    }
}