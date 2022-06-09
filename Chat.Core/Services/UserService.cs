using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.User;
using Chat.Common.Exceptions;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Database.Repository.Manager;

namespace Chat.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChat(Guid chatId, UsersParameters usersParameters)
        {
            if (!usersParameters.ValidDateRange)
            {
                throw new MaxDateRangeBadRequestException();
            }
            
            if (_repositoryManager.Chat.GetChat(c => c.Id == chatId) is null)
            {
                throw new ChatNotFoundException();
            }
            
            var users = await _repositoryManager.User.GetAllUsersIdsInChat(chatId, usersParameters);
            
            var usersDto = _mapper.Map<List<GetAllUsersDto>>(users);

            return (usersDto, users.MetaData);
        }
        
        public async Task UpdateUser(string nickname, string newNick)
        {
            if (_repositoryManager.User.GetUser(u => u.Nickname == nickname) is null)
            {
                throw new UserNotFoundException();
            }
            
            var user = _repositoryManager.User.GetUser(u => u.Nickname == nickname);

            user.Nickname = newNick;
            
            _repositoryManager.User.UpdateUser(user);

            await _repositoryManager.SaveAsync();
        }
    }
}