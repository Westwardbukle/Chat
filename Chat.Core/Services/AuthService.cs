using System;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Error;
using Chat.Common.Result;
using Chat.Core.Validating;
using Chat.Database.Model;
using Chat.Database.Repository.User;

namespace Chat.Core.Services
{
    public class AuthService
    {
        private readonly Mapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthService
        (
            IUserRepository userRepository,
            Mapper mapper
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ResultContainer<UserResponseDto>> Registration(RegisterUserDto registerUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();
            
            var id = Guid.NewGuid();

            var email = registerUserDto.Email;

            if (!EmailValidator.IsEmailValid(email))
            {
                result = result.ErrorType == ErrorType.BadRequest;
            }

            EmailValidator.IsEmailValid(email)
            
            
            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                Age = registerUserDto.Age,
                Email = null,
                Password = null
            };
            result = 
            return result;
        }

        public Task<ResultContainer<UserResponseDto>> Login(LoginUserDto loginUserDto)
        {
        }
    }
}