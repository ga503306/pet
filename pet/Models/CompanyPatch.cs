using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class CompanyPatch
    {
        public string introduce { get; set; }
        public string morning { get; set; }
        public string afternoon { get; set; }
        public string night { get; set; }
        public string midnight { get; set; }

        public string bannerimg { get; set; }
    }
}