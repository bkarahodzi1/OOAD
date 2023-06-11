using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;

namespace BrainBoost.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private static Random random = new Random();

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        private static string GenerateRandomString()
        {
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numericChars = "0123456789";

            var random = new Random();

            var randomString = new StringBuilder();
            randomString.Append(lowercaseChars[random.Next(lowercaseChars.Length)]);
            randomString.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);
            randomString.Append(numericChars[random.Next(numericChars.Length)]);

            const string allChars = lowercaseChars + uppercaseChars + numericChars;

            for (int i = 3; i < 8; i++)
            {
                randomString.Append(allChars[random.Next(allChars.Length)]);
            }

            return randomString.ToString();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                
                string novaSifra = GenerateRandomString();

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, novaSifra);

                if(result.Succeeded)
                {
                    string body = $@"<html>
                            <body>
                                <p>Dear User,</p>
                                <p>This is to inform you that your password has been successfully reset in our system. Please take note of your new password:</p>
                                <p>Newly generated password: " + novaSifra + "</p> <p>After logging into the system, we recommend changing your password through your user profile.</p><p>Thank you for using our system.</p><p>Best regards,</p><p>Support Team</p></body></html>";
                    await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Password Reset Notification",
                    body);
                }
                

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
