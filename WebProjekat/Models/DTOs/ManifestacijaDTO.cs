using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class ManifestacijaDTO
    {
        public string Naziv { get; set; }
        public Enums.TipManifestacije Tip { get; set; }
        public int BrojMesta { get; set; }
        public int BrojRegularKarata { get; set; }
        public int BrojVipKarata { get; set; }
        public int BrojFanpitKarata { get; set; }
        public DateTime DatumVremeOdrzavanja { get; set; }
        public decimal CenaRegularKarte { get; set; }
        public Enums.StatusManifestacije Status { get; set; }
        public string MestoOdrzavanjaID { get; set; }
        public string PosterManifestacije { get; set; } // SLIKA
        public bool IsDeleted { get; set; }

        public ManifestacijaDTO() { }

        public ManifestacijaDTO(string naziv, Enums.TipManifestacije tip, int brojMesta, int brojRegularKarata, int brojVipKarata, int brojFanpitKarata, DateTime datumVremeOdrzavanja, decimal cenaRegularKarte, Enums.StatusManifestacije status, string mestoOdrzavanjaID, string posterManifestacije, bool isDeleted)
        {
            Naziv = naziv;
            Tip = tip;
            BrojMesta = brojMesta;
            BrojRegularKarata = brojRegularKarata;
            BrojVipKarata = brojVipKarata;
            BrojFanpitKarata = brojFanpitKarata;
            DatumVremeOdrzavanja = datumVremeOdrzavanja;
            CenaRegularKarte = cenaRegularKarte;
            Status = status;
            MestoOdrzavanjaID = mestoOdrzavanjaID;
            PosterManifestacije = posterManifestacije;
            IsDeleted = isDeleted;
        }
    }
}