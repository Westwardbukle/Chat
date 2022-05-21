using System;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Error;
using Chat.Common.Result;
using Chat.Core.Auth;
using Chat.Core.Hashing;
using Chat.Core.User;
using Chat.Core.Validating;
using Chat.Database.Model;
using Chat.Database.Repository.User;

namespace Chat.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public AuthService
        (
            IUserRepository userRepository,
            IMapper mapper,
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
                
            if (_userRepository.GetOne(u => u.Nickname == registerUserDto.Nickname) is not null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                DateofBirth = registerUserDto.DateOfBirth,
                Email = registerUserDto.Email,
                Password = _hasher.HashPassword(registerUserDto.Password)
            };
            
            await _userRepository.Create(user);

            result.ErrorType = ErrorType.Create;
            return result;
        }

        /*public async Task<ResultContainer<UserResponseDto>> Login(LoginUserDto loginUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();

            var trueUser = _userRepository.GetOne(u => u.Nickname == loginUserDto.Nickname);

            if (trueUser.Password != loginUserDto.Password)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            
        }*/
    }
}