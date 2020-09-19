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
        public Dictionary<string, Manifestacija> listaManifestacija { get; set; }
        public Dictionary<string, Karta> listaKarata { get; set; }
        public Dictionary<string, Komentar> listaKomentara { get; set; }


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
                if(tokens[7] == Enums.Uloga.PRODAVAC.ToString())
                    k = new Prodavac(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8], tokens[9]);
                else if (tokens[7] == Enums.Uloga.KUPAC.ToString())
                    k = new Kupac(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8], tokens[9], tokens[10], tokens[11]);
                else
                    k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8]);

                listaKorisnika.Add(k.Id, k);
            }
            sr.Close();
            stream.Close();
        }

        public void UcitajManifestacije(string path)
        {
            path = HostingEnvironment.MapPath(path);
            listaManifestacija = new Dictionary<string, Manifestacija>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            Manifestacija m = null;

            while ((line = sr.ReadLine()) != null)
            {
                // PRAVILNO ISPARSIRATI SVA POLJA IZ TEKSTUALNE DATOTEKE

                string[] tokens = line.Split(';');
                m = new Manifestacija(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7], tokens[8], tokens[9], tokens[10], tokens[11], tokens[12]);
                
                listaManifestacija.Add(m.Id, m);
            }
            sr.Close();
            stream.Close();
        }

        public void UcitajKarte(string path)
        {
            path = HostingEnvironment.MapPath(path);
            listaKarata = new Dictionary<string, Karta>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            Karta k = null;

            while ((line = sr.ReadLine()) != null)
            {
                // PRAVILNO ISPARSIRATI SVA POLJA IZ TEKSTUALNE DATOTEKE

                string[] tokens = line.Split(';');
                k = new Karta(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6]);

                listaKarata.Add(k.Id, k);
            }
            sr.Close();
            stream.Close();
        }

        public void UcitajKomentare(string path)
        {
            path = HostingEnvironment.MapPath(path);
            listaKomentara = new Dictionary<string, Komentar>();
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            Komentar k = null;

            while ((line = sr.ReadLine()) != null)
            {
                // PRAVILNO ISPARSIRATI SVA POLJA IZ TEKSTUALNE DATOTEKE

                string[] tokens = line.Split(';');
                k = new Komentar(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6]);

                listaKomentara.Add(k.Id, k);
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

        public void AzurirajManifestacije()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/manifestacije.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            foreach (var key in listaManifestacija.Keys)
            {
                sw.WriteLine(listaManifestacija[key].ToString());
            }

            sw.Close();
            stream.Close();
        }

        public void AzurirajKarte()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/karte.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            foreach (var key in listaKarata.Keys)
            {
                sw.WriteLine(listaKarata[key].ToString());
            }

            sw.Close();
            stream.Close();
        }

        public void AzurirajKomentare()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            foreach (var key in listaKomentara.Keys)
            {
                sw.WriteLine(listaKomentara[key].ToString());
            }

            sw.Close();
            stream.Close();
        }
    }
}