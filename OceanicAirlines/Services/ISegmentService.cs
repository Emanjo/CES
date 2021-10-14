using OceanicAirlines.Models;
using System.Collections.Generic;

namespace OceanicAirlines.Services
{
    public interface ISegmentService
    {
        IEnumerable<SegmentViewModel> GetInternalSegments(double? weight, double? depth, double? height, double? width, string type);
        IEnumerable<SegmentOwner> GetExternalSegments(double? weight, double? depth, double? height, double? width, string type);
    }
}