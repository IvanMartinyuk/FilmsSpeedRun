using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsSpeedRunAPI
{
    public class FilterOptions
    {
        public int Page { get; set; }
        public string Prop { get; set; } = "base";
        public double ImdbRatingTop { get; set; } = 0;
        public double ImdbRatingLast { get; set; } = 10;
        public double LocalRatingTop { get; set; } = 0;
        public double LocalRatingLast { get; set; } = 10;
        public DateTime DateTop { get; set; } = new DateTime(1800, 1, 1);
        public DateTime DateLast { get; set; } = DateTime.Now;
        public string Genre { get; set; } = "all";
        public string SortBy { get; set; }
    }
}

