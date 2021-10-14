using OceanicAirlines.Models;
using System.Collections.Generic;

namespace OceanicAirlines.Services
{
    public interface IDijsktraAlgorithmService
    {
        RouteOverall RunRouteSearching(IEnumerable<SegmentOwner> segments, string startCity, string endCity, double balance);
    }
}