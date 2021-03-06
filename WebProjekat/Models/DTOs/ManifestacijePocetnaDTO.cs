using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class ManifestacijePocetnaDTO
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
        public MestoOdrzavanja MestoOdrzavanjaManif { get; set; }
        public int ProsecnaOcenaManif { get; set; }
        public bool IsDeleted { get; set; }

        public ManifestacijePocetnaDTO() { }

        public ManifestacijePocetnaDTO(string id, string naziv, Enums.TipManifestacije tip, int brojMesta, int brojRegularKarata, int brojVipKarata, int brojFanpitKarata, DateTime datumVremeOdrzavanja, decimal cenaRegularKarte, Enums.StatusManifestacije status, string lokacijaId, string posterManifestacije, MestoOdrzavanja mestoOdrzavanjaManif, int prosecnaOcenaManif, bool isDeleted)
        {
            Id = id;
            Naziv = naziv;
            Tip = tip;
            BrojMesta = brojMesta;
            BrojRegularKarata = brojRegularKarata;
            BrojVipKarata = brojVipKarata;
            BrojFanpitKarata = brojFanpitKarata;
            DatumVremeOdrzavanja = datumVremeOdrzavanja;
            CenaRegularKarte = cenaRegularKarte;
            Status = status;
            LokacijaId = lokacijaId;
            PosterManifestacije = posterManifestacije;
            MestoOdrzavanjaManif = mestoOdrzavanjaManif;
            ProsecnaOcenaManif = prosecnaOcenaManif;
            IsDeleted = isDeleted;
        }
    }
}