using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Core.Message;
using Chat.Core.Token;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ITokenService _tokenService;

        public MessageController
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
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("users/{recipientId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendPersonalMessage(Guid recipientId,PersonalMessageDto personalMessage)
        {
            var senderId = _tokenService.GetCurrentUserId();
            await _messageService.SendPersonalMessage(senderId, recipientId, personalMessage.Text);

            return StatusCode(201);
        } 
        
        /*/// <summary>
        ///  
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        [HttpPost("{userId}/messages/{senderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllMessagesFromUserToUser(Guid userId, Guid senderId, string text)
            => await _messageService.SendPersonalMessage(senderId, senderId, text);*/
    }
}