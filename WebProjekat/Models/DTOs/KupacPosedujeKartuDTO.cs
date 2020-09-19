using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KupacPosedujeKartuDTO
    {
        public bool KorisnikJeKupac { get; set; }
        public bool KorisnikPosedujeKartuManifestacije { get; set; }

        public KupacPosedujeKartuDTO() { }

        public KupacPosedujeKartuDTO(bool korisnikJeKupac, bool korisnikPosedujeKartuManifestacije)
        {
            KorisnikJeKupac = korisnikJeKupac;
            KorisnikPosedujeKartuManifestacije = korisnikPosedujeKartuManifestacije;
        }
    }
}