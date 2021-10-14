using OceanicAirlines.Enums;
using OceanicAirlines.Models;
using System.Collections.Generic;
using System.Linq;

namespace OceanicAirlines.Services
{
    public class SegmentService : ISegmentService
    {
        private readonly IDataService _dataService;
        private readonly IIntegrationApiClient _integrationApiClient;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IInputValidationService _inputValidationService;

        public SegmentService(IDataService dataService, IIntegrationApiClient integrationApiClient,
            IPriceCalculationService priceCalculationService,
            IInputValidationService inputValidationService)
        {
            _dataService = dataService;
            _integrationApiClient = integrationApiClient;
            _priceCalculationService = priceCalculationService;
            _inputValidationService = inputValidationService;
        }

        public IEnumerable<SegmentOwner> GetAllSegments(double? weight, double? depth, double? height, double? width, string type)
        {
            var oceanicSegments = GetInternalSegments(weight, depth, height, width, type)
                .Select(s => new SegmentOwner { Owner = "Oceanic Airlines", Segment = s });

            var telstarSegments = _integrationApiClient.GetSegments(Company.Telestar, height.Value, depth.Value, width.Value, weight.Value, type)
                .Select(s => new SegmentOwner { Owner = "Telestar", Segment = s });


            var eastIndiaSegments = _integrationApiClient.GetSegments(Company.EastIndia, height.Value, depth.Value, width.Value, weight.Value, type)
                .Select(s => new SegmentOwner { Owner = "East India", Segment = s });

            var result = oceanicSegments.Concat(telstarSegments).Concat(eastIndiaSegments);

            return result;
        }

        public IEnumerable<SegmentViewModel> GetInternalSegments(double? weight, double? depth, double? height, double? width, string type)
        {

            var isInputValid = _inputValidationService.IsInputValid(weight, depth, width, height, type);

            if (!isInputValid) return new List<SegmentViewModel>();

            // Retrieve Oceanic segments
            List<SegmentDatabaseEntity> oceanicSegments = _dataService.GetSegments();
            List<SegmentViewModel> returnSegments = new List<SegmentViewModel>();
            foreach (SegmentDatabaseEntity segment in oceanicSegments)
            {
                returnSegments.Add(new SegmentViewModel(
                    segment.StartCity.Name,
                    segment.EndCity.Name,
                    _priceCalculationService.GetPrice(height.Value, width.Value, depth.Value, weight.Value),
                    8,
                    20
                    ));
            }

            return returnSegments;
        }
    }
}
