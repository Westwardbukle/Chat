using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Chat;
using Chat.Common.Dto.Chat;
using Chat.Common.UsersRole;
using Chat.Core.Chat;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserChatRepository _userChatRepository;
        private readonly IMapper _mapper;

        public ChatService
        (
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserChatRepository userChatRepository,
            IMapper mapper
        )
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userChatRepository = userChatRepository;
            _mapper = mapper;
        }


        public async Task<ActionResult> CreatePersonalChat(Guid user1, Guid user2)
        {
            var chatId = Guid.NewGuid();

            List<UserChatModel> userchats = new List<UserChatModel>();

            if (_userRepository.GetUser(u => u.Id == user1) is null ||
                _userRepository.GetUser(u => u.Id == user2) is null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
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

             _chatRepository.CreateChat(chat);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        public async Task<ActionResult> CreateCommonChat(CreateCommonChatDto commonChatDto)
        {
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
            
            _chatRepository.CreateChat(chat);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        public async Task<ActionResult> InviteUserToCommonChat(Guid chatId, InviteUserCommonChatDto inviteUserCommonChatDto)
        {
            foreach (var userId in inviteUserCommonChatDto.UserIds)
            {
                var isUserExistsDb = _userRepository.GetUser(u => u.Id == userId) is not null;
                var isUsersExistsInChat = _userChatRepository.GetOne(u => u.ChatId == chatId) is null;

                if (isUserExistsDb && !isUsersExistsInChat)
                {
                    var userChat = new UserChatModel
                    {
                        UserId = userId,
                        ChatId = chatId,
                        Role = Role.User
                    };
                
                    await _userChatRepository.Create(userChat);
                    
                }
            }
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
        
        public async Task<ActionResult> GetAllChats()
        {
            var chatModels = new List<ChatModel>();
            
            chatModels.AddRange(await _chatRepository.GetAllChats(true));
            
            var result = _mapper.Map<List<ChatResponseDto>>(chatModels);
            
            return new OkObjectResult(result);
        }
        
        public async Task<ActionResult> UpdateChat(Guid id, string name)
        {
            var chat = _chatRepository.GetChat(c => c.Id == id);
            chat.Name = name;
             _chatRepository.UpdateChat(chat);
            return new StatusCodeResult(StatusCodes.Status200OK);
        }
        
        public async Task<ActionResult> RemoveUserInChat(Guid userId, Guid chatId)
        {
            var userChat = _userChatRepository.GetOne(u => u.UserId == userId && u.ChatId == chatId);

            if (userChat is null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            await _userChatRepository.Delete(userChat.Id);

            return new StatusCodeResult(StatusCodes.Status200OK);
        }
    }
}