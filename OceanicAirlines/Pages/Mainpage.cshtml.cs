using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OceanicAirlines.Pages
{
    public class MainPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") != 1)
                return Redirect("/Login");
            return null;
        }
    }
}
