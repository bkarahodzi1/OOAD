using BrainBoost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace BrainBoost.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        public HomeController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        //Landing page
        public async Task<IActionResult> Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            System.Diagnostics.Debug.WriteLine(isAuthenticated);
            if (isAuthenticated)
            {
                return View("HomeCourses");
            }
            else return View();
        }

        //Home page - home courses
        [Authorize]
        public IActionResult HomeCourses()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
