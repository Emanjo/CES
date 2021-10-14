using OceanicAirlines.Enums;
using OceanicAirlines.Models;
using System;
using System.Collections.Generic;

namespace OceanicAirlines.Services
{
    public interface IIntegrationApiClient
    {
        IEnumerable<Segment> GetSegments(Company company, double heigth, double depth, double width, double weigth, string type);
    }
}