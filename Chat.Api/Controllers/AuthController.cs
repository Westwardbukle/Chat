using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Result;
using Chat.Core.Auth;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
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
        /// <response code="422">Validation Exception</response>
        /// <response code="400">user already registered </response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Registration([FromBody] RegisterUserDto registerUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Registration(registerUserDto));


        /// <summary>
        /// User login
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns>Bearer token </returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Login(loginUserDto));

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Verify()
        {
            return Ok();
        }
    }
}