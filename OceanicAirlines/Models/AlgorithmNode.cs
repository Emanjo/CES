using System;
using System.Collections.Generic;

namespace OceanicAirlines.Models
{
    public class AlgorithmNode
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public int NearestCityId { get; set; }
        public double CostToStart { get; set; }
        public bool Visited { get; set; }
        public List<ConnectedNode> Connections;
        public AlgorithmNode(AlgorithmNode nearest, int cityId, string name, double costToStart)
        {
            NearestCityId = -1;
            CityId = cityId;
            Name = name;
            CostToStart = costToStart;
            Visited = false;
            Connections = new();
        }
        public AlgorithmNode() { }
    }
}
