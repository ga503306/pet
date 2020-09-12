namespace pet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Evalution = new HashSet<Evalution>();
            OrderCancel = new HashSet<OrderCancel>();
        }

        [Key]
        [StringLength(20)]
        public string orderseq { get; set; }

        [Required]
        [StringLength(20)]
        public string companyseq { get; set; }

        [Required]
        [StringLength(50)]
        public string companyname { get; set; }

        [Required]
        [StringLength(20)]
        public string roomseq { get; set; }

        [Required]
        [StringLength(50)]
        public string roomname { get; set; }

        [StringLength(10)]
        public string country { get; set; }

        [StringLength(10)]
        public string area { get; set; }

        [StringLength(100)]
        public string address { get; set; }

        [StringLength(10)]
        public string name { get; set; }

        [StringLength(10)]
        public string tel { get; set; }

        [StringLength(20)]
        public string pettype { get; set; }

        public int? petsize { get; set; }

        public int? petamount { get; set; }

        public int? roomprice { get; set; }

        public int? roomamount_amt { get; set; }

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

        public int? state { get; set; }

        public int? amt { get; set; }

        public DateTime? orderdates { get; set; }

        public DateTime? orderdatee { get; set; }

        public string memo { get; set; }

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

        [StringLength(20)]
        public string memberseq { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evalution> Evalution { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCancel> OrderCancel { get; set; }
    }
}
