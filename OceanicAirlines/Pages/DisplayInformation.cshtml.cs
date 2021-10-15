using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OceanicAirlines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace OceanicAirlines.Pages
{
    public class DisplayInformationModel : PageModel
    {
        [ViewData]
        public string ErrorMessage { get; set; }
        [ViewData]
        public System.Reflection.PropertyInfo[] listofnames { get; set; }
        [ViewData]
        public List<string> listofnames2 {  get; set; }
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
        public string _selectedroute { get; set; }
        [ViewData]
        public routeDTO route { get; set; }
        [ViewData]
        public RouteOverall routeDetailed { get; set; }
        [ViewData]
        public DataService _dataservice { get; set; }
        public IActionResult OnGet()
        {
            return Redirect("/PackageInformation");
        }

        public void OnPost(string weight, string height, string width, string depth, string categories, string from, string to, string selectedroute, string selectedroutestr)
        {
            _weight = weight;
            _height = height;
            _width = width;
            _depth = depth;
            _categories = categories;
            switch (categories)
            {
                case "a":
                    _categories = "Cautious Parcels";
                    break;
                case "b":
                    _categories = "Livestock";
                    break;
                case "c":
                    _categories = "Other";
                    break;
                case "d":
                    _categories = "Recorded Delivery";
                    break;
                case "e":
                    _categories = "Refrigerated Goods";
                    break;
                case "f":
                    _categories = "Weapons";
                    break;
            }
            _from = from;
            _to = to;
            _selectedroute = selectedroutestr;

            // Default value
            route = new routeDTO
            {
                ID = 123,
                Cost = 456,
                Duration = 789,
                Final_delivery_by = "bla"
            };
            listofnames = route.GetType().GetProperties();

            var decoded = HttpUtility.HtmlDecode(selectedroutestr).Replace("\\\"", "\"");
            //var result = JsonSerializer.Deserialize<Models.RouteOverall>(decoded, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //    IgnoreReadOnlyFields = true,
            //    IgnoreReadOnlyProperties = true,
            //    AllowTrailingCommas= true
            //});
            var result = JsonConvert.DeserializeObject<RouteOverall>(decoded);
            if (result == null)
                return;
            if (result.Routes == null)
                return;
            route = new routeDTO
            {
                ID = Convert.ToInt32(selectedroute),
                Cost = result.Cost,
                Duration = result.Time,
                Final_delivery_by = result.Routes.Count > 0 ? result.Routes.Last().Owner : "err"
            };
            listofnames = route.GetType().GetProperties();
            listofnames2 = new List<string>();
            foreach (var item in listofnames)
            {
                if (item.Name == "Cost")
                    listofnames2.Add("Cost [USD]");
                else if (item.Name == "Duration")
                    listofnames2.Add("Duration [hours]");
                else
                    listofnames2.Add(item.Name.Replace("_", " "));
            }
            routeDetailed = result;
            _dataservice = new DataService();
        }
    }
}
