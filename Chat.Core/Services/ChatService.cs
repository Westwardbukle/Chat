﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Chat;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Dto.User;
using Chat.Common.Dto.UserChat;
using Chat.Common.Exceptions;
using Chat.Common.Message;
using Chat.Common.RequestFeatures;
using Chat.Common.UsersRole;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ITokenService _tokenService;

        public ChatService
        (
            IRepositoryManager repository,
            IMapper mapper, INotificationService notificationService, ITokenService tokenService)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _tokenService = tokenService;
        }


        public async Task CreatePersonalChatAsync(Guid user1, Guid user2)
        {
            var chatId = Guid.NewGuid();

            var userChats = new List<UserChatModel>();

            if (await _repository.User.GetUserAsync(u => u.Id == user1) is null ||
                await _repository.User.GetUserAsync(u => u.Id == user2) is null)
            {
                throw new UserNotFoundException();
            }

            var userChat1 = new UserChatModel
            {
                UserId = user1,
                ChatId = chatId,
                Role = Role.Creator,
            };
            userChats.Add(userChat1);

            var userChat2 = new UserChatModel
            {
                UserId = user2,
                ChatId = chatId,
                Role = Role.Creator,
            };
            userChats.Add(userChat2);

            var chat = new ChatModel
            {
                Name = null,
                Type = ChatType.Personal,
                UserChats = userChats,
            };

            await _repository.Chat.CreateChatAsync(chat);
            await _repository.SaveAsync();
        }

        public async Task<ChatResponseDto> CreateCommonChatAsync(CreateCommonChatDto commonChatDto)
        {
            if (await _repository.User.GetUserAsync(u => u.Id == commonChatDto.AdminId) is null)
            {
                throw new UserNotFoundException();
            }

            var chatId = Guid.NewGuid();

            var userChat = new UserChatModel
            {
                UserId = commonChatDto.AdminId,
                ChatId = chatId,
                Role = Role.Administrator
            };

            var chat = new ChatModel
            {
                Id = chatId,
                Name = commonChatDto.Name,
                Type = ChatType.Common,
                UserChats = new List<UserChatModel>()
            };

            chat.UserChats.Add(userChat);

            await _repository.Chat.CreateChatAsync(chat);
            await _repository.SaveAsync();


            var chatDto = _mapper.Map<ChatResponseDto>(chat);
            return chatDto;
        }

        public async Task<List<UserChatResponseDto>> InviteUserToCommonChatAsync(Guid chatId,
            InviteUserCommonChatDto inviteUserCommonChatDto)
        {
            var currentUserId = _tokenService.GetCurrentUserId();

            var userChats = new List<UserChatResponseDto>();

            foreach (var userId in inviteUserCommonChatDto.UserIds)
            {
                var isUserExistsDb = await _repository.User.GetUserAsync(u => u.Id == userId) is not null;
                var isUsersExistsInChat = await _repository.UserChat.GetOneUserChatAsync(u => u.ChatId == chatId) is null;

                if (isUserExistsDb && !isUsersExistsInChat)
                {
                    var userChat = new UserChatModel
                    {
                        UserId = userId,
                        ChatId = chatId,
                        Role = Role.User
                    };

                    await _repository.UserChat.CreateUserChatAsync(userChat);
                    await _repository.SaveAsync();

                    var user = await _repository.User.GetUserAsync(u => u.Id == userId);

                    var notifyMessage = new MessageModel
                    {
                        Text = $"{user.Nickname}",
                        UserId = currentUserId,
                        Type = MessageType.InviteUser,
                        ChatId = chatId,
                        DispatchTime = DateTime.Now
                    };

                    await _repository.Message.CreateMessageAsync(notifyMessage);
                    await _repository.SaveAsync();

                    await _notificationService.NotifyChat(chatId, _mapper.Map<MessagesResponseDto>(notifyMessage));

                    var userChatDto = _mapper.Map<UserChatResponseDto>(userChat);
                    userChats.Add(userChatDto);
                }
            }

            return userChats;
        }

        public async Task<(List<ChatResponseDto> Data, MetaData MetaData)> GetAllCommonChatsOfUserAsync(Guid userId,
            ChatsParameters chatsParameters)
        {
            var chatModels = await _repository.Chat.GetAllChatsOfUserAsync(userId, chatsParameters);

            var result = _mapper.Map<List<ChatResponseDto>>(chatModels);

            return (Data: result, MetaData: chatModels.MetaData);
        }

        public async Task UpdateChatAsync(Guid id, string name)
        {
            var changeUser = _tokenService.GetCurrentUserId();

            var chat = await _repository.Chat.GetChatAsync(id);

            chat.Name = name;

            _repository.Chat.UpdateChat(chat);
            await _repository.SaveAsync();

            var notifyMessage = new MessageModel
            {
                Text = name,
                UserId = changeUser,
                ChatId = id,
                Type = MessageType.UpdateChat,
                DispatchTime = DateTime.Now,
            };

            await _repository.Message.CreateMessageAsync(notifyMessage);
            await _repository.SaveAsync();

            await _notificationService.NotifyChat(id, _mapper.Map<MessagesResponseDto>(notifyMessage));
        }

        public async Task RemoveUserInChatAsync(Guid remoteUserId, Guid chatId)
        {
            var userId = _tokenService.GetCurrentUserId();

            var remoteUserChat =
                await _repository.UserChat.GetOneUserChatAsync(u => u.UserId == remoteUserId && u.ChatId == chatId);

            var userChat =
                await _repository.UserChat.GetOneUserChatAsync(u => u.UserId == userId && u.ChatId == chatId);

            if (remoteUserChat is null || userChat is null)
            {
                throw new UserChatNotFoundException();
            }

            if (userChat.Role != Role.Administrator)
            {
                throw new IncorrectUserException();
            }

            var remoteUser = await _repository.User.GetUserAsync(u => u.Id == userId);

            var message = new MessageModel
            {
                Text = $"{remoteUser.Nickname}",
                UserId = userChat.UserId,
                ChatId = chatId,
                Type = MessageType.DeleteUser,
                DispatchTime = DateTime.Now
            };

            await _repository.Message.CreateMessageAsync(message);
            await _repository.SaveAsync();

            _repository.UserChat.DeleteUserChat(remoteUserChat);
            await _repository.SaveAsync();

            await _notificationService.NotifyChat(chatId, _mapper.Map<MessagesResponseDto>(message));
        }

        public async Task<(List<MessagesResponseDto> Data, MetaData MetaData)> GetAllMessageInCommonChatAsync(
            Guid chatId,
            MessagesParameters messagesParameters)
        {
            var chatIsReal = await _repository.Chat.GetChatAsync(chatId) is null;

            if (chatIsReal) throw new ChatNotFoundException();

            var allMessages = await _repository.Message.FindMessagesByConditionAsync(
                m => m.ChatId == chatId, false, messagesParameters);

            var listMess = _mapper.Map<List<MessagesResponseDto>>(allMessages);

            return (Data: listMess, MetaData: allMessages.MetaData);
        }

        public async Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChatAsync(Guid chatId,
            UsersParameters usersParameters)
        {
            if (await _repository.Chat.GetChatAsync(chatId) is null)
            {
                throw new ChatNotFoundException();
            }

            var users = await _repository.User.GetAllUsersInChatAsync(chatId, usersParameters);

            var usersDto = _mapper.Map<List<GetAllUsersDto>>(users);

            return (usersDto, users.MetaData);
        }
    }
}