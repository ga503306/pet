namespace pet.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company")]
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            Room = new HashSet<Room>();
        }

        [Key]
        [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string companyseq { get; set; }

        [Required]
        [StringLength(50)]
        public string companyname { get; set; }

        [Required]
        [StringLength(50)]
        public string companybrand { get; set; }

        [Required]
        [StringLength(10)]
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

        [StringLength(10)]
        public string country { get; set; }

        [StringLength(10)]
        public string area { get; set; }

        [StringLength(100)]
        public string address { get; set; }

        [StringLength(20)]
        public string pblicense { get; set; }

        public DateTime? effectivedate { get; set; }

        public bool? state { get; set; }

        public string avatar { get; set; }

        public string bannerimg { get; set; }

        [StringLength(1)]
        public string del_flag { get; set; }

        public string introduce { get; set; }

        public bool? morning { get; set; }

        public bool? afternoon { get; set; }

        public bool? night { get; set; }

        public bool? midnight { get; set; }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Room { get; set; }
    }
}
