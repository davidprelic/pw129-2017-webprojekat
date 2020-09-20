using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class IzmenaManifestacijeDTO
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public int BrojMesta { get; set; }
        public int BrojRegularKarata { get; set; }
        public int BrojVipKarata { get; set; }
        public int BrojFanpitpKarata { get; set; }
        public DateTime DatumVremeOdrzavanja { get; set; }
        public decimal CenaRegularKarte { get; set; }

        public IzmenaManifestacijeDTO() { }

        public IzmenaManifestacijeDTO(string id, string naziv, int brojMesta, int brojRegularKarata, int brojVipKarata, int brojFanpitpKarata, DateTime datumVremeOdrzavanja, decimal cenaRegularKarte)
        {
            Id = id;
            Naziv = naziv;
            BrojMesta = brojMesta;
            BrojRegularKarata = brojRegularKarata;
            BrojVipKarata = brojVipKarata;
            BrojFanpitpKarata = brojFanpitpKarata;
            DatumVremeOdrzavanja = datumVremeOdrzavanja;
            CenaRegularKarte = cenaRegularKarte;
        }
    }
}