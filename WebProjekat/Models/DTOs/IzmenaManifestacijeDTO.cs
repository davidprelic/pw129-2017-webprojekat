using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class IzmenaManifestacijeDTO
    {
        public string Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Naziv { get; set; }
        [Required]
        public int BrojMesta { get; set; }
        [Required]
        public int BrojRegularKarata { get; set; }
        [Required]
        public int BrojVipKarata { get; set; }
        [Required]
        public int BrojFanpitpKarata { get; set; }
        [Required]
        public DateTime DatumVremeOdrzavanja { get; set; }
        [Required]
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