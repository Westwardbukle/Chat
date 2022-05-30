using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Core.Auth;
using Chat.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
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
        /// <response code="201">User added</response>
        /// <response code="422">Validation Exception</response>
        /// <response code="400">User already registered </response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Registration([FromBody] RegisterUserDto registerUserDto) 
            =>await _authService.Registration(registerUserDto);

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
            => await _authService.Login(loginUserDto);
        
        /// <summary>
        ///  Get all users
        /// </summary>
        /// <returns></returns>
        [HttpPost("Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> GetAllUsers()
            => await _authService.GetAllUsers();
        
        /// <summary>
        /// Update user nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="newNick"></param>
        /// <returns></returns>
        [HttpPut("Users/{nickname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> UpdateUser(string nickname, string newNick)
            => await _authService.UpdateUser(nickname, newNick);
    }
}