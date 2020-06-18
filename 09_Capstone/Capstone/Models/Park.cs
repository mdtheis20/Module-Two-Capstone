using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return $@"{Name} National Park
Location:           {Location}
Established:        {EstablishDate}
Area:               {Area:N0} sq km
Annual Visitors:    {Visitors:N0}

{Description}";
        }
    }
}
