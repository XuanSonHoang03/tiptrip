using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TipTrip.Application.Implements.Interfaces;
using TipTrip.Common.Models;
using System.Text.Encodings.Web;
using TipTrip.Services;

namespace TipTrip.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IMeAuthenticationService _meAuthenticationService;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(IMeAuthenticationService meAuthenticationService,
                             IEmailService emailService,
                             UserManager<IdentityUser> userManager,
                             ILogger<RegisterModel> logger)
        {
            _meAuthenticationService = meAuthenticationService;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public RegisterDTO registerData { get; set; } = new RegisterDTO();

        public string message { get; set; }
        public bool showEmailSentMessage { get; set; } = false;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                registerData.Role ??= "User";

                var result = await _meAuthenticationService.Register(registerData);

                if ((bool)result.Success)
                {
                    var user = await _userManager.FindByEmailAsync(registerData.Email);
                    if (user == null)
                    {
                        message = "User not found after registration!";
                        return Page();
                    }

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Page(
                        "/Auth/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, token = token },
                        protocol: Request.Scheme);

                    var emailSubject = "Confirm Your Email - TipTrip";
                    var emailBody = $"<p>Click <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>here</a> to confirm your email.</p>";

                    await _emailService.SendEmailAsync(registerData.Email, emailSubject, emailBody);

                    _logger.LogInformation($"Email confirmation link sent to {registerData.Email}");

                    // ✅ Hiển thị thông báo kiểm tra email thay vì chuyển hướng
                    message = "A confirmation email has been sent. Please check your email to verify your account.";
                    showEmailSentMessage = true;
                    return Page();
                }

                message = result.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registration: {ex.Message}");
                message = "An error occurred. Please try again later.";
            }

            return Page();
        }
    }
}
