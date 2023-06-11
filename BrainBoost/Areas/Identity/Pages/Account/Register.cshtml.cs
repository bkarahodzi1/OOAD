using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BrainBoost.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BrainBoost.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private ApplicationDbContext context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext c)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            context = c;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Display(Name = "Role")]
            public string Role { get; set; }

            [Display(Name = "Date of birth")]
            [DataType(DataType.Date)]
            public DateTime BirthDate { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        private int generateCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Home/HomeCourses");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var imaUBazi = await _userManager.FindByEmailAsync(Input.Email);
                var dupliUsername = await _userManager.FindByNameAsync(Input.Username);
                if(imaUBazi == null && dupliUsername == null)
                {
                    var user = new IdentityUser { UserName = Input.Username, Email = Input.Email };
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        string combo = Request.Form["role"].ToString();
                        Models.User u;
                        if (combo == "Student")
                        {
                            u = new Models.Student();
                        }
                        else
                        {
                            u = new Models.Professor();
                        }
                        u.FirstName = Input.FirstName;
                        u.LastName = Input.LastName;
                        u.Email = Input.Email;
                        u.BirthDate = Input.BirthDate;
                        u.CreatedAt = DateTime.Now;
                        u.IsVerified = true;
                        u.Username = Input.Username;
                        context.User.Add(u);
                        context.SaveChanges();

                        var rola = await _userManager.FindByNameAsync(Input.Username);
                        await _userManager.AddToRoleAsync(rola, combo);


                        _logger.LogInformation("User created a new account with password.");

                        int code = generateCode();

                        string body = "Dear User,<br/><br/>Thank you for registering on our platform. To complete the registration process, we kindly ask you to confirm your email address.<br/><br/>Your verification code is: " + code + "<br/><br/>If you did not initiate this registration, please disregard this message.<br/><br/>Thank you for your cooperation.<br/><br/>Best regards,<br/>Your Support Team";

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm Your Email Adress", body);

                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, code = code, returnUrl = returnUrl });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else if(imaUBazi != null)
                {
                    TempData["EmailPostoji"] = "This email has already been taken.";
                }
                else
                {
                    TempData["EmailPostoji"] = "This username has already been taken.";
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
