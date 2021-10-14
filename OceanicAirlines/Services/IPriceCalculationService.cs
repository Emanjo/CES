namespace OceanicAirlines.Services
{
    public interface IPriceCalculationService
    {
        double GetPrice(double height, double width, double depth, double weight);
    }
}