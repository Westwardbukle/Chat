using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Chat;
using Chat.Common.Dto.Message;
using Chat.Core.Chat;
using Chat.Core.Message;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Message;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatService _chatService;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserChatRepository _userChatRepository;

        public MessageService
        (
            IMessageRepository messageRepository,
            IChatService chatService,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IUserChatRepository userChatRepository
        )
        {
            _messageRepository = messageRepository;
            _chatService = chatService;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userChatRepository = userChatRepository;
        }


        public async Task<ActionResult> SendMessage(Guid userId, Guid chatId, string text)
        {
            var checkUser = _userRepository.GetUserById(userId) is not null;

            var checkChat = _chatRepository.GetChatById(chatId) is not null;

            if (!checkUser || !checkChat) return new StatusCodeResult(StatusCodes.Status400BadRequest);

            var message = new MessageModel
            {
                Text = text,
                UserId = userId,
                ChatId = chatId,
                DispatchTime = DateTime.Now,
            };
            
            _messageRepository.CreateMessage(message);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        public async Task<ActionResult> GetAllMessageInChat(Guid chatId)
        {
            var chatIsReal = _chatRepository.GetChat(c => c.Id == chatId) is null;

            if (chatIsReal) return new StatusCodeResult(StatusCodes.Status400BadRequest);

            var allMessages = _messageRepository.FindMessageByCondition(m => m.ChatId == chatId, false);

            var listMess = _mapper.Map<List<AllMessagesResponseDto>>(allMessages);

            return new OkObjectResult(listMess);
        }


        /*public async Task<ActionResult> SendPersonalMessage(Guid senderId, Guid recipientId, string text)
        {
            if (_userRepository.GetOne(u => u.Id == senderId) is null)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            if (_userRepository.GetOne(u => u.Id == recipientId) is null)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            
             

            return new OkObjectResult(chats);
        }*/

        /*public async Task<ActionResult> GetAllMessagesFromUser(Guid senderId, Guid userId )
        {
            
        }*/
    }
}