using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;


namespace OceanicAirlines.Pages
{
    public class LoginModel : PageModel
    {
        [ViewData]
        public string ErrorMessage { get; set; }

        //public void OnGet()
        //{
        //    ViewData["confirmation"] = "bla";
        //    Message = "hello";
        //}

        private string realUser = "casper.sloth@oceanicairways.com";
        private string realPassword = "netcompany-123";

        private bool CheckUserInfo(string emailinput, string passwordinput)
        {
            if (emailinput == realUser && passwordinput == realPassword)
                return true;
            return false;
        }

        public IActionResult OnPost(string emailinput, string passwordinput)
        {
            if (CheckUserInfo(emailinput, passwordinput))
            {
                HttpContext.Session.SetInt32("LoggedIn", 1);
                return Redirect("/MainPage");
            }
            else
            {
                ErrorMessage = "E-mail or password was not recognized. Please try again";
            }
            return null;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
                return Redirect("/MainPage");
            return null;
        }
    }
}
