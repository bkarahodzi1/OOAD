using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrainBoost.Data;
using BrainBoost.Models;

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
            var applicationDbContext = _context.CourseProgress.Include(c => c.Course).Include(c => c.Student);
            return View(await applicationDbContext.ToListAsync());
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
    }
}
