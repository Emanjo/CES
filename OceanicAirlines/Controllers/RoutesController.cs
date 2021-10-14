using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
        public ActionResult<List<SegmentViewModel>> Get([FromQuery] double? weight, double? height, double? width, double? depth, string type)
        {
            if(!this.ValidateAuthentication()) return Unauthorized();

            var isInputValid = _inputValidationService.IsInputValid(weight, depth, width, height, type);

            if (!isInputValid) return new List<SegmentViewModel>();

            return Ok(new List<SegmentViewModel>
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
            });
        }

        private bool IsAuthenticated()
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                if (value.ToString() == "Basic OATLEIT") return true;
            }

            return false;
        }
    }
}
