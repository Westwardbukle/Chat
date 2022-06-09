using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Chat;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Dto.User;
using Chat.Common.Exceptions;
using Chat.Common.Message;
using Chat.Common.RequestFeatures;
using Chat.Common.UsersRole;
using Chat.Core.Abstract;
using Chat.Database.Model;
using Chat.Database.Repository.Manager;

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


        public async Task CreatePersonalChat(Guid user1, Guid user2)
        {
            var chatId = Guid.NewGuid();

            var userChats = new List<UserChatModel>();

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
            var currentUserId = _tokenService.GetCurrentUserId();
            
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

                    var user = _repository.User.GetUser(u => u.Id == userId);
                    
                    var notifyMessage = new MessageModel
                    {
                        Text = $"{user.Nickname}",
                        UserId = currentUserId,
                        Type = MessageType.InviteUser,
                        ChatId = chatId,
                        DispatchTime = DateTime.Now
                    };

                    await _repository.Message.CreateMessage(notifyMessage);
                    await _repository.SaveAsync();
                    
                    await _notificationService.NotifyChat(chatId,_mapper.Map<MessagesResponseDto>(notifyMessage));
                }
                else
                {
                    throw new UserNotFoundException();
                }
            }
        }

        public async Task<(List<ChatResponseDto> Data , MetaData MetaData)> GetAllCommonChatsOfUser( Guid userId, ChatsParameters chatsParameters)
        {
            var chatModels = await _repository.Chat.GetAllChatsOfUser(userId, chatsParameters);
            
            var result = _mapper.Map<List<ChatResponseDto>>(chatModels);

            return (Data : result, MetaData: chatModels.MetaData);
        }

        public async Task UpdateChat(Guid id, string name)
        {
            var changeUser= _tokenService.GetCurrentUserId();
            
            var chat = _repository.Chat.GetChat(c => c.Id == id);

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

            await _repository.Message.CreateMessage(notifyMessage);
            await _repository.SaveAsync();
            
            await _notificationService.NotifyChat(id, _mapper.Map<MessagesResponseDto>(notifyMessage));
        }

        public async Task RemoveUserInChat(Guid remoteUserId, Guid chatId)
        {
            var userId = _tokenService.GetCurrentUserId();
            
            var remoteUserChat = _repository.UserChat.GetOneUserChat(u => u.UserId == remoteUserId && u.ChatId == chatId);

            var userChat = _repository.UserChat.GetOneUserChat(u => u.UserId == userId && u.ChatId == chatId);

            if (remoteUserChat is null || userChat is null)
            {
                throw new UserChatNotFoundException();
            }

            if (userChat.Role != Role.Administrator)
            {
                throw new PermissionDemied();
            }

            var remoteUser = _repository.User.GetUser(u => u.Id == userId);
            
            var message = new MessageModel
            {
                Text = $"{remoteUser.Nickname}",
                UserId = userChat.UserId,
                ChatId = chatId,
                Type = MessageType.DeleteUser,
                DispatchTime = DateTime.Now
            };

            await _repository.Message.CreateMessage(message);
            await _repository.SaveAsync();

            _repository.UserChat.DeleteUserChat(remoteUserChat);
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
        
        public async Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChat(Guid chatId, UsersParameters usersParameters)
        {
            if (!usersParameters.ValidDateRange)
            {
                throw new MaxDateRangeBadRequestException();
            }
            
            if (_repository.Chat.GetChat(c => c.Id == chatId) is null)
            {
                throw new ChatNotFoundException();
            }
            
            var users = await _repository.User.GetAllUsersIdsInChat(chatId, usersParameters);
            
            var usersDto = _mapper.Map<List<GetAllUsersDto>>(users);

            return (usersDto, users.MetaData);
        }
    }
}