using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Core.Chat;
using Chat.Core.Message;
using Chat.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatController
        (
            IChatService chatService,
            IMessageService messageService
        )
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        /// <summary>
        /// Create personal chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost("private")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> CreatePersonalChat(Guid user1, Guid user2)
        {
            await _chatService.CreatePersonalChat(user1, user2);

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Create common chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost("common")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> CreateCommonChat(CreateCommonChatDto commonChatDto)
        {
            var chats = _chatService.CreateCommonChat(commonChatDto);

            return Ok(chats);
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
            await _chatService.InviteUserToCommonChat(chatId, inviteUserCommonChatDto);

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        ///  Get all chats
        /// </summary>
        /// <returns>List with chat names</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> GetAllChats()
        {
            var chats = await _chatService.GetAllCommonChats();
            return Ok(chats);
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
        public async Task<ActionResult> UpdateChat([Required] Guid chatId, [Required] string name)
        {
            await _chatService.UpdateChat(chatId, name);

            return Ok();
        }

        /// <summary>
        /// Delete user in chat
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [HttpDelete("{chatId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> RemoveUserInChat([Required] Guid userId, [Required] Guid chatId)
        {
            await _chatService.RemoveUserInChat(userId, chatId);

            return Ok();
        }

        /// <summary>
        /// Send message in common chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> SendMessage(Guid userId, Guid chatId, [Required] string text)
        {
            await _messageService.SendMessage(userId, chatId, text);

            return StatusCode(StatusCodes.Status201Created);
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
        public async Task<ActionResult> GetAllMessage(Guid chatId)
        {
            var messages = await _messageService.GetAllMessageInChat(chatId);

            return Ok(messages);
        }
    }
}