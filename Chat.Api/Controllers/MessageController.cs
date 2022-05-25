using Chat.Common.Result;
using Chat.Core.Message;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController
        (
            IMessageService messageService
        )
        {
            _messageService = messageService;
        }
        
        /*public async ResultContainer<ActionResult> CreateMessage()
        => await _messageService.*/

    }
}