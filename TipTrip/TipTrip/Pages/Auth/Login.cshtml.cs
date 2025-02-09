using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TipTrip.Application.Implements.Interfaces;
using TipTrip.Common.Models;

namespace TipTrip.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IMeAuthenticationService _meAuthenticationService;

        public LoginModel(IMeAuthenticationService meAuthenticationService)
        {
            _meAuthenticationService = meAuthenticationService;
        }

        [BindProperty]
        public LoginDTO loginData { get; set; } = new LoginDTO();

        public string message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _meAuthenticationService.Login(loginData);

            if ((bool)result.Success)
            {
                return RedirectToAction("Index", "Home");
            }

			message = result.Message;
			return Page();
		}


    }
}
