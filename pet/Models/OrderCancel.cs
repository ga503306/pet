namespace pet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderCancel")]
    public partial class OrderCancel
    {
        [Key]
        [StringLength(20)]
        public string ocseq { get; set; }

        [StringLength(20)]
        public string orderseq { get; set; }

        public string memo { get; set; }

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

        public virtual Order Order { get; set; }
    }
}
