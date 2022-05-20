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
        //var trueUser = _userRepository.GetOne(u => u.Nickname == registerUserDto.Nickname);

        public async Task<ResultContainer<UserResponseDto>> Registration(RegisterUserDto registerUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();

            var id = Guid.NewGuid();
            

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

            if (loginUserDto.Nickname == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var trueUser =  _userRepository.GetOne(u => u.Nickname == loginUserDto.Nickname);
            if (trueUser==null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            if (!_hasher.VerifyHashedPassword(trueUser.Password, loginUserDto.Password))
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }


            result = _mapper.Map<ResultContainer<UserResponseDto>>(user);
        }*/
    }
}