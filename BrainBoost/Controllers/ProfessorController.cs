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

namespace BrainBoost.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfessorController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Professor
        public async Task<IActionResult> Index()
        {
              return View(await _context.Professor.ToListAsync());
        }

        // GET: Professor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Professor == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // GET: Professor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Professor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Username,BirthDate")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professor);
        }

        // GET: Professor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Kljuc represents id that should be sent to view
            TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            if (id == null || _context.Professor == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            return View(professor);
        }

        // POST: Professor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,BirthDate")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validations
                    var profa = await _context.Professor.FindAsync(id);
                    if (profa.FirstName == professor.FirstName && profa.LastName == professor.LastName && profa.BirthDate == professor.BirthDate)
                    {
                        return View(professor);
                    }
                    int age = 0;
                    age = DateTime.Now.Subtract(professor.BirthDate).Days;
                    age = age / 365;
                    if (age < 6)
                    {
                        TempData["Datum"] = "You must be at least 6 years old to register!";
                        return View(professor);
                    }
                    // Making changes on model that is already in database
                    profa.FirstName = professor.FirstName;
                    profa.LastName = professor.LastName;
                    profa.BirthDate = professor.BirthDate;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Podaci"] = "You have successfully changed your personal information.";
            }
            return View(professor);
        }

        // GET: Professor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Professor == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // POST: Professor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Professor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Professor'  is null.");
            }
            var professor = await _context.Professor.FindAsync(id);
            if (professor != null)
            {
                _context.Professor.Remove(professor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorExists(int id)
        {
          return _context.Professor.Any(e => e.UserId == id);
        }

        ///GET: Professor/CreateCourse
        public IActionResult CreateCourse()
        {
            return View();
        }

        // TODO: POST: Professor/CreateCourse


        ///GET: Professor/CreateQuiz
        public IActionResult CreateQuiz()
        {
            return View();
        }

        // TODO: POST: Professor/CreateQuiz


        ///GET: Professor/CreateCourseContent
        public IActionResult CreateCourseContent()
        {
            return View();
        }

        // TODO: POST: Professor/CreateCourseContent
        
    }
}
