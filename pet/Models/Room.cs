namespace pet.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [Key]
        [StringLength(20)]
        public string roomseq { get; set; }

        [StringLength(50)]
        public string roomname { get; set; }

        [Required]
        [StringLength(20)]
        public string companyseq { get; set; }

        public string introduce { get; set; }

        public bool? pettype_cat { get; set; }

        public bool? pettype_dog { get; set; }

        public bool? pettype_other { get; set; }

        public int? petsizes { get; set; }

        public int? petsizee { get; set; }

        public int? roomamount { get; set; }

        public int? roomprice { get; set; }

        public int? roomamount_amt { get; set; }

        public int? walk { get; set; }

        public bool? canned { get; set; }

        public bool? feed { get; set; }

        public bool? catlitter { get; set; }

        public int? visit { get; set; }

        public bool? medicine_infeed { get; set; }

        public int? medicine_infeed_amt { get; set; }

        public bool? medicine_pill { get; set; }

        public int? medicine_pill_amt { get; set; }

        public bool? medicine_paste { get; set; }

        public int? medicine_paste_amt { get; set; }

        public bool? bath { get; set; }

        public int? bath_amt { get; set; }

        public bool? hair { get; set; }

        public int? hair_amt { get; set; }

        public bool? nails { get; set; }

        public int? nails_amt { get; set; }

        public bool? state { get; set; }

        [Required]
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

        public string img1 { get; set; }

        public string img2 { get; set; }

        public string img3 { get; set; }

        public string img4 { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}
