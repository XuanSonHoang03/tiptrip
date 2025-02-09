using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TipTrip.Application.Implements.Interfaces;
using TipTrip.Common.Models;

namespace TipTrip.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IMeAuthenticationService _meAuthenticationService;

        public RegisterModel(IMeAuthenticationService meAuthenticationService)
        {
            _meAuthenticationService = meAuthenticationService;
        }

        [BindProperty]
        public RegisterDTO registerData { get; set; } = new RegisterDTO();

        public string message { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _meAuthenticationService.Register(registerData);

            if((bool)result.Success)
            {
                return RedirectToPage("/Auth/Login");
            }
            message = result.Message;
            return Page();
        }
    }
}
