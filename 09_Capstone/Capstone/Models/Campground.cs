using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int Id { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public DateTime OpeningMonth { get; set; }
        public DateTime ClosingMonth { get; set; }
        public decimal DailyFee { get; set; }
    }
}
