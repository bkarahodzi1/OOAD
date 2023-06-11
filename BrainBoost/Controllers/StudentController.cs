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
using System.Text.RegularExpressions;

namespace BrainBoost.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
              return View(await _context.Student.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountBalance,UserId,FirstName,LastName,Username,Email,BirthDate,CreatedAt,IsVerified")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Kljuc represents id that should be sent to view
            TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountBalance,FirstName,LastName,BirthDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validations
                    if (student.AccountBalance <= 0 || student.AccountBalance > 10000)
                    {
                        TempData["Pare"] = "Entered amount is over limitation.";
                        return View(student);
                    }
                    var stu = await _context.Student.FindAsync(id);
                    if (stu.FirstName == student.FirstName && stu.LastName == student.LastName && stu.BirthDate == student.BirthDate && stu.AccountBalance == student.AccountBalance)
                    {
                        return View(student);
                    }
                    int age = 0;
                    age = DateTime.Now.Subtract(student.BirthDate).Days;
                    age = age / 365;
                    if (age < 6)
                    {
                        TempData["Pare"] = "You must be at least 6 years old to register!";
                        return View(student);
                    }
                    // Making changes on model that is already in database
                    stu.FirstName = student.FirstName;
                    stu.LastName = student.LastName;
                    stu.BirthDate = student.BirthDate;
                    stu.AccountBalance = student.AccountBalance;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.UserId))
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
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                var user = await _userManager.FindByNameAsync(student.Username);
                _context.Student.Remove(student);
                await _userManager.DeleteAsync(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return _context.Student.Any(e => e.UserId == id);
        }

        ///GET: Student/Settings
        public IActionResult Settings()
        {
            return View();
        }
    }
}
