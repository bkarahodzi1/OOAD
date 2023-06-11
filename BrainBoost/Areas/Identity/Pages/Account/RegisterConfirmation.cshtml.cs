using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System;

namespace BrainBoost.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Enter a six-digit number:")]
            [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a valid six-digit number.")]
            public string VerificationCode { get; set; }
        }

        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync(string email, int code, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            if (ModelState.IsValid)
            {
                // Checking if the given input matches sent verification code
                if (Input.VerificationCode == null)
                {
                    TempData["ErrorMessage"] = "Invalid verification code.";
                    return Page();
                }
                else if (code == int.Parse(Input.VerificationCode))
                {
                    return RedirectToPage("ConfirmEmail", new { userid = user.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid verification code.";
                    return Page();
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid verification code.";
                return Page();
            }
            
        }
    }
}
