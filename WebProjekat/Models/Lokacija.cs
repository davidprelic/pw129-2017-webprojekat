using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Lokacija
    {
        public float GeografskaDuzina { get; set; }
        public float GeografskaSirina { get; set; }
        public MestoOdrzavanja MestoOdrzavanja { get; set; }  // Ovo mozda nije poseban entitet!!!

    }
}