using Microsoft.AspNetCore.Mvc;
using OceanicAirlines.Models;
using System.Collections.Generic;

namespace OceanicAirlines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        [HttpGet]
        public List<Segment> Get([FromQuery] double? weigth, double? heigth, double? width, double? depth)
        {
            if(heigth is null || weigth is null || width is null || depth is null)
            {
                return new List<Segment>();
            };

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
                    Cost = 200,
                    Time = 1
                }
            };
        }
    }
}
