using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class TipKorisnika
    {
        public Enums.TipKorisnika ImeTipa { get; set; }
        public int Popust { get; set; }
        public int PotrebanBrojBodovaSrebro { get; set; }
        public int PotrebanBrojBodovaZlato { get; set; }

        public TipKorisnika()
        {
            ImeTipa = Enums.TipKorisnika.BRONZANI;
            Popust = 5;
            PotrebanBrojBodovaSrebro = 10000;
            PotrebanBrojBodovaZlato = 20000;
        }
        
    }
}