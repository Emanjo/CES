namespace OceanicAirlines.Models
{
    public class City
    {
        public int Id;
        public string Name { get; }
        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
