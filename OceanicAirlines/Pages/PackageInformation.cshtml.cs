using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using OceanicAirlines.Models;

namespace OceanicAirlines.Pages
{
    public class PackageInformationModel : PageModel
    { 
        [ViewData]
        public string ErrorMessage { get; set; }

        [ViewData]
        public List<City> listOfCities { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") != 1)
                return Redirect("/Login");
            // Do more stuff
            return null;
        }

    }
}
