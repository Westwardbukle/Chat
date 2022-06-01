﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Dto.User;
using Chat.Core.Abstract;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("/api/v{version:apiVersion}/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IAuthService _authService;

        public ChatController
        (
            IChatService chatService,
            IMessageService messageService,
            IAuthService authService
        )
        {
            _chatService = chatService;
            _messageService = messageService;
            _authService = authService;
        }

        /// <summary>
        /// Create personal chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost("type/personal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> CreatePersonalChat(CreatePersonalChatDto createPersonalChat)
        {
            await _chatService.CreatePersonalChat(createPersonalChat.User1, createPersonalChat.User2);

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Create common chat
        /// </summary>
        /// <param name="commonChatDto"></param>
        /// <returns></returns>
        [HttpPost("type/common")]
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
        ///  Get all users in chat
        /// </summary>
        /// <returns></returns>
        [HttpGet("/{chatId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IEnumerable<GetAllUsersDto>> GetAllUsersInChat(Guid chatId)
            => await _authService.GetAllUsersInChat(chatId);
        

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
        public async Task<ActionResult> UpdateChat( Guid chatId, [FromBody] NewNameChatDto newNameChat)
        {
            await _chatService.UpdateChat(chatId, newNameChat.Name);

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
        public async Task<ActionResult> RemoveUserInChat( Guid userId,  Guid chatId)
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
        public async Task<ActionResult> SendMessage( Guid chatId, SendMessageDto sendMessage)
        {
            await _messageService.SendMessage(sendMessage.UserId, chatId, sendMessage.Text);

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
        public async Task<ActionResult> GetAllMessageInChat( Guid chatId)
        {
            var messages = await _messageService.GetAllMessageInChat(chatId);

            return Ok(messages);
        }
    }
}