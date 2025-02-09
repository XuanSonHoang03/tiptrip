using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TipTrip.Application.Implements.Interfaces;
using TipTrip.Common.Models;
using System.Security.Claims;

namespace TipTrip.Application.Implements.Services
{
    public class AuthenticationService : IMeAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<IdentityUser> userManager,
                                     SignInManager<IdentityUser> signInManager,
                                     IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BooleanResponse> Register(RegisterDTO registerDTO)
        {
            var user = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (user != null)
            {
                return new BooleanResponse { Success = false, Message = "Email already exists!" };
            }

            user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, registerDTO.Role);
                if (!roleResult.Succeeded)
                {
                    return new BooleanResponse
                    {
                        Success = false,
                        Message = "Failed to assign role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description))
                    };
                }

                return new BooleanResponse
                {
                    Success = true,
                    Message = "User registered successfully!"
                };
            }

            return new BooleanResponse
            {
                Success = false,
                Message = "Invalid data: " + string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        public async Task<BooleanResponse> Login(LoginDTO loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Username);
            if (user == null)
            {
                return new BooleanResponse { Success = false, Message = "Invalid username or password." };
            }

            var correctPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!correctPassword)
            {
                return new BooleanResponse { Success = false, Message = "Invalid username or password." };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Save information of user to Session
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("UserId", user.Id);
            session.SetString("UserName", user.UserName);
            session.SetString("UserEmail", user.Email);
            session.SetString("UserRoles", string.Join(",", userRoles)); 

            return new BooleanResponse
            {
                Success = true,
                Message = "Login successful!"
            };
        }

        public void Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear(); // Logout
        }
    }
}
