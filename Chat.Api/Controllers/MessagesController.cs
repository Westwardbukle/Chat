using System;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ITokenService _tokenService;

        public MessagesController
        (
            IMessageService messageService,
            ITokenService tokenService
        )
        {
            _messageService = messageService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Send Personal message
        /// </summary>
        /// <param name="recipientId"></param>
        /// <param name="personalMessage"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("users/{recipientId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendPersonalMessage(Guid recipientId,[FromBody] PersonalMessageDto personalMessage)
        {
            var senderId = _tokenService.GetCurrentUserId();
            var message = await _messageService.SendPersonalMessage(senderId, recipientId, personalMessage.Text);

            return Created("", message);
        }

        /// <summary>
        ///  Get messages for personal chat
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="senderId"></param>
        /// <param name="messagesParameters"></param>
        /// <returns></returns>
        [HttpGet("{userId}/messages/{senderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllMessagesFromUserToUser(Guid userId, Guid senderId, [FromQuery] MessagesParameters messagesParameters)
        {
           var messages =  await _messageService.GetAllMessagesFromUserToUser(userId, senderId, messagesParameters);
           
           Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(messages.MetaData));

           return Ok(messages.Data);
        }

        /// <summary>
        /// Send message in common chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="sendMessage"></param>
        /// <returns></returns>
        [HttpPost("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> SendMessage(Guid chatId, SendMessageDto sendMessage)
        {
           var message =  await _messageService.SendMessage(sendMessage.UserId, chatId, sendMessage.Text);

            return Created("", message);
        }
    }
}