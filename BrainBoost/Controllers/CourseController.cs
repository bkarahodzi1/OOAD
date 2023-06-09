using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrainBoost.Data;
using BrainBoost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace BrainBoost.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CourseController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Course.Include(c => c.Professor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            string username = User.Identity.Name;
            List<string> currencies = new List<string>
            {
                "USD", "EUR", "GBP", "KM"
            };
            SelectList slCurrencies=new SelectList(currencies);
            ViewBag.Currencies = slCurrencies;
            return View();
        }


        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseName,Description,Price,Currency")] Course course)
        {
            if (ModelState.IsValid)

            {
                var username = User.Identity.Name;

                try
                {
                    Professor professor = await _context.Professor.FirstOrDefaultAsync(p => p.Username == User.Identity.Name);
                    course.CreatedAt = DateTime.Now;
                    course.UpdatedAt = DateTime.Now;
                    course.Professor = professor;
                    course.ProfessorId = professor.UserId;
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                }
                catch {; }

            ViewData["ProfessorId"] = new SelectList(_context.Professor, "UserId", "UserId", course.ProfessorId);

                return RedirectToAction(nameof(Details), new { id = course.CourseId });
            }
            else { return View(); }
           
            return View();
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["ProfessorId"] = new SelectList(_context.Professor, "UserId", "UserId", course.ProfessorId);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,ProfessorId,CourseName,Description,Price,Currency,CompletedCount,CompletedPercentage,CreatedAt,UpdatedAt")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProfessorId"] = new SelectList(_context.Professor, "UserId", "UserId", course.ProfessorId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return _context.Course.Any(e => e.CourseId == id);
        }

        ///GET: Course/CourseAttendees
        public IActionResult CourseAttendees()
        {
            return View();
        }

        ///GET: Course/CourseContent
        public IActionResult CourseContent()
        {
            return View();
        }

        ///GET: Course/CourseProfessor
        public IActionResult CourseProfessor()
        {
            return View();
        }

        ///GET: Course/CourseSearch

        ///GET: Course/CourseStatistics
        public IActionResult CourseStatistics()
        {
            return View();
        }

        public IActionResult RefundConfirmation()
        {
            return View();
        }

        public IActionResult RefundForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitRefundRequest(string reason)
        {
            TempData["SuccessMessage"] = "Vaš zahtjev za refund je uspješno poslan!";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult MyCourses()
        {
            if(User.IsInRole("Student"))
            {
             var courses = _context.CourseProgress
                            .Include(c => c.Course)
                            .ThenInclude(s => s.Professor)
                            .Include(c => c.Student)
                            .Where(c => c.Student.Username.Equals(User.Identity.Name))
                            .ToList();
                return View(courses);
            }
           else if(User.IsInRole("Professor"))
            {
                var courses = _context.CourseProgress
                            .Include(c => c.Course)
                            .ThenInclude(s => s.Professor)
                            .Include(c => c.Student)
                            .Where(c => c.Course.Professor.Username.Equals(User.Identity.Name))
                            .ToList();
                return View(courses);
            }
            return NotFound();
        }
        public IActionResult CourseSearch()
        {
            var courses = _context.Course.Include(c => c.Professor).ToList();
            return View(courses);
        }
        public IActionResult Search(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                var courses = _context.Course.Include(c => c.Professor).ToList();
                return View("CourseSearch", courses);
            }

            var filteredCourses = _context.Course.Include(c => c.Professor)
                                                .Where(c => c.CourseName.Contains(searchString))
                                                .ToList();

            return View("CourseSearch", filteredCourses);
        }
        public async Task<IActionResult> DetailsForMyCourses(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            // Find the CourseProgress entity
            var courseProgress = await _context.CourseProgress
                .FirstOrDefaultAsync(cp => cp.CourseId == id);

            if (courseProgress != null)
            {
                // Update the LastAccess property
                courseProgress.LastAccess = DateTime.Now;

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = course.CourseId });
        }
       
        
        // GET: Course/Details/5
        public async Task<IActionResult> EnrollMe(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Student"))
            {

                var username = User.Identity.Name;
                Student student = await _context.Student.FirstOrDefaultAsync(s => s.Username == username);
                var courseProgress = await _context.CourseProgress
                .Include(cp=>cp.Student)
                .FirstOrDefaultAsync(cp => cp.CourseId == id&& cp.StudentId==student.UserId);

                if (courseProgress == null && course.Price == 0)
                {
                    CourseProgress courseProgressNew = new CourseProgress();
                    courseProgressNew.StudentId = student.UserId;
                    courseProgressNew.Course = course;
                    courseProgressNew.CourseId = course.CourseId;
                    courseProgressNew.Progress = 0;
                    courseProgressNew.Hours = 0;
                    courseProgressNew.IsCompleted = false;
                    courseProgressNew.LastAccess = DateTime.Now;

                    _context.Add(courseProgressNew);
                    courseProgress = courseProgressNew;
                    await _context.SaveChangesAsync();

                }
                else if (course.Price > 0)
                {
                    ViewData["controller"] = "Billing";
                    ViewData["action"] = "CourseBilling";
                }


                Billing billing = await _context.Billing.FirstOrDefaultAsync(b => b.user.UserId == student.UserId&&b.CourseId==id);

                if (courseProgress != null && courseProgress.StudentId.Equals(student.UserId))
                {
                    // Update the LastAccess property
                    courseProgress.LastAccess = DateTime.Now;
                    ViewData["isEnrolled"] = "true";
                    ViewData["Controller"] = "Course";
                    ViewData["Action"] = "Details";

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }



                return RedirectToAction("Details", new { id = course.CourseId });

            }



            return View(course);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Student"))
            {
                var username = User.Identity.Name;
                Student student = await _context.Student.FirstOrDefaultAsync(s => s.Username == username);
                var courseProgress = await _context.CourseProgress
                .Include(cp => cp.Student)
                .FirstOrDefaultAsync(cp => cp.CourseId == id && cp.StudentId == student.UserId);




                Billing billing = await _context.Billing.FirstOrDefaultAsync(b => b.user.UserId == student.UserId && b.CourseId == id);
                if (courseProgress == null && course.Price == 0)
                {
                    ViewData["isEnrolled"] = "false";
                    ViewData["Controller"] = "Course";
                    ViewData["Action"] = "EnrollMe";
                }
                 else if (courseProgress != null && courseProgress.StudentId.Equals(student.UserId))
                {
                    // Update the LastAccess property
                    courseProgress.LastAccess = DateTime.Now;
                    ViewData["isEnrolled"] = "true";
                    ViewData["Controller"] = "Course";
                    ViewData["Action"] = "Details";

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewData["controller"] = "Billing";
                    ViewData["action"] = "CourseBilling";
                }



                return View(course);

            }

            

            return View(course);
        }


    }
}
