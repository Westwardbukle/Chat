using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Chat;
using Chat.Common.Dto.Chat;
using Chat.Common.Error;
using Chat.Common.Result;
using Chat.Common.UsersRole;
using Chat.Core.Chat;
using Chat.Database.Model;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;

namespace Chat.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserChatRepository _userChatRepository;

        public ChatService
        (
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserChatRepository userChatRepository
        )
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userChatRepository = userChatRepository;
        }


        public async Task<ResultContainer<ChatResponseDto>> CreatePersonalChat(Guid user1, Guid user2)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var chatId = Guid.NewGuid();

            List<UserChatModel> userchats = new List<UserChatModel>();

            if (_userRepository.GetOne(u => u.Id == user1) is null || _userRepository.GetOne(u => u.Id == user2) is null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
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

            await _chatRepository.Create(chat);

            result.ErrorType = ErrorType.Create;
            
            return result;
        }
        
        public async Task<ResultContainer<ChatResponseDto>> CreateCommonChat(CreateCommonChatDto commonChatDto)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var chatId = Guid.NewGuid();

            var chat = new ChatModel
            {
                Id = chatId,
                Name = null,
                Type = commonChatDto.chatType,
                UserChats = new List<UserChatModel>()
            };
            
            foreach (var userId in commonChatDto.userIds)
            {
                var userChat = new UserChatModel
                {
                    UserId = userId,
                    ChatId = chatId,
                    Role = commonChatDto.adminId == userId ? Role.Administrator : Role.User
                };
                
                chat.UserChats.Add(userChat);
            }
            
            await _chatRepository.Create(chat);

            return result;
        }
    }
}