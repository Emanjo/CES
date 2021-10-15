using OceanicAirlines.Models;
using System.Collections.Generic;

namespace OceanicAirlines
{
    public interface IDataService
    {
        List<City> GetCities();
        List<SegmentDatabaseEntity> GetSegments();
        int GetUserID(string email);
        string GetPasswordHash(string email);
        bool AddOrder(
            int lastLocation,
            string route,
            int userID,
            double weight,
            double width,
            double height,
            double depth,
            double price,
            double time,
            string category);
        string GetDanishCity(string city);
    }
}