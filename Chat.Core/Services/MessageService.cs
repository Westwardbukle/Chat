using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.Message;
using Chat.Common.Exceptions;
using Chat.Common.Message;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly INotificationService _notificationService;

        public MessageService
        (
            IRepositoryManager repository,
            IChatService chatService,
            IMapper mapper,
            INotificationService notificationService)
        {
            _repository = repository;
            _chatService = chatService;
            _mapper = mapper;
            _notificationService = notificationService;
        }


        public async Task<MessagesResponseDto> SendMessageAsync(Guid userId, Guid chatId, string text)
        {
            var checkUser = await _repository.User.GetUserAsync(u => u.Id == userId) is not null;

            var checkChat = await _repository.Chat.GetChatAsync(chatId) is not null;

            if (!checkUser) throw new UserNotFoundException();
            
            if (!checkChat) throw new ChatNotFoundException();

            var message = new MessageModel
            {
                Text = text,
                UserId = userId,
                Type = MessageType.Message,
                ChatId = chatId,
                DispatchTime = DateTime.Now
            };

            await _repository.Message.CreateMessageAsync(message);

            await _repository.SaveAsync();

            var messageDto = _mapper.Map<MessagesResponseDto>(message);

            await _notificationService.NotifyChat(chatId, messageDto);
            
            return messageDto;
        }
        
        public async Task<MessagesResponseDto> SendPersonalMessageAsync(Guid senderId, Guid recipientId, string text)
        {
            if (_repository.User.GetUserAsync(u => u.Id == senderId) is null)
                throw new UserNotFoundException();

            if (_repository.User.GetUserAsync(u => u.Id == recipientId) is null)
                throw new UserNotFoundException();

            var personalChat = await _repository.Chat.GetPersonalChatAsync(senderId, recipientId);


            if (personalChat is null)
            {
                await _chatService.CreatePersonalChatAsync(senderId, recipientId);

                var chat = await _repository.Chat.GetPersonalChatAsync(senderId, recipientId);

                var personalMessage2 = new MessageModel
                {
                    Text = text,
                    UserId = senderId,
                    ChatId = chat.Id,
                    Type = MessageType.Message,
                    DispatchTime = DateTime.Now,
                };

                await _repository.Message.CreateMessageAsync(personalMessage2);
                await _repository.SaveAsync(); 
                
                var messageDto = _mapper.Map<MessagesResponseDto>(personalMessage2);
                
                await _notificationService.NotifyChat(chat.Id, messageDto);
                
                return messageDto;
            }
            else
            {
                var chat = await _repository.Chat.GetPersonalChatAsync(senderId, recipientId);

                var personalMessage = new MessageModel
                {
                    Text = text,
                    UserId = senderId,
                    ChatId = personalChat.Id,
                    Type = MessageType.Message,
                    DispatchTime = DateTime.Now,
                };
                await _repository.Message.CreateMessageAsync(personalMessage);
                await _repository.SaveAsync();
                
                var messageDto = _mapper.Map<MessagesResponseDto>(personalMessage);

                await _notificationService.NotifyChat(chat.Id, messageDto);
                
                return messageDto;
            }
        }

        public async Task<(List<MessagesResponseDto> Data, MetaData MetaData )> GetAllMessagesFromUserToUserAsync(
            Guid userId, Guid senderId, MessagesParameters messagesParameters)
        {
            if (_repository.User.GetUserAsync(u => u.Id == userId) is null ||
                _repository.User.GetUserAsync(u => u.Id == senderId) is null)
            {
                throw new UserNotFoundException();
            }

            var chat = await _repository.Chat.GetPersonalChatAsync(userId, senderId);

            if (chat is null)
            {
                throw new ChatNotFoundException();
            }

            var messages =
                await _repository.Message.FindMessagesByConditionAsync(m => m.ChatId == chat.Id, false, messagesParameters);

            var allMessages = _mapper.Map<List<MessagesResponseDto>>(messages);
            
            return (allMessages, messages.MetaData);
        }
    }
}