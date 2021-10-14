namespace OceanicAirlines.Models
{
    public class SegmentModel
    {
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public double Cost { get; set; }
        public double Time { get; set; }
        public double MaxWeigth => 20;
    }
}