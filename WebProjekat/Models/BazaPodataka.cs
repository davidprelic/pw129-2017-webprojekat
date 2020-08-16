using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class BazaPodataka
    {
        public Dictionary<string, Korisnik> listaKorisnika { get; set; }


        public BazaPodataka() { }

        public void UcitajKorisnike(string path)
        {
            path = HostingEnvironment.MapPath(path);
            listaKorisnika = new Dictionary<string, Korisnik>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            Korisnik k = null;

            while ((line = sr.ReadLine()) != null)
            {
                // PRAVILNO ISPARSIRATI SVA POLJA IZ TEKSTUALNE DATOTEKE

                string[] tokens = line.Split(';');
                if(tokens[6] == Enums.Uloga.PRODAVAC.ToString())
                    k = new Prodavac(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8]);
                else if (tokens[6] == Enums.Uloga.KUPAC.ToString())
                    k = new Kupac(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8], tokens[9], tokens[10]);
                else
                    k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7]);

                listaKorisnika.Add(k.KorisnickoIme, k);
            }
            sr.Close();
            stream.Close();
        }

        public void AzurirajKorisnike()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            foreach (var key in listaKorisnika.Keys)
            {
                sw.WriteLine(listaKorisnika[key].ToString());
            }

            sw.Close();
            stream.Close();
        }






    }
}