using BrainBoost.Data;
using BrainBoost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private ApplicationDbContext _context;
        public HomeController(IEmailSender emailSender, ApplicationDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        //Landing page
        public async Task<IActionResult> Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            System.Diagnostics.Debug.WriteLine(isAuthenticated);
            if (User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
                return View("HomeCourses");
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
                return View();
            }
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
