using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace OceanicAirlines.Pages
{
    public class SelectRouteModel : PageModel
    {
        [ViewData]
        public string ErrorMessage { get; set; }
        [ViewData]
        public List<routeDTO> routeDTOs { get; set; }
        [ViewData]
        public System.Reflection.PropertyInfo[] listofnames { get; set; }
        [ViewData]
        public string _weight { get; set; }
        [ViewData]
        public string _height { get; set; }
        [ViewData]
        public string _width { get; set; }
        [ViewData]
        public string _depth { get; set; }
        [ViewData]
        public string _categories { get; set; }
        [ViewData]
        public string _from { get; set; }
        [ViewData]
        public string _to { get; set; }
        [ViewData]
        public List<string> routes { get; set; }

        public IActionResult OnGet()
        {
            //if (HttpContext.Session.GetInt32("LoggedIn") != 1)
            //    return Redirect("/Login");
            //// Do more stuff
            //return null;
            return Redirect("/PackageInformation");
        }

        public void OnPost(string weight, string height, string width, string depth, string categories, string from, string to)
        {
            _weight = weight;
            _height = height;
            _width = width;
            _depth = depth;
            _categories = categories;
            _from = from;
            _to = to;
            //ErrorMessage = $"weight: {weight}. height: {height}. width: {width}. depth: {depth}. categories: {categories}. from: {from}. to: { to}. ";
            routeDTOs = new List<routeDTO>{
                new routeDTO
                {
                    ID = 1,
                    Cost = 80,
                    Duration = 16,
                },
                new routeDTO
                {
                    ID = 2,
                    Cost = 40,
                    Duration = 32,
                },
                new routeDTO
                {
                    ID = 3,
                    Cost = 400,
                    Duration = 320,
                }
            };
            routes = new List<string> { "abc", "def" };
            listofnames = routeDTOs[0].GetType().GetProperties();
        }
    }
}