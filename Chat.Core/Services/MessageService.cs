using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.Result;
using Chat.Core.Chat;
using Chat.Core.Message;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Message;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatService _chatService;
        private readonly IChatRepository _chatRepository;

        public MessageService
        (
            IMessageRepository messageRepository,
            IChatService chatService,
            IChatRepository chatRepository
        )
        {
            _messageRepository = messageRepository;
            _chatService = chatService;
            _chatRepository = chatRepository;
        }


        public async Task<ResultContainer<MessageResponseDto>> SendMessage(Guid userId, Guid chatId, string text)
        {
            var result = new ResultContainer<MessageResponseDto>();

            var message = new MessageModel
            {
                Text = text,
                UserId = userId,
                ChatId = chatId,
                DispatchTime = DateTime.Now,
            };

            await _messageRepository.Create(message);

            return result;
        }

        /*public async Task<ResultContainer<MessageResponseDto>> GetAllMessageInChat(Guid chatId)
        {
            var result = new ResultContainer<MessageResponseDto>();
            
            
            

            return result;
        }*/
        
        
        
        
    }
}