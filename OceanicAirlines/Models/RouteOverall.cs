using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines.Models
{
    public class RouteOverall
    {
        public double Cost { get; set; }
        public double Time { get; set; }
        public List<Route> Routes { get; set; }
        public RouteOverall(double cost, double time, List<Route> routes)
        {
            Cost = cost;
            Time = time;
            Routes = routes;
        }
    }
}
