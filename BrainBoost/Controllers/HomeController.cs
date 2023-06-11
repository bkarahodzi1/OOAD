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
            if (isAuthenticated)
            {
                Console.WriteLine("User logged in {0}", User.Identity.Name);
                if (User.IsInRole("Professor") || User.IsInRole("Admin"))
                {
                    TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
                }
                else if(User.IsInRole("Student"))
                {
                    TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
                }

                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Student");
                    
                }
                // Dohvaćamo trenutno prijavljenog studenta preko korisničkog imena.
                var currentStudent = await _context.Student
                    .FirstOrDefaultAsync(s => s.Username == User.Identity.Name);

                if (currentStudent != null)
                {
                    // Dohvaćamo posljednji pristupljeni kurs za trenutno prijavljenog studenta.
                    var lastAccessedCourseProgress = await _context.CourseProgress
                        .Include(cp => cp.Course)
                        .Where(cp => cp.StudentId == currentStudent.UserId)
                        .OrderByDescending(cp => cp.LastAccess)
                        .FirstOrDefaultAsync();

                    if (lastAccessedCourseProgress != null)
                    {
                        // Šaljemo ime kursa na View.
                        ViewData["LastAccessedCourse"] = lastAccessedCourseProgress.Course.CourseName;
                    }
                }

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
