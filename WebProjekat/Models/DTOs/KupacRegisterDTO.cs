using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KupacRegisterDTO
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string KorisnickoIme { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 8)]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string Lozinka { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public Enums.Pol Pol { get; set; }
        [Required]
        public DateTime DatumRodjenja { get; set; }
        public Enums.Uloga Uloga { get; set; }
        public bool IsDeleted { get; set; }
        public int BrojSakupljenihBodova { get; set; }

        public KupacRegisterDTO() { }

        public KupacRegisterDTO(string korisnickoIme, string lozinka, string ime, string prezime, Enums.Pol pol, DateTime datumRodjenja, Enums.Uloga uloga, bool isDeleted, int brojSakupljenihBodova)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            IsDeleted = isDeleted;
            BrojSakupljenihBodova = brojSakupljenihBodova;
        }
    }
}