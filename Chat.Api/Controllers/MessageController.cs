using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Database.Model;
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
        public async Task<ActionResult> SendPersonalMessage(Guid recipientId,[FromBody] PersonalMessageDto personalMessage)
        {
            var senderId = _tokenService.GetCurrentUserId();
            await _messageService.SendPersonalMessage(senderId, recipientId, personalMessage.Text);

            return StatusCode(201);
        }

        /// <summary>
        ///  Get messages for personal chat
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        [HttpGet("{userId}/messages/{senderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllMessagesFromUserToUser(Guid userId, Guid senderId, [FromQuery] MessagesFeatures messagesFeatures)
        {
           var messages =  await _messageService.GetAllMessagesFromUserToUser(userId, senderId, messagesFeatures);
           
           Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(messages.MetaData));

           return Ok(messages.Data);
        }  
    }
}