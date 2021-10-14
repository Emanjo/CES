using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines
{
    public class City
    {
        private int ID;
        private string name { get; }
        public City(int ID, string name)
        {
            this.ID = ID;
            this.name = name;
        }
    }
}
