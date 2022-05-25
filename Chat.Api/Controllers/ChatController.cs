using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Core.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatController
        (
            IChatService chatService
            
        )
        {
            _chatService = chatService;
        }
        
        /*public async Task<ActionResult> CreateChat(ChatRequestDto chatDto)
        =>await  _chatService

        
        public async Task<ActionResult> DeleteChat(ChatRequestDto chatDto)
            =>await  _chatService
        
        public async Task<ActionResult> CreateCommonChat(ChatRequestDto chatDto)
            =>await  _chatService*/
    }
}