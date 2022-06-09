using System;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
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
    [Authorize]
    [Route("/api/v{version:apiVersion}/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// Create common chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> CreateCommonChat(CreateCommonChatDto commonChatDto)
        {
           var chat = await _chatService.CreateCommonChat(commonChatDto);

            return Created("", chat);
        }

        /// <summary>
        /// Invite Users in chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="inviteUserCommonChatDto"></param>
        /// <returns></returns>
        [HttpPost("{chatId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> InviteUserToCommonChat(Guid chatId,
            InviteUserCommonChatDto inviteUserCommonChatDto)
        {
            var userChat = await _chatService.InviteUserToCommonChat(chatId, inviteUserCommonChatDto);

            return Created("", userChat);
        }

        /// <summary>
        ///  Get all users in chat
        /// </summary>
        /// <returns></returns>
        [HttpGet("/{chatId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> GetAllUsersInChat(Guid chatId, [FromQuery] UsersParameters usersParameters)
        {
            var result = await _chatService.GetAllUsersInChat(chatId, usersParameters);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));
            
            return Ok(result.Data);
        } 



        /// <summary>
        /// UpdateChat
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> UpdateChat(Guid chatId, [FromBody] NewNameChatDto newNameChat)
        {
            await _chatService.UpdateChat(chatId, newNameChat.Name);

            return Ok();
        }

        /// <summary>
        /// Delete user in chat
        /// </summary>
        /// <param name="remoteUserId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [HttpDelete("{chatId}/users/{remoteUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> RemoveUserInChat(Guid remoteUserId, Guid chatId)
        {
            await _chatService.RemoveUserInChat(remoteUserId, chatId);

            return Ok();
        }
        
        /// <summary>
        /// Get all messages in chat
        /// </summary>
        /// <param name="chat id"></param>
        /// <returns> ListMessages in chat</returns>
        [HttpGet("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> GetAllMessageInChat(Guid chatId, [FromQuery] MessagesParameters messagesParameters)
        {
            var messages = await _chatService.GetAllMessageInCommonChat(chatId, messagesParameters );
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(messages.MetaData));
            
            return Ok(messages.Data);
        }
    }
}