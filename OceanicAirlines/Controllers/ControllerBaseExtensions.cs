using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines.Controllers
{
    public static class ControllerBaseExtensions
    {
        public static bool ValidateAuthentication(this ControllerBase controller)
        {
            if (controller.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                if (value.ToString() == "Basic OATLEIT") return true;
            }

            return false;
        }
    }
}
