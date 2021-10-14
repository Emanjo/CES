using Microsoft.AspNetCore.Mvc;
using OceanicAirlines.Models;
using OceanicAirlines.Services;
using System.Collections.Generic;

namespace OceanicAirlines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IInputValidationService _inputValidationService;
        private readonly IDataService _dataService;
        private readonly IPriceCalculationService _priceCalculationService;

        public RoutesController(IInputValidationService inputValidationService,
            IDataService dataService,
            IPriceCalculationService priceCalculationService)
        {
            _inputValidationService = inputValidationService;
            _dataService = dataService;
            _priceCalculationService = priceCalculationService;
        }

        [HttpGet]
        public List<SegmentViewModel> Get([FromQuery] double? weight, double? height, double? width, double? depth, string type)
        {
            var isInputValid = _inputValidationService.IsInputValid(weight, depth, width, height, type);

            if(!isInputValid) return new List<SegmentViewModel>();

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
