﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Exceptions;
using Chat.Core.Chat;
using Chat.Core.Message;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Manager;
using Chat.Database.Repository.Message;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;

        public MessageService
        (
            IRepositoryManager repository,
            IChatService chatService,
            IMapper mapper
        )
        {
            _repository = repository;
            _chatService = chatService;
            _mapper = mapper;
        }


        public async Task SendMessage(Guid userId, Guid chatId, string text)
        {
            var checkUser = _repository.User.GetUserById(userId) is not null;

            var checkChat = _repository.Chat.GetChatById(chatId) is not null;

            if (!checkUser || !checkChat) throw new UserOrChatNotFoundException();

            var message = new MessageModel
            {
                Text = text,
                UserId = userId,
                ChatId = chatId,
                DispatchTime = DateTime.Now,
            };

            _repository.Message.CreateMessage(message);
            
            await _repository.SaveAsync();
            
        }

        public async Task<List<AllMessagesResponseDto>> GetAllMessageInChat(Guid chatId)
        {
            var chatIsReal = _repository.Chat.GetChat(c => c.Id == chatId) is null;

            if (chatIsReal) throw new ChatNotFoundException();

            var allMessages = _repository.Message.FindMessageByCondition(m => m.ChatId == chatId, false);

            var listMess = _mapper.Map<List<AllMessagesResponseDto>>(allMessages);

            return listMess;
        }


        /*public async Task SendPersonalMessage(Guid senderId, Guid recipientId, string text)
        {
            if (_repository.User.GetUser(u => u.Id == senderId) is null)
                throw new UserNotFoundException();
            
            if (_repository.User.GetUser(u => u.Id == recipientId) is null)
                throw new UserNotFoundException();
            
            _repository.Message.
             

            return new OkObjectResult(chats);
        }*/

        /*public async Task<ActionResult> GetAllMessagesFromUser(Guid senderId, Guid userId )
        {
            
        }*/
    }
}