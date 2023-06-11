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



        // GET: Quiz/Quiz
        public async Task<IActionResult> Quiz(int courseMaterialId)
        {
            var courseMaterial = await _context.CourseMaterial.FindAsync(courseMaterialId);

            if (courseMaterial == null)
            {
                // Course material not found, handle the error accordingly
                return NotFound();
            }

            var quiz = await _context.Quiz
                .FirstOrDefaultAsync(q => q.CourseMaterialId == courseMaterialId);

            var questions = await _context.Question
            .Where(q => q.QuizId == quiz.QuizId).ToListAsync();

            if (quiz == null)
            {
                // Quiz not found, handle the error accordingly
                return NotFound();
            }

            return View(new QuizViewModel
            {
                Quiz = quiz,
                allQuestions=questions
            });
        }

        // POST: /Quiz/SubmitQuiz
        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(Dictionary<int, string> answers)
        { int correct = 0;
            int incorrect = 0;
            // Process the submitted answers
            foreach (var answersItem in answers) {
                var questionId = answersItem.Key;
                var userAnswer = answersItem.Value ?? string.Empty;

                // Retrieve the correct answer from the database for the question
                var correctAnswer = await _context.QuestionAnswer
            .Where(a => a.QuestionId == questionId && (bool?)a.IsCorrect == true)
            .Select(a => a.Answer )
            .FirstOrDefaultAsync();

                if (string.Equals(userAnswer, correctAnswer, StringComparison.OrdinalIgnoreCase))
                    correct++;
                
                else incorrect++;
            }

            return RedirectToAction(nameof(QuizAnswers), new { correctCount = correct, incorrectCount = incorrect });

        }

        public IActionResult QuizAnswers( int correctCount, int incorrectCount)
        {
            // Show the quiz result page
            var viewModel = new QuizAnswersViewModel
            {
                CorrectCount = correctCount,
                IncorrectCount = incorrectCount
            };

            // Pass the view model to the view
            return View(viewModel);
        }


    }

    public class QuizViewModel
    {
        public Quiz Quiz { get; set; }
        public List<Question> allQuestions { get; set; }
    }
    public class QuizAnswersViewModel
    {
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
    }
}
