using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
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
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;

        public UserController(ITokenService tokenService, IChatService chatService, IUserService userService)
        {
            _tokenService = tokenService;
            _chatService = chatService;
            _userService = userService;
        }

        /// <summary>
        ///  Get all chats of user
        /// </summary>
        /// <returns>List with chat names</returns>
        [Authorize]
        [HttpGet("{userid}/chats")]
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
        [HttpPut("{nickname}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUser(string nickname, string newNick)
        {
            await _userService.UpdateUser(nickname, newNick);
            return NoContent();
        }

        /// <summary>
        ///  Get user
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{nickname}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetUser(string nickname)
        {
            var user = await _userService.GetOneUser(nickname);

            return Ok(user);
        }
        
        /// <summary>
        /// Send requset user
        /// </summary>
        /// <param name="recipientId"></param>
        /// <param name="personalMessage"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{recipientId}/request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendFriendRequest([Required]Guid recipientId)
        {
            var request = await _userService.SendFriendRequest(recipientId);

            return Created("",request);
        }
    }
}