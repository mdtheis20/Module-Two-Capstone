using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public override string ToString()
        {
            return $"{Id,-4}{SiteId,-8}{Name,-40}{FromDate.ToShortDateString(),15}{ToDate.ToShortDateString(),15}";
        }
    }
}
