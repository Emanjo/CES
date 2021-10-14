namespace OceanicAirlines.Models
{
    public class SegmentViewModel
    {
        public SegmentViewModel(string startCity, string endCity, double cost, double time, double maxWeight)
        {
            StartCity = startCity;
            EndCity = endCity;
            Cost = cost;
            Time = time;
            MaxWeight = maxWeight;
        }

        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public double Cost { get; set; }
        public double Time { get; set; }
        public double MaxWeight { get; set; }
    }
}