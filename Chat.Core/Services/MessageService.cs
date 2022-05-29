using System;
using System.Threading.Tasks;
using Chat.Core.Chat;
using Chat.Core.Message;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Message;
using Chat.Database.Repository.User;
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

        public MessageService
        (
            IMessageRepository messageRepository,
            IChatService chatService,
            IChatRepository chatRepository,
            IUserRepository userRepository
        )
        {
            _messageRepository = messageRepository;
            _chatService = chatService;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }


        public async Task<ActionResult> SendMessage(Guid userId, Guid chatId, string text)
        {
            var checkUser = _userRepository.GetById(userId) is not null;

            var checkChat = _chatRepository.GetById(chatId) is not null;

            if (!checkUser || !checkChat) return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            var message = new MessageModel
            {
                Text = text,
                UserId = userId,
                ChatId = chatId,
                DispatchTime = DateTime.Now,
            };

            await _messageRepository.Create(message);
                
            return new StatusCodeResult(StatusCodes.Status201Created);

        }

        /*public async Task<ResultContainer<MessageResponseDto>> GetAllMessageInChat(Guid chatId)
        {
            var result = new ResultContainer<MessageResponseDto>();
            
            
            

            return result;
        }*/
        
        
        
        
    }
}