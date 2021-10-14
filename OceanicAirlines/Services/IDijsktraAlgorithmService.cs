using System.Collections.Generic;

namespace OceanicAirlines.Services
{
    public interface IDijsktraAlgorithmService
    {
        double[] CalculatePriceAndTime(List<int> Path);
        List<int> RunRouteSearching();
    }
}