using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.User;
using Chat.Common.Exceptions;
using Chat.Core.Auth;
using Chat.Core.Hashing;
using Chat.Core.Token;
using Chat.Database.Model;
using Chat.Database.Repository.Manager;
using Chat.Database.Repository.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IRepositoryManager _repositoryManager;

        public AuthService
        (
            IMapper mapper,
            IPasswordHasher hasher,
            ITokenService tokenService,
            IRepositoryManager repositoryManager
        )
        {
            _mapper = mapper;
            _hasher = hasher;
            _tokenService = tokenService;
            _repositoryManager = repositoryManager;
        }

        public async Task Registration(RegisterUserDto registerUserDto)
        {
            var id = Guid.NewGuid();

            if (_repositoryManager.User.GetUser(u => u.Nickname == registerUserDto.Nickname) is not null)
            {
                throw new UserExistException();
            }

            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                DateofBirth = registerUserDto.DateOfBirth,
                Email = registerUserDto.Email,
                Password = _hasher.HashPassword(registerUserDto.Password),
                Active = false,
                DateTimeActivation = null,
            };

            _repositoryManager.User.CreateUser(user);
            await _repositoryManager.SaveAsync();
        }


        public async Task<string> Login(LoginUserDto loginUserDto)
        {
            var trueUser = _repositoryManager.User.GetUser(u => u.Nickname == loginUserDto.Nickname);

            if (!_hasher.VerifyHashedPassword(loginUserDto.Password, trueUser.Password))
            {
              throw new PasswordIncorrectException();
            }

            var user = _mapper.Map<UserModelDto>(trueUser);

            var token = _tokenService.CreateToken(user).Token;

            return token;
        }

        /*public async Task<IEnumerable<GetAllUsersDto>> GetAllUsersInChat(Guid chatId)
        {
            var users = await _repositoryManager.UserChat.FindUserChatByCondition( uc => uc.ChatId)

            var usersDto = _mapper.Map<IEnumerable<GetAllUsersDto>>(users);

            return usersDto;
        }*/


        public async Task UpdateUser(string nickname, string newNick)
        {
            var user = _repositoryManager.User.GetUser(u => u.Nickname == nickname);

            user.Nickname = newNick;
            
            _repositoryManager.User.UpdateUser(user);

            await _repositoryManager.SaveAsync();
        }
    }
}