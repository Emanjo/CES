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
        private readonly ISegmentService _segmentService;

        public RoutesController(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SegmentViewModel>> Get([FromQuery] double? weight, double? height, double? width, double? depth, string type)
        {
            if(!this.ValidateAuthentication()) return Unauthorized();

            var result = _segmentService.GetInternalSegments(weight, depth, height, width, type);

            return Ok(result);
        }
    }
}
