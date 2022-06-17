using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.Friend;
using Chat.Common.Dto.User;
using Chat.Common.Exceptions;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper, ITokenService tokenService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChatAsync(Guid chatId, UsersParameters usersParameters)
        {
            if (_repositoryManager.Chat.GetChatAsync( chatId) is null)
            {
                throw new ChatNotFoundException();
            }
            
            var users = await _repositoryManager.User.GetAllUsersInChatAsync(chatId, usersParameters);
            
            var usersDto = _mapper.Map<List<GetAllUsersDto>>(users);

            return (usersDto, users.MetaData);
        }
        
        public async Task UpdateUserAsync(string nickname, string newNick)
        {
            if (_repositoryManager.User.GetUserAsync(u => u.Nickname == nickname) is null)
            {
                throw new UserNotFoundException();
            }
            
            var user = await _repositoryManager.User.GetUserAsync(u => u.Nickname == nickname);

            user.Nickname = newNick;
            
            _repositoryManager.User.UpdateUser(user);

            await _repositoryManager.SaveAsync();
        }

        public async Task<GetAllUsersDto> GetOneUserAsync(string nickName)
        {
            var user = await _repositoryManager.User.GetUserAsync(u => u.Nickname == nickName);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            return _mapper.Map<GetAllUsersDto>(user);
        }
        
        public async Task<FriendResponseDto> SendFriendRequestAsync(Guid recipientId)
        {
            var senderId = _tokenService.GetCurrentUserId();
            
            var friendRequest = new FriendModel
            {
                UserId = senderId,
                FriendId = recipientId,
                Confirmed = false,
            };

            await _repositoryManager.Friend.CreateFriendRequestAsync(friendRequest);
            await _repositoryManager.SaveAsync();
            
            return _mapper.Map<FriendResponseDto>(friendRequest);
        }
    }
}