namespace OceanicAirlines.Models
{
    public class ConnectedNode
    {
        public int Id { get; set; }
        public double Time { get; set; }
        public double Price { get; set; }



        public ConnectedNode(int id, double time, double price)
        {
            Id = id;
            Time = time;
            Price = price;
        }
    }
}
