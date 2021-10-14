using OceanicAirlines.Models;
using System.Collections.Generic;

namespace OceanicAirlines
{
    public interface IDataService
    {
        List<City> GetCities();
        List<SegmentDatabaseEntity> GetSegments();
    }
}