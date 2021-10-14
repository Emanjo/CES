namespace OceanicAirlines.Services
{
    public interface IInputValidationService
    {
        bool IsInputValid(double? weight, double? depth, double? width, double? heigth, string type);
    }
}