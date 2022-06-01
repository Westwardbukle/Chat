using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Chat;
using Chat.Common.Dto.Chat;
using Chat.Common.Exceptions;
using Chat.Common.UsersRole;
using Chat.Core.Chat;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Manager;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ChatService
        (
            IRepositoryManager repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task CreatePersonalChat(Guid user1, Guid user2)
        {
            var chatId = Guid.NewGuid();

            List<UserChatModel> userchats = new List<UserChatModel>();

            if (_repository.User.GetUser(u => u.Id == user1) is null ||
                _repository.User.GetUser(u => u.Id == user2) is null)
            {
                throw new UserNotFoundException();
            }

            var userChat1 = new UserChatModel
            {
                UserId = user1,
                ChatId = chatId,
                Role = Role.Creator,
            };
            userchats.Add(userChat1);

            var userChat2 = new UserChatModel
            {
                UserId = user2,
                ChatId = chatId,
                Role = Role.Creator,
            };
            userchats.Add(userChat2);

            var chat = new ChatModel
            {
                Name = null,
                Type = ChatType.Personal,
                UserChats = userchats,
            };

            _repository.Chat.CreateChat(chat);
            await _repository.SaveAsync();
        }

        public async Task CreateCommonChat(CreateCommonChatDto commonChatDto)
        {
            if (_repository.User.GetUser(u => u.Id == commonChatDto.AdminId) is null)
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

            _repository.Chat.CreateChat(chat);
            await _repository.SaveAsync();
        }

        public async Task InviteUserToCommonChat(Guid chatId,
            InviteUserCommonChatDto inviteUserCommonChatDto)
        {
            foreach (var userId in inviteUserCommonChatDto.UserIds)
            {
                var isUserExistsDb = _repository.User.GetUser(u => u.Id == userId) is not null;
                var isUsersExistsInChat = _repository.UserChat.GetOneUserChat(u => u.ChatId == chatId) is null;

                if (isUserExistsDb && !isUsersExistsInChat)
                {
                    var userChat = new UserChatModel
                    {
                        UserId = userId,
                        ChatId = chatId,
                        Role = Role.User
                    };

                    _repository.UserChat.CreateUserChat(userChat);
                    await _repository.SaveAsync();
                }
                else
                {
                    throw new UserNotFoundException();
                }
            }
        }

        public async Task<List<ChatResponseDto>> GetAllCommonChatsOfUser()
        {
            var chatModels = new List<ChatModel>();

            chatModels.AddRange( _repository.Chat.FindChatByCondition( c=> c.Type == ChatType.Common, false));

            var result = _mapper.Map<List<ChatResponseDto>>(chatModels);

            return result;
        }

        public async Task UpdateChat(Guid id, string name)
        {
            var chat = _repository.Chat.GetChat(c => c.Id == id);

            chat.Name = name;

            _repository.Chat.UpdateChat(chat);
            await _repository.SaveAsync();
        }

        public async Task RemoveUserInChat(Guid userId, Guid chatId)
        {
            var userChat = _repository.UserChat.GetOneUserChat(u => u.UserId == userId && u.ChatId == chatId);

            if (userChat is null)
            {
                throw new UserChatNotFoundException();
            }

            _repository.UserChat.DeleteUserChat(userChat);
            await _repository.SaveAsync();
        }
    }
}