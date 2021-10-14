using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OceanicAirlines.Pages
{
    public class ConfirmationModel : PageModel
    {
        [ViewData]
        public string ErrorMessage { get; set; }
        [ViewData]
        public string SuccessMessage { get; set; }

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
            //ErrorMessage = $"weight: {weight}. height: {height}. width: {width}. depth: {depth}. categories: {categories}. from: {from}. to: { to}. route: {route}";
            SuccessMessage = "Your package has been submitted";
        }
    }
}
