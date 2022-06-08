using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.Message;
using Chat.Common.Exceptions;
using Chat.Common.Message;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Database.Model;
using Chat.Database.Repository.Manager;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly INotificationService _notificationService;
        private readonly ITokenService _tokenService;

        public MessageService
        (
            IRepositoryManager repository,
            IChatService chatService,
            IMapper mapper,
            INotificationService notificationService, ITokenService tokenService)
        {
            _repository = repository;
            _chatService = chatService;
            _mapper = mapper;
            _notificationService = notificationService;
            _tokenService = tokenService;
        }


        public async Task SendMessage(Guid userId, Guid chatId, string text)
        {
            var checkUser = _repository.User.GetUser(u => u.Id == userId) is not null;

            var checkChat = _repository.Chat.GetChat(c => c.Id == chatId) is not null;

            if (!checkUser || !checkChat) throw new UserOrChatNotFoundException();

            var message = new MessageModel
            {
                Text = text,
                UserId = userId,
                Type = MessageType.Message,
                ChatId = chatId,
                DispatchTime = DateTime.Now
            };

            await _repository.Message.CreateMessage(message);

            await _repository.SaveAsync();

            await _notificationService.NotifyChat(chatId, _mapper.Map<MessagesResponseDto>(message));
        }

        public async Task<(List<MessagesResponseDto> Data, MetaData MetaData)> GetAllMessageInCommonChat(Guid chatId,
            MessagesParameters messagesParameters)
        {
            var chatIsReal = _repository.Chat.GetChat(c => c.Id == chatId) is null;

            if (chatIsReal) throw new ChatNotFoundException();

            var allMessages = await _repository.Message.FindMessagesByCondition(
                m => m.ChatId == chatId, false, messagesParameters);

            var listMess = _mapper.Map<List<MessagesResponseDto>>(allMessages);

            return (Data: listMess, MetaData: allMessages.MetaData);
        }


        public async Task SendPersonalMessage(Guid senderId, Guid recipientId, string text)
        {
            if (_repository.User.GetUser(u => u.Id == senderId) is null)
                throw new UserNotFoundException();

            if (_repository.User.GetUser(u => u.Id == recipientId) is null)
                throw new UserNotFoundException();

            var personalChat = await _repository.Chat.GetPersonalChat(senderId, recipientId);


            if (personalChat is null)
            {
                await _chatService.CreatePersonalChat(senderId, recipientId);

                var chat = await _repository.Chat.GetPersonalChat(senderId, recipientId);

                var personalMessage2 = new MessageModel
                {
                    Text = text,
                    UserId = senderId,
                    ChatId = chat.Id,
                    Type = MessageType.Message,
                    DispatchTime = DateTime.Now,
                };

                await _repository.Message.CreateMessage(personalMessage2);
                await _repository.SaveAsync();
                await _notificationService.NotifyChat(chat.Id, _mapper.Map<MessagesResponseDto>(personalMessage2));
            }
            else
            {
                var chat = await _repository.Chat.GetPersonalChat(senderId, recipientId);

                var personalMessage = new MessageModel
                {
                    Text = text,
                    UserId = senderId,
                    ChatId = personalChat.Id,
                    Type = MessageType.Message,
                    DispatchTime = DateTime.Now,
                };
                await _repository.Message.CreateMessage(personalMessage);
                await _repository.SaveAsync();

                await _notificationService.NotifyChat(chat.Id, _mapper.Map<MessagesResponseDto>(personalMessage));
            }
        }

        public async Task<(List<MessagesResponseDto> Data, MetaData MetaData )> GetAllMessagesFromUserToUser(
            Guid userId, Guid senderId, MessagesParameters messagesParameters)
        {
            if (_repository.User.GetUser(u => u.Id == userId) is null ||
                _repository.User.GetUser(u => u.Id == senderId) is null)
            {
                throw new UserNotFoundException();
            }

            var chat = await _repository.Chat.GetPersonalChat(userId, senderId);

            if (chat is null)
            {
                throw new ChatNotFoundException();
            }

            var messages =
                await _repository.Message.FindMessagesByCondition(m => m.ChatId == chat.Id, false, messagesParameters);

            var allMessages = _mapper.Map<List<MessagesResponseDto>>(messages);

            return (allMessages, messages.MetaData);
        }
    }
}