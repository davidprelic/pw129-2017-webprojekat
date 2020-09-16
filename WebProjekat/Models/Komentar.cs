using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Komentar
    {
        public string Id { get; set; }
        public string KupacID { get; set; }
        public string ManifestacijaID { get; set; }
        public string Tekst { get; set; }
        public Enums.Ocena Ocena { get; set; }

        public Komentar() { }

        public Komentar(string id, string kupacID, string manifestacijaID, string tekst, string ocena)
        {
            Id = id;
            KupacID = kupacID;
            ManifestacijaID = manifestacijaID;
            Tekst = tekst;
            Enums.Ocena oc;
            Enum.TryParse(ocena, out oc);
            Ocena = oc;
        }

        public override string ToString()
        {
            return $"{Id};{KupacID};{ManifestacijaID};{Tekst};{Ocena.ToString()}";
        }

        public string SacuvajKomentar()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(this.ToString());

            sw.Close();
            stream.Close();

            return Id;
        }
    }
}