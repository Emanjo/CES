using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines.Services
{
    public class RouteFindingAlgoritm
    {
        public class Dijsktra
        {
            List<AlgorithmNode> NodeList;
            List<int> ShortestPath;

            public Dijsktra()
            {
                NodeList = new();
            }

            public List<int> RunRouteSearching()
            {
                int originCity = 0;
                int destinationCity = 7;
                int balance = 1;
                CreateFakeData();
                DijsktraAlgorithm(originCity, destinationCity, balance);
                ShortestPath = new();
                ShortestPath.Add(destinationCity);
                CalculateShortestPath(ShortestPath, NodeList.ElementAt(destinationCity));
                ShortestPath.Reverse();
                return ShortestPath;
            }



            public void DijsktraAlgorithm(int OriginCity, int DestinationCity, int Balance)
            {
                NodeList.ElementAt(OriginCity).CostToStart = 0;
                List<AlgorithmNode> VisitingQueue = new();
                VisitingQueue.Add(NodeList.ElementAt(OriginCity));
                while (VisitingQueue.Count != 0)
                {
                    VisitingQueue = VisitingQueue.OrderBy(node => node.CostToStart).ToList();
                    var node = VisitingQueue.First();
                    foreach (var cnn in node.Connections)//.OrderBy(node => node.Price))
                    {
                        var childNode = NodeList.ElementAt(cnn.Id);
                        if (childNode.Visited)
                            continue;
                        if (node.CostToStart + (cnn.Time * (1 - Balance) + cnn.Price * (Balance)) < childNode.CostToStart)
                        {
                            childNode.CostToStart = node.CostToStart + (cnn.Time * (1 - Balance) + cnn.Price * (Balance));
                            childNode.NearestCityId = node.CityId;
                            if (!VisitingQueue.Contains(childNode))
                                VisitingQueue.Add(childNode);
                        }
                    }
                    VisitingQueue.Remove(node);
                    node.Visited = true;
                    if (node.CityId == DestinationCity)
                        return;
                }
            }

            public void CalculateShortestPath(List<int> ShortestPath, AlgorithmNode Node)
            {
                if (Node.NearestCityId == -1)
                    return;
                ShortestPath.Add(Node.NearestCityId);
                CalculateShortestPath(ShortestPath, NodeList.ElementAt(Node.NearestCityId));
            }
            public double[] CalculatePriceAndTime(List<int> Path)
            {
                double[] ReturnDouble = new double[2] { 0.0, 0.0 };
                for (int i = 1; i < Path.Count; ++i)
                {
                    double CurrentPrice = -1.0;
                    double CurrentTime = -1.0;
                    foreach (ConnectedNode cNode in NodeList.ElementAt(Path[i]).Connections)
                    {
                        if (cNode.Id == Path[i - 1])
                        {
                            CurrentPrice = cNode.Price;
                            CurrentTime = cNode.Time;
                        }
                    }
                    ReturnDouble[0] += CurrentPrice;
                    ReturnDouble[1] += CurrentTime;
                }
                return ReturnDouble;
            }

            private void CreateFakeData()
            {
                NodeList.Add(new AlgorithmNode(null, 0, "Kair", Double.PositiveInfinity));
                NodeList[0].Connections.Add(new ConnectedNode(1, 1, 1));
                NodeList[0].Connections.Add(new ConnectedNode(2, 2, 2));
                NodeList.Add(new AlgorithmNode(null, 1, "Kapstad", Double.PositiveInfinity));
                NodeList[1].Connections.Add(new ConnectedNode(0, 1, 1));
                NodeList[1].Connections.Add(new ConnectedNode(5, 5, 5));
                NodeList.Add(new AlgorithmNode(null, 2, "Rome", Double.PositiveInfinity));
                NodeList[2].Connections.Add(new ConnectedNode(4, 5, 5));
                NodeList[2].Connections.Add(new ConnectedNode(6, 2, 2));
                NodeList[2].Connections.Add(new ConnectedNode(0, 2, 2));
                NodeList.Add(new AlgorithmNode(null, 3, "Msocow", Double.PositiveInfinity));
                NodeList[3].Connections.Add(new ConnectedNode(4, 1, 1));
                NodeList[3].Connections.Add(new ConnectedNode(6, 3, 3));
                NodeList[3].Connections.Add(new ConnectedNode(7, 4, 4));
                NodeList.Add(new AlgorithmNode(null, 4, "London", Double.PositiveInfinity));
                NodeList[4].Connections.Add(new ConnectedNode(2, 5, 5));
                NodeList[4].Connections.Add(new ConnectedNode(3, 1, 1));
                NodeList.Add(new AlgorithmNode(null, 5, "A", Double.PositiveInfinity));
                NodeList[5].Connections.Add(new ConnectedNode(1, 5, 5));
                NodeList[5].Connections.Add(new ConnectedNode(6, 8, 8));
                NodeList.Add(new AlgorithmNode(null, 6, "B", Double.PositiveInfinity));
                NodeList[6].Connections.Add(new ConnectedNode(5, 8, 8));
                NodeList[6].Connections.Add(new ConnectedNode(2, 2, 2));
                NodeList[6].Connections.Add(new ConnectedNode(3, 3, 3));
                NodeList[6].Connections.Add(new ConnectedNode(7, 8, 8));
                NodeList.Add(new AlgorithmNode(null, 7, "C", Double.PositiveInfinity));
                NodeList[7].Connections.Add(new ConnectedNode(3, 4, 4));
                NodeList[7].Connections.Add(new ConnectedNode(6, 8, 8));
            }
        }

        public class AlgorithmNode
        {
            public int CityId { get; set; }
            public String Name { get; set; }
            public int NearestCityId { get; set; }
            public double CostToStart { get; set; }
            public bool Visited { get; set; }
            public List<ConnectedNode> Connections;
            public AlgorithmNode(AlgorithmNode nearest, int cityId, String name, double costToStart)
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
}
