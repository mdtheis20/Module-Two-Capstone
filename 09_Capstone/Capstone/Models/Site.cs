using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int Id { get; set; }
        public int CampgroundId { get; set; }
        public string CampgroundName { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessible { get; set; }
        public string PrintIsAccessible
        {
            get
            {
                if (IsAccessible)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public int MaxRVLength { get; set; }
        public bool HasUtilities { get; set; }
        public string PrintHasUtilities
        {
            get
            {
                if (HasUtilities)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public decimal Cost { get; set; }
    }
}
