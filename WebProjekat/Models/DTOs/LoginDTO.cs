using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class LoginDTO
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }

        public LoginDTO() { }

        public LoginDTO(string korisnickoIme, string lozinka)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
        }
    }
}