using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int Id { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpeningMonth { get; set; }
        public string PrintOpeningMonth
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(OpeningMonth);
            }
        }
        public int ClosingMonth { get; set; }
        public string PrintClosingMonth
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(ClosingMonth);
            }
        }
        public decimal DailyFee { get; set; }
    }
}
