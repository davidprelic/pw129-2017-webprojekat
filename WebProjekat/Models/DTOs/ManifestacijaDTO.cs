using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class ManifestacijaDTO
    {
        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Naziv { get; set; }
        public Enums.TipManifestacije Tip { get; set; }
        [Required]
        public int BrojMesta { get; set; }
        [Required]
        public int BrojRegularKarata { get; set; }
        [Required]
        public int BrojVipKarata { get; set; }
        [Required]
        public int BrojFanpitKarata { get; set; }
        [Required]
        public DateTime DatumVremeOdrzavanja { get; set; }
        [Required]
        public decimal CenaRegularKarte { get; set; }
        public Enums.StatusManifestacije Status { get; set; }
        public string GeografskaSirina { get; set; }
        public string GeografskaDuzina { get; set; }
        public string Ulica { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }
        public string PostanskiBroj { get; set; }
        public int OcenaManifestacije { get; set; }
        public string PosterManifestacije { get; set; } // SLIKA
        public bool IsDeleted { get; set; }

        public ManifestacijaDTO() { }

        public ManifestacijaDTO(string naziv, Enums.TipManifestacije tip, int brojMesta, int brojRegularKarata, int brojVipKarata, int brojFanpitKarata, DateTime datumVremeOdrzavanja, decimal cenaRegularKarte, Enums.StatusManifestacije status, string geografskaSirina, string geografskaDuzina, string ulica, string grad, string drzava, string postanskiBroj, int ocenaManifestacije, string posterManifestacije, bool isDeleted)
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
            GeografskaSirina = geografskaSirina;
            GeografskaDuzina = geografskaDuzina;
            Ulica = ulica;
            Grad = grad;
            Drzava = drzava;
            PostanskiBroj = postanskiBroj;
            OcenaManifestacije = ocenaManifestacije;
            PosterManifestacije = posterManifestacije;
            IsDeleted = isDeleted;
        }
    }
}