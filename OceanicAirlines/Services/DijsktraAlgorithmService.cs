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

        public RouteOverall RunRouteSearching(IEnumerable<SegmentOwner> supportedSegments, string startCity, string endCity, double balance)
        {
            int originCity = GetCityId(startCity);
            int destinationCity= GetCityId(endCity);                        
            FetchData(supportedSegments);
            DijsktraAlgorithm(originCity, destinationCity, balance);
            ShortestPath = new();
            ShortestPath.Add(destinationCity);
            CalculateShortestPath(ShortestPath, NodeList.ElementAt(destinationCity-1));
            ShortestPath.Reverse();
            double[] result = CalculatePriceAndTime(ShortestPath);            
            return new RouteOverall(result[0],result[1], TranslateCityIdToName(ShortestPath, supportedSegments));
        }

        private void DijsktraAlgorithm(int OriginCity, int DestinationCity, double Balance)
        {
            NodeList.ElementAt(OriginCity-1).CostToStart = 0;
            List<AlgorithmNode> VisitingQueue = new();
            VisitingQueue.Add(NodeList.ElementAt(OriginCity-1));
            while (VisitingQueue.Count != 0)
            {
                VisitingQueue = VisitingQueue.OrderBy(node => node.CostToStart).ToList();
                var node = VisitingQueue.First();
                foreach (var cnn in node.Connections)
                {
                    var childNode = NodeList.ElementAt(cnn.Id-1);
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
            CalculateShortestPath(ShortestPath, NodeList.ElementAt(Node.NearestCityId-1));
        }

        private double[] CalculatePriceAndTime(List<int> Path)
        {
            double[] ReturnDouble = new double[2] { 0.0, 0.0 };
            for (int i = 0; i < Path.Count-1; ++i)
            {
                var connections = NodeList.ElementAt(Path[i]-1).Connections;
                var doBreak = false;
                foreach (ConnectedNode cNode in connections)
                {
                    double CurrentPrice = 0;
                    double CurrentTime = 0;
                    for (int j = i+1; j < Path.Count; ++j )
                    {
                        if (cNode.Id == Path[j])
                        {
                            CurrentPrice = cNode.Price;
                            CurrentTime = cNode.Time;
                            doBreak = true;
                            break;
                        }
                    }
                    ReturnDouble[0] += CurrentPrice;
                    ReturnDouble[1] += CurrentTime;
                    if (doBreak)
                        break;
                }
            }
            return ReturnDouble;
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
                    if (aNode.Name.Equals(sOwner.Segment.StartCity))
                    {
                        var id = GetCityId(sOwner.Segment.EndCity);
                        if (id > 0)
                            aNode.Connections.Add(new ConnectedNode(id, sOwner.Segment.Time, sOwner.Segment.Cost));
                    }
                    if (aNode.Name.Equals(sOwner.Segment.EndCity))
                    {
                        var id = GetCityId(sOwner.Segment.StartCity);
                        if (id > 0)
                            aNode.Connections.Add(new ConnectedNode(GetCityId(sOwner.Segment.StartCity), sOwner.Segment.Time, sOwner.Segment.Cost));
                    }
                }
            }
        }
        private int GetCityId(String CityName)
        {
            var cities = _dataService.GetCities();
            foreach (City c in cities)
            {
                if (CityName.Equals(c.Name))
                {
                    return c.Id;
                }
            }
            return -1;
        }

        private String GetCityName(int CityId)
        {
            foreach (City c in _dataService.GetCities())
            {
                if (CityId == c.Id)
                {
                    return c.Name;
                }
            }
            return null;
        }

        private String GetSegmentOwenr(int originId, int endId, IEnumerable<SegmentOwner> Segments)
        {
            foreach(SegmentOwner sOwner in Segments)
            {
                if ((sOwner.Segment.StartCity.Equals(GetCityName(originId)) && sOwner.Segment.EndCity.Equals(GetCityName(endId)))
                    || (sOwner.Segment.StartCity.Equals(GetCityName(endId)) && sOwner.Segment.EndCity.Equals(GetCityName(originId))))
                    return sOwner.Owner;
            }
            return null;           
        }

        private List<Route> TranslateCityIdToName(List<int> ShortestPath, IEnumerable<SegmentOwner> Segments)
        {
            List<Route> RoutesToReturn = new();
            for(int i = 1; i < ShortestPath.Count; i++)
            {
                Route Route1 = new();
                Route1.StartCity = GetCityName(ShortestPath[i - 1]);
                Route1.EndCity = GetCityName(ShortestPath[i]);
                Route1.Owner = GetSegmentOwenr(ShortestPath[i - 1], ShortestPath[i], Segments);
                RoutesToReturn.Add(Route1);
            }
            return RoutesToReturn;
        }
    }
}
