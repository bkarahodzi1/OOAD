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
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quiz
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Quiz.Include(q => q.CourseMaterial).Include(q => q.CreatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Quiz/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .Include(q => q.CourseMaterial)
                .Include(q => q.CreatedBy)
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Quiz/Create
        public IActionResult Create()
        {
            ViewData["CourseMaterialId"] = new SelectList(_context.CourseMaterial, "CourseMaterialId", "CourseMaterialId");
            ViewData["CreatedById"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Quiz/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizId,CourseMaterialId,CreatedById,QuizName,Description")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseMaterialId"] = new SelectList(_context.CourseMaterial, "CourseMaterialId", "CourseMaterialId", quiz.CourseMaterialId);
            ViewData["CreatedById"] = new SelectList(_context.User, "UserId", "UserId", quiz.CreatedById);
            return View(quiz);
        }

        // GET: Quiz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            ViewData["CourseMaterialId"] = new SelectList(_context.CourseMaterial, "CourseMaterialId", "CourseMaterialId", quiz.CourseMaterialId);
            ViewData["CreatedById"] = new SelectList(_context.User, "UserId", "UserId", quiz.CreatedById);
            return View(quiz);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizId,CourseMaterialId,CreatedById,QuizName,Description")] Quiz quiz)
        {
            if (id != quiz.QuizId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizId))
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
            ViewData["CourseMaterialId"] = new SelectList(_context.CourseMaterial, "CourseMaterialId", "CourseMaterialId", quiz.CourseMaterialId);
            ViewData["CreatedById"] = new SelectList(_context.User, "UserId", "UserId", quiz.CreatedById);
            return View(quiz);
        }

        // GET: Quiz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .Include(q => q.CourseMaterial)
                .Include(q => q.CreatedBy)
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quiz == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Quiz'  is null.");
            }
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz != null)
            {
                _context.Quiz.Remove(quiz);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
          return _context.Quiz.Any(e => e.QuizId == id);
        }

        ///GET: Quiz/Quiz
        public IActionResult Quiz()
        {
            return View();
        }

        ///GET: Quiz/QuizAnswers
        public IActionResult QuizAnswers()
        {
            return View();
        }
    }
}
