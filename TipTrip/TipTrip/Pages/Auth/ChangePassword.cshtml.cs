using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TipTrip.Common.Models;
using System.Threading.Tasks;

namespace TipTrip.Pages.Auth
{
	public class ChangePasswordModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;

		public ChangePasswordModel(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		[BindProperty]
		public ChangePasswordDTO ChangePasswordData { get; set; } = new ChangePasswordDTO();

		public string Message { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _userManager.GetUserAsync(User); // Lấy user đã đăng nhập
			if (user == null)
			{
				Message = "User not found.";
				return RedirectToPage("/Auth/Login");
			}

			// Kiểm tra mật khẩu cũ có chính xác không
			var result = await _userManager.ChangePasswordAsync(user, ChangePasswordData.OldPassword, ChangePasswordData.NewPassword);
			if (result.Succeeded)
			{
				Message = "Your password has been changed successfully.";
				return RedirectToPage("/Home/Index"); // Chuyển hướng về trang chủ hoặc trang khác
			}

			Message = "Error changing password: " + string.Join(", ", result.Errors.Select(e => e.Description));
			return Page();
		}
	}
}
