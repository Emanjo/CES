using OceanicAirlines.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OceanicAirlines.Services
{
    public class DijsktraAlgorithmService : IDijsktraAlgorithmService
    {
        List<AlgorithmNode> NodeList;
        List<int> ShortestPath;
        private readonly IDataService _dataService;

        public DijsktraAlgorithmService(IDataService dataService)
        {
            NodeList = new();
            _dataService = dataService;
        }

        public List<int> RunRouteSearching(IEnumerable<SegmentOwner> segments)
        {
            int originCity = 0;
            int destinationCity = 7;
            int balance = 1;
            CreateFakeData();
            FetchData(segments);
            DijsktraAlgorithm(originCity, destinationCity, balance);
            ShortestPath = new();
            ShortestPath.Add(destinationCity);
            CalculateShortestPath(ShortestPath, NodeList.ElementAt(destinationCity));
            ShortestPath.Reverse();
            return ShortestPath;
        }

        private void DijsktraAlgorithm(int OriginCity, int DestinationCity, int Balance)
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

        private void CalculateShortestPath(List<int> ShortestPath, AlgorithmNode Node)
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
            NodeList.Add(new AlgorithmNode(0, "Kair", Double.PositiveInfinity));
            NodeList[0].Connections.Add(new ConnectedNode(1, 1, 1));
            NodeList[0].Connections.Add(new ConnectedNode(2, 2, 2));
            NodeList.Add(new AlgorithmNode(1, "Kapstad", Double.PositiveInfinity));
            NodeList[1].Connections.Add(new ConnectedNode(0, 1, 1));
            NodeList[1].Connections.Add(new ConnectedNode(5, 5, 5));
            NodeList.Add(new AlgorithmNode(2, "Rome", Double.PositiveInfinity));
            NodeList[2].Connections.Add(new ConnectedNode(4, 5, 5));
            NodeList[2].Connections.Add(new ConnectedNode(6, 2, 2));
            NodeList[2].Connections.Add(new ConnectedNode(0, 2, 2));
            NodeList.Add(new AlgorithmNode(3, "Msocow", Double.PositiveInfinity));
            NodeList[3].Connections.Add(new ConnectedNode(4, 1, 1));
            NodeList[3].Connections.Add(new ConnectedNode(6, 3, 3));
            NodeList[3].Connections.Add(new ConnectedNode(7, 4, 4));
            NodeList.Add(new AlgorithmNode(4, "London", Double.PositiveInfinity));
            NodeList[4].Connections.Add(new ConnectedNode(2, 5, 5));
            NodeList[4].Connections.Add(new ConnectedNode(3, 1, 1));
            NodeList.Add(new AlgorithmNode(5, "A", Double.PositiveInfinity));
            NodeList[5].Connections.Add(new ConnectedNode(1, 5, 5));
            NodeList[5].Connections.Add(new ConnectedNode(6, 8, 8));
            NodeList.Add(new AlgorithmNode(6, "B", Double.PositiveInfinity));
            NodeList[6].Connections.Add(new ConnectedNode(5, 8, 8));
            NodeList[6].Connections.Add(new ConnectedNode(2, 2, 2));
            NodeList[6].Connections.Add(new ConnectedNode(3, 3, 3));
            NodeList[6].Connections.Add(new ConnectedNode(7, 8, 8));
            NodeList.Add(new AlgorithmNode(7, "C", Double.PositiveInfinity));
            NodeList[7].Connections.Add(new ConnectedNode(3, 4, 4));
            NodeList[7].Connections.Add(new ConnectedNode(6, 8, 8));
        }

        private void FetchData(IEnumerable<SegmentOwner> segments)
        {
            foreach (City c in _dataService.GetCities())
            {
                NodeList.Add(new AlgorithmNode(c.Id, c.Name, Double.PositiveInfinity));
            }
            foreach (AlgorithmNode aNode in NodeList)
            {
                foreach (SegmentOwner sOwner in segments)
                {
                    if (aNode.Equals(sOwner.Segment.StartCity))
                    {
                        aNode.Connections.Add(new ConnectedNode(GetCityId(sOwner.Segment.EndCity), sOwner.Segment.Time, sOwner.Segment.Cost));
                    }
                    if (aNode.Equals(sOwner.Segment.EndCity))
                    {
                        aNode.Connections.Add(new ConnectedNode(GetCityId(sOwner.Segment.StartCity), sOwner.Segment.Time, sOwner.Segment.Cost));
                    }
                }
            }
        }
        private int GetCityId(String CityName)
        {
            foreach (City c in _dataService.GetCities())
            {
                if (CityName.Equals(c.Name))
                {
                    return c.Id;
                }
            }
            return - 1;
        }
    }
}
