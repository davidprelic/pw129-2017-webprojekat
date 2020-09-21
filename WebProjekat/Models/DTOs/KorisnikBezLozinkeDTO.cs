using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KorisnikBezLozinkeDTO
    {
        public string Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Enums.Pol Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Enums.Uloga Uloga { get; set; }
        public bool IsDeleted { get; set; }

        public KorisnikBezLozinkeDTO() { }

        public KorisnikBezLozinkeDTO(string id, string korisnickoIme, string ime, string prezime, Enums.Pol pol, DateTime datumRodjenja, Enums.Uloga uloga, bool isDeleted)
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