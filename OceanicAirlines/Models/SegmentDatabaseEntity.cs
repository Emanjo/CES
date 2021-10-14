namespace OceanicAirlines.Models
{
    public class SegmentDatabaseEntity
    {
        public City StartCity { get; }
        public City EndCity { get; }

        public SegmentDatabaseEntity(City startCity, City endCity)
        {
            StartCity = startCity;
            EndCity = endCity;
        }
    }
}
