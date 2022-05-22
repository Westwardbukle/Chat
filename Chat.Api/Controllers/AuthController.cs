using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Result;
using Chat.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize]
    [Route("/api/v{version:apiVersion}/[controller]")]
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

        /// <summary>
        /// User registrarion
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <response code="200">Return bearer</response>
        /// <response code="400">Validation Exception</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Registration([FromBody] RegisterUserDto registerUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Registration(registerUserDto));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Login(loginUserDto));
    }
}