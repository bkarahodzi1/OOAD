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
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["BillingCardId"] = new SelectList(_context.BillingCard, "BillingCardId", "BillingCardId");
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId");
            return View();
        }

        // POST: Billing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillingId,BillingCardId,CourseId,CreatedAt,IsPurchaseSuccessful")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillingCardId"] = new SelectList(_context.BillingCard, "BillingCardId", "BillingCardId", billing.BillingCardId);
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", billing.CourseId);
            return View(billing);
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


        ///GET: Billing/CourseBilling
        public IActionResult CourseBilling()
        {
            return View();
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
