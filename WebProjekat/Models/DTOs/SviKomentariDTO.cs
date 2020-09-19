using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class SviKomentariDTO
    {
        public string Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string NazivManif { get; set; }
        public string Tekst { get; set; }
        public Enums.Ocena Ocena { get; set; }
        public Enums.StatusKomentara Status { get; set; }

        public SviKomentariDTO() { }

        public SviKomentariDTO(string id, string korisnickoIme, string nazivManif, string tekst, Enums.Ocena ocena, Enums.StatusKomentara status)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            NazivManif = nazivManif;
            Tekst = tekst;
            Ocena = ocena;
            Status = status;
        }
    }
}