using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.User;
using Chat.Core.Auth;
using Chat.Core.Hashing;
using Chat.Core.Token;
using Chat.Database.Model;
using Chat.Database.Repository.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;
       // private readonly 

        public AuthService
        (
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher hasher,
            ITokenService tokenService
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<ActionResult> Registration(RegisterUserDto registerUserDto)
        {
            var id = Guid.NewGuid();

            if (_userRepository.GetUser(u => u.Nickname == registerUserDto.Nickname) is not null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
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

             _userRepository.CreateUser(user);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }


        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
        {
            var trueUser = _userRepository.GetUser(u => u.Nickname == loginUserDto.Nickname);

            if (!_hasher.VerifyHashedPassword(loginUserDto.Password, trueUser.Password))
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            var user = _mapper.Map<UserModelDto>(trueUser);
            
            return new OkObjectResult(_tokenService.CreateToken(user).Token);
        }

        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers(false);

            var usersDto = _mapper.Map<IEnumerable<GetAllUsersDto>>(users);
            
            return new OkObjectResult(usersDto);
        }
        
        

        public async Task<ActionResult> UpdateUser(string nickname, string newNick)
        {
            var user = _userRepository.GetUser(u => u.Nickname == nickname);

            user.Nickname = newNick;

             _userRepository.UpdateUser(user);
             

            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}