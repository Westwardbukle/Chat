using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Result;
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


        /// <summary>
        /// Send message in common chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost("chat/{chatId}/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> SendMessage(Guid userId, Guid chatId,[Required] string text)
            => await _messageService.SendMessage(userId, chatId, text);

    }
}