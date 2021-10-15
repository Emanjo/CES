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
        public List<string> routes { get; set; }

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
            var segments = _SegmentService.GetAllSegments(Convert.ToDouble(weight), Convert.ToDouble(depth), Convert.ToDouble(height), Convert.ToDouble(width), _categories.ToLower().Replace(" ", ""));
            var ListOfRoutes = new List<Models.RouteOverall>();
            try
            {
                ListOfRoutes.Add(_DijkstraService.RunRouteSearching(segments, from, to, 0.0));
            }
            catch (Exception) { }
            try
            {
                ListOfRoutes.Add(_DijkstraService.RunRouteSearching(segments, from, to, 0.5));
            }
            catch (Exception) { }
            try
            {
                ListOfRoutes.Add(_DijkstraService.RunRouteSearching(segments, from, to, 1.0));
            }
            catch (Exception) { }

            routeDTOs = new List<routeDTO>();
            routes = new List<string>();
            var i = 1;
            foreach (var item in ListOfRoutes)
            {
                if (item.Routes.Count > 0)
                {
                    var duplicate = (routeDTOs.Count > 0 && routeDTOs[0].Cost == item.Cost
                        && routeDTOs[0].Duration == item.Time && item.Routes.Last().Owner.Equals(routeDTOs[0].Final_delivery_by));
                    if (duplicate)
                        continue;
                    routeDTOs.Add(new routeDTO
                    {
                        ID = i,
                        Cost = item.Cost,
                        Duration = item.Time,
                        Final_delivery_by = item.Routes.Last().Owner ?? ""
                    });
                    i += 1;
                    var obj = JsonConvert.SerializeObject(item);
                    routes.Add(HttpUtility.HtmlEncode(obj));
                }
            }
            listofnames = (new routeDTO()).GetType().GetProperties();
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
            if (routeDTOs.Count == 0)
                ErrorMessage = $"No connections found between {from} and {to}. Possible reasons: missing airport connections, refusal from other transport services to carry cargo, or connectivity loss with other transport services.";
        }
    }
}