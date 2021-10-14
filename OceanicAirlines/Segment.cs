using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines
{
    public class Segment
    {
        private City startCity { get; }
        private City endCity { get; }
        public Segment(City startCity, City endCity)
        {
            this.startCity = startCity;
            this.endCity = endCity;
        }
    }
}
