using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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


        public async Task<ResultContainer<ChatResponseDto>> CreatePersonalChat(Guid user1, Guid user2)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var chatId = Guid.NewGuid();

            List<UserChatModel> userchats = new List<UserChatModel>();

            if (_userRepository.GetOne(u => u.Id == user1) is null ||
                _userRepository.GetOne(u => u.Id == user2) is null)
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

            await _chatRepository.Create(chat);

            return result;
        }

        public async Task<ResultContainer<ChatResponseDto>> InviteUserToCommonChat(Guid chatId,
            InviteUserCommonChatDto inviteUserCommonChatDto)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var chat = _chatRepository.GetOne(u => u.Id == chatId);


            foreach (var user in inviteUserCommonChatDto.UserIds.ToList())
            {
                var isUserExistsDb = _userRepository.GetOne(u => u.Id == user) is not null;

                var isUsersExistsInChat = _userChatRepository.GetOne(u => u.ChatId == chatId) is not null;

                if (!isUserExistsDb || !isUsersExistsInChat)
                {
                    inviteUserCommonChatDto.UserIds.Remove(user);
                }
            }


            foreach (var userId in inviteUserCommonChatDto.UserIds)
            {
                var userChat = new UserChatModel
                {
                    UserId = userId,
                    ChatId = chatId,
                    Role = Role.User
                };

                chat.UserChats.Add(userChat);
            }

            await _chatRepository.Create(chat);

            return result;
        }


        public async Task<ResultContainer<ChatResponseDto>> GetAllChats()
        {
            var result = new ResultContainer<ChatResponseDto>();

            List<ChatModel> chatModels = new List<ChatModel>();
            
            chatModels.AddRange(_chatRepository.GetAllObjects());

            result = _mapper.Map<ResultContainer<ChatResponseDto>>(chatModels);
            return result;
        }


        public async Task<ResultContainer<ChatResponseDto>> UpdateChat(Guid id, string name)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var chat = _chatRepository.GetOne(c => c.Id == id);
            chat.Name = name;
            await _chatRepository.Update(chat);
            return result;
        }


        public async Task<ResultContainer<ChatResponseDto>> AddUserInChat(Guid UserId, Guid chatId)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var user = _userRepository.GetById(UserId);

            var userchat = new UserChatModel
            {
                UserId = UserId,
                ChatId = chatId,
                Role = Role.User,
            };

            await _userChatRepository.Create(userchat);
            return result;
        }
        
        public async Task<ResultContainer<ChatResponseDto>> RemoveUserInChat(Guid userId, Guid chatId)
        {
            var result = new ResultContainer<ChatResponseDto>();

            var userchat = _userChatRepository.GetOne(u => u.UserId == userId && u.ChatId == chatId);

            await _userChatRepository.Delete(userchat.Id);

            return result;
        }
    }
}