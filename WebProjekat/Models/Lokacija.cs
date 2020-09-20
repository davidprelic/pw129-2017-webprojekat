using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebProjekat.Models
{
    public class Lokacija
    {
        public string Id { get; set; }
        public double GeografskaDuzina { get; set; }
        public double GeografskaSirina { get; set; }
        public MestoOdrzavanja MestoOdrzavanja { get; set; }  

        public Lokacija() { }

        public Lokacija(string id, string geografskaDuzina, string geografskaSirina, string ulica, string grad, string drzava, string postanskiBroj)
        {
            Id = id;
            GeografskaDuzina = double.Parse(geografskaDuzina);
            GeografskaSirina = double.Parse(geografskaSirina);
            MestoOdrzavanja = new MestoOdrzavanja(ulica, grad, drzava, int.Parse(postanskiBroj));
        }

        public override string ToString()
        {
            return $"{Id};{GeografskaDuzina.ToString()};{GeografskaSirina.ToString()};{MestoOdrzavanja.Ulica};{MestoOdrzavanja.Grad};{MestoOdrzavanja.Drzava};{MestoOdrzavanja.PostanskiBroj.ToString()}";
        }

        public string SacuvajLokaciju()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/lokacije.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(this.ToString());

            sw.Close();
            stream.Close();

            return Id;
        }


    }
}