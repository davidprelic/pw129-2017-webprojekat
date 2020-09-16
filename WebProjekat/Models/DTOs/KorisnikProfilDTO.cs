using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KorisnikProfilDTO
    {
        public string Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string KorisnickoIme { get; set; }
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

        public KorisnikProfilDTO() { }

        public KorisnikProfilDTO(string id, string korisnickoIme, string ime, string prezime, Enums.Pol pol, DateTime datumRodjenja, Enums.Uloga uloga, bool isDeleted)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            IsDeleted = isDeleted;
        }
    }
}