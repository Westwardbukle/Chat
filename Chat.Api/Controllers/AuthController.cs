using System.Text.Json;
using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.Token;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITokenService _tokenService;
        private readonly IChatService _chatService;

        public AuthController
        (
            IAuthService authService,
            ITokenService tokenService,
            IChatService chatService
        )
        {
            _authService = authService;
            _tokenService = tokenService;
            _chatService = chatService;
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
        public async Task<IActionResult> Registration(RegisterUserDto registerUserDto)
        {
            await _authService.Registration(registerUserDto);

            return StatusCode(201);
        }


        /// <summary>
        /// User login
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns>Bearer token </returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<TokenModel> Login(LoginUserDto loginUserDto)
            => await _authService.Login(loginUserDto);


        /// <summary>
        ///  Get all chats of user
        /// </summary>
        /// <returns>List with chat names</returns>
        [Authorize]
        [HttpGet("Users/{userid}/chats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> GetAllChatsOfUser([FromQuery] ChatsParameters chatsParameters)
        {
            var userid = _tokenService.GetCurrentUserId();

            var chats = await _chatService.GetAllCommonChatsOfUser(userid, chatsParameters);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(chats.MetaData));

            return Ok(chats.Data);
        }

        /// <summary>
        /// Update user nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="newNick"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Users/{nickname}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUser(string nickname, string newNick)
        {
            await _authService.UpdateUser(nickname, newNick);
            return NoContent();
        }
    }
}