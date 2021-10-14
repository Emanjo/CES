using OceanicAirlines.Enums;
using OceanicAirlines.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace OceanicAirlines.Services
{
    public class IntegrationApiClient : IIntegrationApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private Uri EastIndiaBaseurl => new Uri("https://wa-eit-t2.azurewebsites.net/api/external/");
        private Uri TelestaraBaseurl => new Uri("https://wa-tl-t2.azurewebsites.net/");

        public IntegrationApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<Segment> GetSegments(Company company, double heigth, double depth, double width, double weigth, string type)
        {
            var client = _httpClientFactory.CreateClient();

            switch (company)
            {
                case Company.EastIndia:
                    client.BaseAddress = EastIndiaBaseurl;
                    break;
                case Company.Telestar:
                    client.BaseAddress = TelestaraBaseurl;
                    break;
            }

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            var response = client.GetAsync($"routes?weight={weigth.ToString(nfi)}&height={heigth.ToString(nfi)}&width={width.ToString(nfi)}&depth={depth.ToString(nfi)}&type={type.ToString(nfi)}").Result;

            response.EnsureSuccessStatusCode();

            var contentAsString = response.Content.ReadAsStringAsync().Result;

            var result = JsonSerializer.Deserialize<IEnumerable<Segment>>(contentAsString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return result;
        }

        private static string ToStringWithNoComma(double value)
        {
            return value.ToString().Replace(",", ".");
        }
    }
}
