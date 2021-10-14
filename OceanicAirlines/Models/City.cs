namespace OceanicAirlines.Models
{
    public class City
    {
        public int Id { set; get; }
        public string Name { set;  get; }
        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
