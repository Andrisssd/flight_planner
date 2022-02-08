using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanner2.Models
{
    public class SearchFlightRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Date { get; set; }
    }
}
