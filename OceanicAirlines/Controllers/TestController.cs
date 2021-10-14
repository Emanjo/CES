﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanicAirlines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            if (!this.ValidateAuthentication()) return Unauthorized();

            DataService service = new DataService();
            List<City> returnValue = service.GetCities();
            DataService service1 = new DataService();

            return Ok();
        }
    }
}
