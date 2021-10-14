namespace OceanicAirlines.Services
{
    public class InputValidationService : IInputValidationService
    {
   
        public bool IsInputValid(double? weight, double? depth, double? width, double? heigth, string type)
        {
            if (heigth is null ||
                weight is null ||
                width is null ||
                depth is null ||
                string.IsNullOrWhiteSpace(type))
            {
                return false;
            };

            if (weight > 20) return false;

            if (IsDimensionValid(weight.Value, depth.Value, width.Value, heigth.Value)) return true;

            return false;
        }

        private bool IsDimensionValid(double weight, double depth, double width, double heigth)
        {
            if (weight < 1) return IsNotExceedingSizeLimitation(0.25, heigth, depth, width);

            if (weight >= 1 && weight <= 5) return IsNotExceedingSizeLimitation(0.4, heigth, depth, width);

            if (weight > 5) return IsNotExceedingSizeLimitation(2, heigth, depth, width);

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
