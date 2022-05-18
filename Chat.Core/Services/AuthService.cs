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
        private readonly IUserValidator _userValidator;

        public AuthService
        (
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher hasher,
            IUserValidator validator
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hasher = hasher;
            _userValidator = validator;
        }

        public async Task<ResultContainer<UserResponseDto>> Registration(RegisterUserDto registerUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();

            var id = Guid.NewGuid();

            if (!_userValidator.Validate(registerUserDto))
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                Age = registerUserDto.Age,
                Email = registerUserDto.Email,
                Password = _hasher.HashPassword(registerUserDto.Password)
            };
            result = _mapper.Map<ResultContainer<UserResponseDto>>(await _userRepository.Create(user));
            return result;
        }

        public async Task<ResultContainer<LoginUserDto>> Login(LoginUserDto loginUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();

            if (loginUserDto.Nickname == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }


            if (_hasher.VerifyHashedPassword(user.Password, loginUserDto.Password))
            {
            }


            //result = _mapper.Map<ResultContainer<UserResponseDto>>(user);
            return result;
        }
    }
}