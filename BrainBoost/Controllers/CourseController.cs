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
            if(User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            string username = User.Identity.Name;
            //list of currencies available to choose in view 
            List<string> currencies = new List<string>
            {
                "USD", "EUR", "GBP", "KM"
            };
            SelectList slCurrencies=new SelectList(currencies);
            ViewBag.Currencies = slCurrencies;
            //returns view for creating course
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
                    //creates course with professor data
                    Professor professor = await _context.Professor.FirstOrDefaultAsync(p => p.Username == User.Identity.Name);
                    course.CreatedAt = DateTime.Now;
                    course.UpdatedAt = DateTime.Now;
                    course.Professor = professor;
                    course.ProfessorId = professor.UserId;
                    //saving course to database
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                }
                catch {; }

                
            ViewData["ProfessorId"] = new SelectList(_context.Professor, "UserId", "UserId", course.ProfessorId);
                //returns view to invite students to course
                return RedirectToAction(nameof(InviteStudents), new { id = course.CourseId });
            }
            else { return View(); }
           
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

        public IActionResult RefundConfirmation(int id)
        {
            var userId = User.Identity.Name;
            bool isEnrolled = _context.CourseProgress.Any(cp => cp.CourseId == id && cp.StudentId.ToString() == userId);
            var course = _context.Course.Find(id);
            TempData["CourseName"] = course.CourseName;
            TempData["CoursePrice"] = course.Price;
            TempData["CourseCurrency"] = course.Currency;
            ViewData["IsEnrolled"] = isEnrolled;
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
            TempData["SuccessMessage"] = "Va� zahtjev za refund je uspje�no poslan!";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult MyCourses()
        {
            if (User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            if (User.IsInRole("Student"))
            {
                //gets student's courses
             var courses = _context.CourseProgress
                            .Include(c => c.Course)
                            .ThenInclude(s => s.Professor)
                            .Include(c => c.Student)
                            .Where(c => c.Student.Username.Equals(User.Identity.Name))
                            .ToList();
                return View(courses);
            }
           else if(User.IsInRole("Professor"))
            {//gets professor's courses
                var courses = _context.Course
                        .Include(s => s.Professor)
                        .Where(c => c.Professor.Username.Equals(User.Identity.Name))
                        .ToList();

                return View("MyCourses(Professor)", courses);
            }
            return NotFound();
        }
        public IActionResult CourseSearch()
        {
            if (User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            //courses available for searching
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
            //courses with string from input
            var filteredCourses = _context.Course.Include(c => c.Professor)
                                                .Where(c => c.CourseName.Contains(searchString))
                                                .ToList();

            return View("CourseSearch", filteredCourses);
        }
        
        // GET: Course/Details/5
        public async Task<IActionResult> EnrollMe(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            //gets selected course from database
            var course = await _context.Course
                .Include(c => c.Professor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Student"))
            {
                //finds course progress for student and selected course
                var username = User.Identity.Name;
                Student student = await _context.Student.FirstOrDefaultAsync(s => s.Username == username);
                var courseProgress = await _context.CourseProgress
                .Include(cp=>cp.Student)
                .FirstOrDefaultAsync(cp => cp.CourseId == id&& cp.StudentId==student.UserId);

                if (courseProgress == null && course.Price == 0)
                {
                    //creates course progress if it doesn't exist and course is free
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
                    //redirects to billing if course is not free
                    ViewData["controller"] = "Billing";
                    ViewData["action"] = "CourseBilling";
                }


                Billing billing = await _context.Billing.FirstOrDefaultAsync(b => b.user.UserId == student.UserId&&b.CourseId==id);

                if (courseProgress != null && courseProgress.StudentId.Equals(student.UserId))
                {
                    //if course is paid, redirects to details
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
        public async Task<IActionResult> DetailsForMyCourses(int? id)
        {
            if (User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
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
            var courseMaterials = await _context.CourseMaterial
                .Where(cm => cm.CourseId == id)
                .ToListAsync();

            ViewData["isEnrolled"] = "true";
            ViewData["CourseMaterials"] = courseMaterials;
            return View("Details", course);
        }
       


        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
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
                var username = User.Identity.Name;
                Student student = await _context.Student.FirstOrDefaultAsync(s => s.Username == username);
                var courseMaterials = await _context.CourseMaterial
                .Where(cm => cm.CourseId == id)
                .ToListAsync();
            if (User.IsInRole("Student"))
            {
                var courseProgress = await _context.CourseProgress
                .Include(cp => cp.Student)
                .FirstOrDefaultAsync(cp => cp.CourseId == id && cp.StudentId == student.UserId);
                Billing billing = await _context.Billing.FirstOrDefaultAsync(b => b.user.UserId == student.UserId && b.CourseId == id);
                if (courseProgress == null && course.Price == 0)
                {
                    //redirects to enroll me page if course is free and student not enrolled
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
                    //redirects to billing if course is not free

                    ViewData["controller"] = "Billing";
                    ViewData["action"] = "CourseBilling";
                }
                

            var proxy = new Proxy();
            proxy.CheckPayment(courseProgress, course, ViewData);
           
            }

             ViewData["CourseMaterials"] = courseMaterials;
            return View(course);
        }

        [HttpPost]
        public IActionResult SelectedStudents(List<int> selectedStudents, int? courseId)
        {
            // Process the selected students
            List<Student> students = _context.Student.Where(s => selectedStudents.Contains(s.UserId)).ToList();


            if (students.Count > 0) { 
            //do something with selected students, eg. Send them emails.
            }


            return RedirectToAction("Details", new { id = courseId });
        }
        public IActionResult InviteStudents(int? id)
        {
            //gets course and list of all students
            ViewBag.course = _context.Course.FirstOrDefault(c => c.CourseId == id);

            List<Student> students = _context.Student.ToList();
            //returns view with option to select students
            return View(students);
        }


    }
}
