using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class MestoOdrzavanja
    {
        public string Ulica { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }
        public int PostanskiBroj { get; set; }

        public MestoOdrzavanja() { }

        public MestoOdrzavanja(string ulica, string grad, string drzava, int postanskiBroj)
        {
            Ulica = ulica;
            Grad = grad;
            Drzava = drzava;
            PostanskiBroj = postanskiBroj;
        }
    }
}