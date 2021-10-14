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

        public RoutesController(IInputValidationService inputValidationService)
        {
            _inputValidationService = inputValidationService;
        }

        [HttpGet]
        public List<SegmentViewModel> Get([FromQuery] double? weight, double? height, double? width, double? depth, string type)
        {
            var isInputValid = _inputValidationService.IsInputValid(weight, depth, width, height, type);

            if(!isInputValid) return new List<SegmentViewModel>();

            return new List<SegmentViewModel>
            {
                new SegmentViewModel {
                    EndCity = "Fredrikstad",
                    StartCity = "Oslo",
                    Cost = 20,
                    Time = 2,
                    MaxWeight = 20
                },
                new SegmentViewModel {
                    EndCity = "Copenhagen",
                    StartCity = "Helsingborg",
                    Cost = 50,
                    Time = 2,
                    MaxWeight = 20
                }
            };
        }
    }
}
