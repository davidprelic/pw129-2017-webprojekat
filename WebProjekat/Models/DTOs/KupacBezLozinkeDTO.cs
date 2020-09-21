using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KupacBezLozinkeDTO
    {
        public string Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Enums.Pol Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Enums.Uloga Uloga { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> SveMojeKarteBezObziraNaStatus { get; set; }
        public double BrojSakupljenihBodova { get; set; }
        public TipKorisnika TipKorisn { get; set; }
        public bool SumnjivKupac { get; set; }

        public KupacBezLozinkeDTO() { }

        public KupacBezLozinkeDTO(string id, string korisnickoIme, string ime, string prezime, Enums.Pol pol, DateTime datumRodjenja, Enums.Uloga uloga, bool isDeleted, List<string> sveMojeKarteBezObziraNaStatus, double brojSakupljenihBodova, TipKorisnika tipKorisn, bool sumnjivKupac)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            IsDeleted = isDeleted;
            SveMojeKarteBezObziraNaStatus = sveMojeKarteBezObziraNaStatus;
            BrojSakupljenihBodova = brojSakupljenihBodova;
            TipKorisn = tipKorisn;
            SumnjivKupac = sumnjivKupac;
        }
    }
}