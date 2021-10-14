using System.Collections.Generic;

namespace OceanicAirlines.Infrastructure.Data
{
    public interface ISupportedTypesDataService
    {
        IEnumerable<string> GetTypes();
    }
}