using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class MemberRegisterModel
    {
        [Required]
        [MaxLength(50)]
        public string membername { get; set; }

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


        public string avatar { get; set; }
    }
}