using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class RoomCompanyModel
    {
        public string companyseq { get; set; }

        public string companybrand { get; set; }

        public string avatar { get; set; }

        public string country { get; set; }

        public string area { get; set; }

        public string address { get; set; }

        public string pettype { get; set; }

        public int? roomprice_min { get; set; }

        public int? roomprice_max { get; set; }

        public int? rooms { get; set; }
    }
}