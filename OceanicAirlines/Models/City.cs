namespace OceanicAirlines.Models
{
    public class City
    {
        private int Id;
        private string Name { get; }
        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
