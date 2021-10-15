using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OceanicAirlines.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace OceanicAirlines.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IDataService _dataService;

        public LoginModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        [ViewData]
        public string ErrorMessage { get; set; }

        private bool CheckUserInfo(string emailinput, string passwordinput)
        {
            string passwordHash = _dataService.GetPasswordHash(emailinput);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(passwordinput));
                string hashString = Convert.ToBase64String(hash);
                if (hashString == passwordHash)
                    return true;
                return false;
            }
        }

        public IActionResult OnPost(string emailinput, string passwordinput)
        {
            if (CheckUserInfo(emailinput, passwordinput))
            {
                HttpContext.Session.SetInt32("LoggedIn", 1);
                HttpContext.Session.SetInt32("userID", _dataService.GetUserID(emailinput));
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
