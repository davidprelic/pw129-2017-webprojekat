using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Enums
    {
        public enum Pol { MUSKI, ZENSKI }
        public enum Uloga { ADMINISTRATOR, PRODAVAC, KUPAC, NEULOGOVAN }
        public enum TipKorisnika { ZLATNI, SREBRNI, BRONZANI }
        public enum TipManifestacije { KONCERT, FESTIVAL, POZORISTE, SPORT }
        public enum StatusManifestacije { AKTIVNO, NEAKTIVNO }
        public enum StatusKarte { REZERVISANA, ODUSTANAK }
        public enum TipKarte { VIP, REGULAR, FANPIT }
        public enum Ocena { JEDAN, DVA, TRI, CETIRI, PET }
    }
}