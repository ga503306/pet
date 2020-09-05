using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class CompanyLoginModel
    {
        [Required]
        [MaxLength(200)]
        public string email { get; set; }
        [Required]
        [MaxLength(50)]
        public string pwd { get; set; }
    }
}