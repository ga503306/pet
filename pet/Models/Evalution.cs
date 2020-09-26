namespace pet.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Evalution")]
    public partial class Evalution
    {
        [Key]
        [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string evaseq { get; set; }

        [StringLength(20)]
        public string orderseq { get; set; }

        public int? star { get; set; }

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

        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
