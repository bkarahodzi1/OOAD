using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrainBoost.Data;
using BrainBoost.Models;
using SQLitePCL;

namespace BrainBoost.Controllers
{
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
        }

       

        ///GET: Billing/CourseBilling
        public async Task<IActionResult> CourseBilling(int id)
        {
            if (User.IsInRole("Professor") || User.IsInRole("Admin"))
            {
                TempData["Kljuc"] = _context.Professor.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            else
            {
                TempData["Kljuc"] = _context.Student.FirstOrDefault(p => p.Username == User.Identity.Name).UserId;
            }
            ViewData["id"] = id;
            ViewBag.course=_context.Course.FirstOrDefault(c=>c.CourseId==id);
            //returns billing page for selected course

         return View();
        }

        // GET: Billing
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Billing.Include(b => b.BillingCard).Include(b => b.Course);
            return View(await applicationDbContext.ToListAsync());
           
        }

        // GET: Billing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Billing == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.BillingCard)
                .Include(b => b.Course)
                .FirstOrDefaultAsync(m => m.BillingId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // GET: Billing/Create
        public  IActionResult Create(int courseid)
        {
            var username = User.Identity.Name;
            ViewData["BillingCardId"] = new SelectList(_context.BillingCard, "BillingCardId", "BillingCardId");
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId");
            

            return RedirectToAction("Details", "Course", new { id = courseid });
        }

        // POST: Billing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int expiryMonth,int expiryYear,string cardNumber, int courseid, int cvv)
		{
            var username = User.Identity.Name;
			Student student = await _context.Student.FirstOrDefaultAsync(s => s.Username == username);
            Course course =await _context.Course.FirstOrDefaultAsync(c=>c.CourseId == courseid);
            Billing billing = new Billing();


            var billingCard = new BillingCard();

            billingCard = await _context.BillingCard.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

             if (billingCard is null)
    {
                // Add validation error for card number
                TempData["CardError"] = "Card doesn't exist.";
                ModelState.AddModelError(nameof(cardNumber), "Card doesn't exist.");
    }
    else
    {
        if (billingCard.CVV != cvv)
        {
                    // Add validation error for CVV code
                    TempData["CVVError"] = "Wrong CVV code.";

                    ModelState.AddModelError(nameof(cvv), "Wrong CVV code.");
        }

        if (student.AccountBalance < course.Price)
        {
                    // Add validation error for account balance
                    TempData["BalanceError"] = "Not enough balance in your account.";

                    ModelState.AddModelError(nameof(student.AccountBalance), "Not enough balance in your account.");
        }
        if (billingCard.ExpiryMonth != expiryMonth)
        {
            // Add validation error for account balance
            TempData["MonthError"] = "Wrong expiry month.";

            ModelState.AddModelError(nameof(student.AccountBalance), "Not enough balance in your account.");
        }
        if (billingCard.ExpiryYear != expiryYear)
        {
            // Add validation error for account balance
            TempData["YearError"] = "Wrong expiry year.";

            ModelState.AddModelError(nameof(student.AccountBalance), "Not enough balance in your account.");
        }
            }

    if (!ModelState.IsValid)
    {
                
                return RedirectToAction("CourseBilling", "Billing", new { id = courseid });
            }
            if (ModelState.IsValid)
            {
               //creating billing for student and chosen course
                billing.BillingCard = billingCard;
                billing.IsPurchaseSuccessful = true;
                billing.BillingCardId = billingCard.BillingCardId;
                billing.user = student;
              billing.CreatedAt = DateTime.Now;
                billing.Course = course;
                billing.CourseId=courseid;

                //create course progress and set it to 0
                CourseProgress courseProgress = new CourseProgress();
                courseProgress.StudentId = student.UserId;
                courseProgress.Course = course;
                courseProgress.CourseId = course.CourseId;
                courseProgress.Progress = 0;
                courseProgress.Hours = 0;
                courseProgress.IsCompleted = false;
                courseProgress.LastAccess= DateTime.Now;

                //saving to database
                _context.Add(courseProgress);
            _context.Add(billing);
                student.AccountBalance = (double)(student.AccountBalance - course.Price);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Course", new { id = courseid });

            }

            //Default redirect
            ViewData["BillingCardId"] = new SelectList(_context.BillingCard, "BillingCardId", "BillingCardId", billing.BillingCardId);
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", billing.CourseId);
            return RedirectToAction("Details", "Course", new { id = courseid });

        }

        // GET: Billing/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Billing == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }
            ViewData["BillingCardId"] = new SelectList(_context.BillingCard, "BillingCardId", "BillingCardId", billing.BillingCardId);
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", billing.CourseId);
            return View(billing);
        }

        // POST: Billing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillingId,BillingCardId,CourseId,CreatedAt,IsPurchaseSuccessful")] Billing billing)
        {
            if (id != billing.BillingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingExists(billing.BillingId))
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
            ViewData["BillingCardId"] = new SelectList(_context.BillingCard, "BillingCardId", "BillingCardId", billing.BillingCardId);
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", billing.CourseId);
            return View(billing);
        }

        // GET: Billing/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Billing == null)
            {
                return NotFound();
            }

            var billing = await _context.Billing
                .Include(b => b.BillingCard)
                .Include(b => b.Course)
                .FirstOrDefaultAsync(m => m.BillingId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // POST: Billing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Billing == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Billing'  is null.");
            }
            var billing = await _context.Billing.FindAsync(id);
            if (billing != null)
            {
                _context.Billing.Remove(billing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingExists(int id)
        {
          return _context.Billing.Any(e => e.BillingId == id);
        }



        // TODO: POST: Billing/CourseBilling


        ///GET: Billing/CourseRefund
        public IActionResult CourseRefund()
        {
            return View();
        }

        // TODO: POST: Billing/CourseRefund
    }
}
