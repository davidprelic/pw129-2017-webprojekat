using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KupacProfilDTO
    {
        public string Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string KorisnickoIme { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public Enums.Pol Pol { get; set; }
        [Required]
        public DateTime DatumRodjenja { get; set; }
        public double BrojSakupljenihBodova { get; set; }
        public TipKorisnika TipKorisn { get; set; }
        public Enums.Uloga Uloga { get; set; }
        public bool IsDeleted { get; set; }

        public KupacProfilDTO() { }

        public KupacProfilDTO(string id, string korisnickoIme, string ime, string prezime, Enums.Pol pol, DateTime datumRodjenja, double brojSakupljenihBodova, TipKorisnika tipKorisn, Enums.Uloga uloga, bool isDeleted)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            BrojSakupljenihBodova = brojSakupljenihBodova;
            TipKorisn = tipKorisn;
            Uloga = uloga;
            IsDeleted = isDeleted;
        }
    }
}