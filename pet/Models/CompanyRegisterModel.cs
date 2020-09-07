using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class CompanyRegisterModel
    {
        [Required]
        [MaxLength(50)]
        public string companyname { get; set; }

        [Required]
        [MaxLength(50)]
        public string companybrand { get; set; }

        [Required]
        [MaxLength(10)]
        public string phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(200)]
        public string email { get; set; }

        [MaxLength(50)]
        [Required]
        [DataType(DataType.Password)]
        public string pwd { get; set; }

        //[MaxLength(200)]
        //public string pwdsalt { get; set; }

        [Required]
        [MaxLength(10)]
        public string country { get; set; }

        [Required]
        [MaxLength(10)]
        public string area { get; set; }

        [MaxLength(100)]
        public string address { get; set; }

        [Required]
        [MaxLength(20)]
        public string pblicense { get; set; }

        [Required]
        public DateTime effectivedate { get; set; }

        public string avatar { get; set; }


    }
}