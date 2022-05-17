using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Result;
using Chat.Core.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    public class AuthController : BaseController
    {
        
        private readonly IAuthService _authService;

        public AuthController
        (
            IAuthService authService
        )
        {
            _authService = authService;
        }
        
        public async Task<ActionResult> Registration(RegisterUserDto registerUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Registration(registerUserDto));

        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Login(loginUserDto));
    }
}