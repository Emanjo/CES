using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OceanicAirlines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OceanicAirlines.Pages
{
    public class ConfirmationModel : PageModel
    {
        [ViewData]
        public string ErrorMessage { get; set; }
        [ViewData]
        public string SuccessMessage { get; set; }

        public DataService _dataService {  get; set; }

        private int GetCityId(string CityName)
        {
            var cities = _dataService.GetCities();
            foreach (City c in cities)
            {
                if (CityName.Equals(c.Name))
                {
                    return c.Id;
                }
            }
            return -1;
        }

        public IActionResult OnGet()
        {
            return Redirect("/PackageInformation");
        }

        public void OnPost(string weight, string height, string width, string depth, string categories, string from, string to, string route, string weaponsconfirmed)
        {
            if (categories == "Weapons" && weaponsconfirmed != "yes")
            {
                ErrorMessage = "Weapons were not confirmed";
                return;
            }

            _dataService = new DataService();
            var decoded = HttpUtility.HtmlDecode(route).Replace("\\\"", "\"");
            var result = JsonConvert.DeserializeObject<RouteOverall>(decoded);
            var lastCity = result.Routes.Last().EndCity;
            var cities = new List<Int32>();
            foreach (var c in result.Routes)
            {
                cities.Add(GetCityId(c.StartCity));
            }
            cities.Add(GetCityId(result.Routes.Last().EndCity));
            //ErrorMessage = $"weight: {weight}. height: {height}. width: {width}. depth: {depth}. categories: {categories}. from: {from}. to: { to}. route: {route}";

            var suc = _dataService.AddOrder(GetCityId(lastCity), string.Join(",", cities), HttpContext.Session.GetInt32("userID"),
                Convert.ToDouble(weight), Convert.ToDouble(width), Convert.ToDouble(height), Convert.ToDouble(depth), result.Cost, result.Time, categories);

            if (suc)
                SuccessMessage = "Your package has been submitted.";
            else
                ErrorMessage = "The order could not be saved in the database.";
        }
    }
}
