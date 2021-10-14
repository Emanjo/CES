using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OceanicAirlines.Pages
{
    public class DisplayInformationModel : PageModel
    {
        [ViewData]
        public string ErrorMessage { get; set; }
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
        public routeDTO route { get; set; }
        public IActionResult OnGet()
        {
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

            route = new routeDTO
            {
                ID = 1,
                Cost = 80,
                Duration = 16,
            };
            listofnames = route.GetType().GetProperties();
        }
    }
}
