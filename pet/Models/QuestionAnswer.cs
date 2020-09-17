namespace pet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestionAnswer")]
    public partial class QuestionAnswer
    {
        [Key]
        [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ansseq { get; set; }

        [StringLength(20)]
        public string queseq { get; set; }

        [StringLength(20)]
        public string memberseq { get; set; }

        [StringLength(20)]
        public string companyseq { get; set; }

        public string message { get; set; }

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

        public virtual Question Question { get; set; }
    }
}
