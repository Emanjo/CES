namespace OceanicAirlines.Models
{
    public class City
    {
        public int Id { get; }
        public string Name { get; }
        public string DanishName { get; }
        public City(int id, string name, string danishName)
        {
            Id = id;
            Name = name;
            DanishName = danishName;
        }
    }
}
