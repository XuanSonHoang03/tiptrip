using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TipTrip.Application.Implements.Interfaces;
using System.Text.Encodings.Web;
using TipTrip.Services;

namespace TipTrip.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email)) return Page();

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                Message = "If your email exists, you will receive a reset link.";
                return Page();
            }

            // Tạo token reset password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Tạo link reset password
            var resetLink = Url.Page(
                "/Auth/ResetPassword",
                pageHandler: null,
                values: new { userId = user.Id, token = token },
                protocol: Request.Scheme);

            // Gửi email reset password
            var subject = "Reset Your Password - TipTrip";
            var body = $"Click <a href='{HtmlEncoder.Default.Encode(resetLink)}'>here</a> to reset your password.";
            await _emailService.SendEmailAsync(Email, subject, body);

            Message = "If your email exists, you will receive a reset link.";
            return Page();
        }
    }
}
