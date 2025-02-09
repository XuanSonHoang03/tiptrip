using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TipTrip.Common.Models;

namespace TipTrip.Application.Implements.Interfaces
{
    public interface IMeAuthenticationService
    {
        Task<BooleanResponse> Register(RegisterDTO registerDTO);

        Task<BooleanResponse> Login(LoginDTO registerDTO);

        void Logout();
    }
}
