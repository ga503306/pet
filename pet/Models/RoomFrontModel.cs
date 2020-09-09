using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class RoomFrontModel
    {
        public string roomseq { get; set; }

        public string roomname { get; set; }

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

        public bool canned { get; set; }

        public bool feed { get; set; }

        public bool catlitter { get; set; }

        public int visit { get; set; }

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

        public string img1 { get; set; }

        public string img2 { get; set; }

        public string img3 { get; set; }

        public string img4 { get; set; }
    }
}