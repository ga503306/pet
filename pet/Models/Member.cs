namespace pet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        [Key]
        [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string memberseq { get; set; }

        [Required]
        [StringLength(20)]
        public string membername { get; set; }

        [StringLength(20)]
        public string phone { get; set; }

        [Required]
        [StringLength(200)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string pwd { get; set; }

        [Required]
        [StringLength(200)]
        public string pwdsalt { get; set; }

        public string avatar { get; set; }

        [StringLength(1)]
        public string del_flag { get; set; }

        [StringLength(20)]
        public string postseq { get; set; }

        [StringLength(20)]
        public string postname { get; set; }

        public DateTime? postday { get; set; }

        [StringLength(20)]
        public string updateseq { get; set; }

        [StringLength(20)]
        public string updatename { get; set; }

        public DateTime? updateday { get; set; }
    }
}
