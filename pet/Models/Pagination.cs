using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public class Pagination
    {
        public int total { get; set; } //總比

        public int count { get; set; } //這頁有幾筆

        public int per_page { get; set; } //每一頁有幾筆

        public int current_page { get; set; } //現在在第幾頁

        public int total_page { get; set; } //總共有幾頁

        public List<string> links { get; set; }
    }
}