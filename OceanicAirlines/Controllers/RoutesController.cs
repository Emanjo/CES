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
        public List<Segment> Get([FromQuery] double? weigth, double? height, double? width, double? depth, string type)
        {
            var isInputValid = _inputValidationService.IsInputValid(weigth, depth, width, height, type);

            if(!isInputValid) return new List<Segment>();

            return new List<Segment>
            {
                new Segment {
                    EndCity = "Fredrikstad",
                    StartCity = "Oslo",
                    Cost = 20,
                    Time = 2
                },
                new Segment {
                    EndCity = "Copenhagen",
                    StartCity = "Helsingborg",
                    Cost = 50,
                    Time = 2
                }
            };
        }
    }
}
