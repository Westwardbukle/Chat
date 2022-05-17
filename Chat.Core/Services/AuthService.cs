using System;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Error;
using Chat.Common.Result;
using Chat.Core.Hashing;
using Chat.Core.Validating;
using Chat.Database.Model;
using Chat.Database.Repository.User;

namespace Chat.Core.Services
{
    public class AuthService
    {
        private readonly Mapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public AuthService
        (
            IUserRepository userRepository,
            Mapper mapper,
            IPasswordHasher hasher
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hasher = hasher;
        }

        public async Task<ResultContainer<UserResponseDto>> Registration(RegisterUserDto registerUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();

            var id = Guid.NewGuid();

            var email = registerUserDto.Email;

            if (!EmailValidator.IsEmailValid(email))
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var password = registerUserDto.Password;
            
            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                Age = registerUserDto.Age,
                Email = email,
                Password = _hasher.HashPassword(password)
            };
            result = _mapper.Map<ResultContainer<UserResponseDto>>(await _userRepository.Create(user));
            return result;
        }

        /*public Task<ResultContainer<UserResponseDto>> Login(LoginUserDto loginUserDto)
        {
        }*/
    }
}