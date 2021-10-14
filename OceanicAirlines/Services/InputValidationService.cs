using OceanicAirlines.Infrastructure.Data;
using System.Linq;

namespace OceanicAirlines.Services
{
    public class InputValidationService : IInputValidationService
    {
        private readonly ISupportedTypesDataService _supportedTypesDataService;

        public InputValidationService(ISupportedTypesDataService supportedTypesDataService)
        {
            _supportedTypesDataService = supportedTypesDataService;
        }
   
        public bool IsInputValid(double? weight, double? depth, double? width, double? height, string type)
        {
            if (height is null ||
                weight is null ||
                width is null ||
                depth is null ||
                string.IsNullOrWhiteSpace(type))
            {
                return false;
            };

            if (weight > 20) return false;

            if (IsDimensionValid(weight.Value, depth.Value, width.Value, height.Value) &&
                IsTypeValid(type))
            {
                return true;
            }

            return false;
        }

        private bool IsTypeValid(string type)
        {
            var types = _supportedTypesDataService.GetTypes();

            var hasType = types.Any(t => t.ToLower() == type.ToLower());

            if (hasType) return true;

            return false;
        }

        private bool IsDimensionValid(double weight, double depth, double width, double height)
        {
            if (weight < 1) return IsNotExceedingSizeLimitation(2, height, depth, width);

            if (weight >= 1 && weight < 5) return IsNotExceedingSizeLimitation(2, height, depth, width);

            if (weight >= 5) return IsNotExceedingSizeLimitation(2, height, depth, width);

            return false;
        }

        private bool IsNotExceedingSizeLimitation(double maxSize, double height, double depth, double width)
        {
            if (height > maxSize) return false;
            if (depth > maxSize) return false;
            if (width > maxSize) return false;

            return true;
        }
    }
}
