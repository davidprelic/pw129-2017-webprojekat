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
        public int PotrebanBrojBodova { get; set; }

        public TipKorisnika()
        {
            ImeTipa = Enums.TipKorisnika.BRONZANI;
            Popust = 10;
            PotrebanBrojBodova = 2500;
        }
        
    }
}