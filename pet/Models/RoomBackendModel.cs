using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class RoomBackendModel //後台顯示list
    {
        public string companyseq { get; set; }

        public string roomseq { get; set; }

        public string roomname { get; set; }

        public bool state { get; set; }

        public string pettype { get; set; }

    }
}