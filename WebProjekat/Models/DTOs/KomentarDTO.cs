using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KomentarDTO
    {
        public string ManifestacijaID { get; set; }
        public string Tekst { get; set; }
        public Enums.Ocena Ocena { get; set; }
        public Enums.StatusKomentara Status { get; set; }
        public bool IsDeleted { get; set; }

        public KomentarDTO() { }

        public KomentarDTO(string manifestacijaID, string tekst, Enums.Ocena ocena, Enums.StatusKomentara status, bool isDeleted)
        {
            ManifestacijaID = manifestacijaID;
            Tekst = tekst;
            Ocena = ocena;
            Status = status;
            IsDeleted = isDeleted;
        }
    }
}