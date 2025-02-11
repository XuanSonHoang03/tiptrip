using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TipTrip.Pages.Auth
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId, string token)
		{
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
			{
				Message = "Get chưa nhận đưuọc dữ liệu";
				return Page();
			}

			UserId = userId;
			Token = token;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
        {
            if (NewPassword != ConfirmPassword)
            {
                Message = "Passwords do not match!";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                Message = "Không tìm thấy UserId khi Post";
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Token, NewPassword);
            if (result.Succeeded)
            {
                Message = "Your password has been reset successfully.";
                return RedirectToPage("/Auth/Login");
            }

            Message = "Error resetting password. Try again.";
            return Page();
        }
    }
}
