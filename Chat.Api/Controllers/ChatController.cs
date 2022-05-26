using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Chat.Common.Chat;
using Chat.Common.Dto;
using Chat.Common.Dto.Chat;
using Chat.Common.Result;
using Chat.Core.Chat;
using Chat.Validation;
using Microsoft.AspNetCore.Http;
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        [HttpPost("users/{recepientId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendPersonalMessage(Guid recepientId,/*Дто с получателем и тектом*/  )
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.CreatePersonalChat( recepientId, ));

        [HttpPost("chats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateCommonChat(CreateCommonChatDto commonChatDto)
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.CreateCommonChat(commonChatDto));

        /*public async Task<ActionResult> Conversation(ChatRequestDto chatDto)
            =>await  _chatService#2##1#*/
    }
}