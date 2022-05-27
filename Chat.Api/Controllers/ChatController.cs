using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Chat.Common.Chat;
using Chat.Common.Dto;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Result;
using Chat.Core.Chat;
using Chat.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/chats")]
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

        /*/// <summary>
        ///  
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        [HttpPost("users/{recepientId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendPersonalMessage(Guid recepientId, PersonalSenderDto personalSenderDto )
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.CreatePersonalChat( recepientId, personalSenderDto ));*/


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
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.CreateCommonChat(commonChatDto));

        /*/// <summary>
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
            => ReturnResult<>(_chatService.InviteUserToCommonChat(chatId, inviteUserCommonChatDto))*/

        /// <summary>
        ///  Get all chats
        /// </summary>
        /// <returns>List with chat names</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> GetAllChats()
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.GetAllChats());

        
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
        public async Task<ActionResult> UpdateChat(Guid chatId, string name)
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.UpdateChat(chatId, name));
        
        
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
        public async Task<ActionResult> RemoveUserInChat(Guid userId, Guid chatId)
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.RemoveUserInChat(userId, chatId));
        
    }
}