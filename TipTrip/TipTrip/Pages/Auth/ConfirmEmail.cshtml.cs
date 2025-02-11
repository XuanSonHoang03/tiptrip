using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TipTrip.Pages.Auth
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ConfirmEmailModel> _logger;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, ILogger<ConfirmEmailModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                Message = "Invalid email confirmation request.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Message = "User not found.";
                return Page();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                Message = "Your email has been confirmed! You can now log in.";
                _logger.LogInformation($"User {user.Email} confirmed email successfully.");
            }
            else
            {
                Message = "Email confirmation failed. The link may be expired or invalid.";
                _logger.LogWarning($"Email confirmation failed for user {user.Email}.");
            }

            return Page();
        }
    }
}
