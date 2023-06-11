using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrainBoost.Data;
using BrainBoost.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BrainBoost.Controllers
{
    public class CourseProgressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseProgressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseProgress
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Professor"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            //Ako je profesor dohvacamo kreirane kurseve za statistiku
            if (User.IsInRole("Professor"))
            {
                var courseProgresses = await _context.CourseProgress.
                    Include(c => c.Course).
                    Include(c => c.Student).
                    Where(c => c.Course.Professor.Username.Equals(User.Identity.Name)).ToListAsync();

                ///Mapiranje podataka u custom mapu koja je proslijeđena u ViewBag, da se ne prikazuju dupli kursevi
                var courseMap = new Dictionary<int, Dictionary<string, dynamic>>();
                foreach (CourseProgress item in courseProgresses)
                {
                   if(!courseMap.ContainsKey(item.CourseId))
                    {
                        courseMap.Add(item.CourseId, new Dictionary<string, dynamic>());
                        courseMap[item.CourseId].Add("numberOfStudents", 0);
                        courseMap[item.CourseId].Add("studentsCompleted", 0);
                        courseMap[item.CourseId].Add("totalHours", 0);
                        courseMap[item.CourseId].Add("courseName", item.Course.CourseName);
                        courseMap[item.CourseId].Add("studentList", new List<Dictionary<string, dynamic>>());
                    }

                    courseMap[item.CourseId]["numberOfStudents"]++;
                    courseMap[item.CourseId]["totalHours"]+=item.Hours;

                    Dictionary<string, dynamic> student = new Dictionary<string, dynamic>();
                    student.Add("id", item.StudentId);
                    student.Add("firstName", item.Student.FirstName);
                    student.Add("lastName", item.Student.LastName);
                    student.Add("userName", item.Student.Username);
                    student.Add("hours", item.Hours);
                    courseMap[item.CourseId]["studentList"].Add(new Dictionary<string, dynamic>(student));
                    if(item.Progress >= 1)
                    {
                        courseMap[item.CourseId]["studentsCompleted"]++;
                    }

                }

                ///Sortiranje prema broju upisanih studenata
                var sourtedCourseMap = from entry in courseMap orderby entry.Value["numberOfStudents"] descending select entry;
                ViewBag.courseMap = sourtedCourseMap;

                ///Ispisivanje u konzolu
                foreach (var kvp in sourtedCourseMap)
                {
                    Console.WriteLine("Key = {0}", kvp.Key);
                    foreach (var kvp2 in kvp.Value)
                    {
                        Console.WriteLine("Key2 = {0}, Value2 = {1}", kvp2.Key, kvp2.Value);

                    }

                }

                return View("ProfessorStatistics", courseProgresses);
            }
            ///Ako je student dohvacamo enrollane kurseve za statistiku
            else
            {
                var courseProgresses = await _context.CourseProgress
                               .Include(c => c.Course)
                               .Include(c => c.Student)
                               .Where(c => c.Student.Username.Equals(User.Identity.Name))
                               .ToListAsync();
                return View(courseProgresses);
            }
        }

        // GET: CourseProgress/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CourseProgress == null)
            {
                return NotFound();
            }

            var courseProgress = await _context.CourseProgress
                .Include(c => c.Course)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.CourseProgressId == id);
            if (courseProgress == null)
            {
                return NotFound();
            }

            return View(courseProgress);
        }

        // GET: CourseProgress/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId");
            ViewData["StudentId"] = new SelectList(_context.Student, "UserId", "UserId");
            return View();
        }

        // POST: CourseProgress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseProgressId,StudentId,CourseId,LastAccess,Progress,IsCompleted,Hours")] CourseProgress courseProgress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseProgress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", courseProgress.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "UserId", "UserId", courseProgress.StudentId);
            return View(courseProgress);
        }

        // GET: CourseProgress/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CourseProgress == null)
            {
                return NotFound();
            }

            var courseProgress = await _context.CourseProgress.FindAsync(id);
            if (courseProgress == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", courseProgress.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "UserId", "UserId", courseProgress.StudentId);
            return View(courseProgress);
        }

        // POST: CourseProgress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseProgressId,StudentId,CourseId,LastAccess,Progress,IsCompleted,Hours")] CourseProgress courseProgress)
        {
            if (id != courseProgress.CourseProgressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseProgress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseProgressExists(courseProgress.CourseProgressId))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", courseProgress.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "UserId", "UserId", courseProgress.StudentId);
            return View(courseProgress);
        }

        // GET: CourseProgress/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CourseProgress == null)
            {
                return NotFound();
            }

            var courseProgress = await _context.CourseProgress
                .Include(c => c.Course)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.CourseProgressId == id);
            if (courseProgress == null)
            {
                return NotFound();
            }

            return View(courseProgress);
        }

        // POST: CourseProgress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CourseProgress == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CourseProgress'  is null.");
            }
            var courseProgress = await _context.CourseProgress.FindAsync(id);
            if (courseProgress != null)
            {
                _context.CourseProgress.Remove(courseProgress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseProgressExists(int id)
        {
          return _context.CourseProgress.Any(e => e.CourseProgressId == id);
        }

        ///GET: CourseProgress/MyActiveCourses
        public IActionResult MyActiveCourses()
        {
            return View();
        }

        ///GET: CourseProgress/MyFinishedCourses
        public IActionResult MyFinishedCourses()
        {
            return View();
        }

        ///GET: CourseProgress/CourseStatistics/1
        public async Task<IActionResult> CourseStatistics(int? id)
        {
            var course = await _context.Course
                        .FirstOrDefaultAsync(m => m.CourseId == id);
            return View(course);
        }
    }
}
