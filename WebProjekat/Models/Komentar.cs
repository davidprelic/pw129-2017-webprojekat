using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Komentar
    {
        public string Id { get; set; }
        public string KupacID { get; set; }
        public string ManifestacijaID { get; set; }
        public string Tekst { get; set; }
        public Enums.Ocena Ocena { get; set; }
    }
}