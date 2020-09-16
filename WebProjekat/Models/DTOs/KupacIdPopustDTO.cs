using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models.DTOs
{
    public class KupacIdPopustDTO
    {
        public string Id { get; set; }
        public int Popust { get; set; }

        public KupacIdPopustDTO() { }

        public KupacIdPopustDTO(string id, int popust)
        {
            Id = id;
            Popust = popust;
        }
    }
}