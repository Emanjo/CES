using System.Collections.Generic;

namespace OceanicAirlines.Infrastructure.Data
{
    public class SupportedTypesDataService : ISupportedTypesDataService
    {
        public IEnumerable<string> GetTypes()
        {
            return new string[]
            {
                "Weapons",
                "Other",
                "Toys",
                "Food"
            };
        }
    }
}
