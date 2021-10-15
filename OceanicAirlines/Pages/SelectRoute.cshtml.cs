using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using OceanicAirlines.Services;
using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace OceanicAirlines.Pages
{
    public class SelectRouteModel : PageModel
    {
        IDijsktraAlgorithmService _DijkstraService;
        ISegmentService _SegmentService;
        public SelectRouteModel(IDijsktraAlgorithmService DijkstraService, ISegmentService SegmentService)
        {
            _DijkstraService = DijkstraService;
            _SegmentService = SegmentService;
        }

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
            var segments = _SegmentService.GetAllSegments(Convert.ToDouble(weight), Convert.ToDouble(depth), Convert.ToDouble(height), Convert.ToDouble(width), _categories.ToLower().Replace(" ", ""));
            var ListOfRoutes = new List<Models.RouteOverall>();
            ListOfRoutes.Add(_DijkstraService.RunRouteSearching(segments, from, to, 0.0));
            ListOfRoutes.Add(_DijkstraService.RunRouteSearching(segments, from, to, 0.5));
            ListOfRoutes.Add(_DijkstraService.RunRouteSearching(segments, from, to, 1.0));

            routeDTOs = new List<routeDTO>();
            routes = new List<string>();
            var i = 1;
            foreach (var item in ListOfRoutes)
            {
                //var str = "";
                //foreach (var item2 in item.Routes)
                //{
                //    str += item2.Owner + ", ";
                //}
                routeDTOs.Add(new routeDTO
                {
                    ID = i,
                    Cost = item.Cost,
                    Duration = item.Time,
                    Final_delivery_by = item.Routes.Count > 0 ? item.Routes.Last().Owner : "err"
                });
                i += 1;
                var obj = JsonConvert.SerializeObject(item);
                routes.Add(HttpUtility.HtmlEncode(obj));
                    //JsonSerializer.Deserialize<IEnumerable<SegmentViewModel>>(contentAsString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            listofnames = routeDTOs[0].GetType().GetProperties();
        }
    }
}