using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Core.Message;
using Chat.Validation;
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

        public MessageController
        (
            IMessageService messageService
        )
        {
            _messageService = messageService;
        }

        /*/// <summary>
        ///  
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        [HttpPost("users/{senderId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendPersonalMessage(Guid senderId, Guid recipientId, string text)
            => await _messageService.SendPersonalMessage(senderId, recipientId, text);
        
        /// <summary>
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