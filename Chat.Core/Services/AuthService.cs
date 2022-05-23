using System;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.User;
using Chat.Common.Error;
using Chat.Common.Result;
using Chat.Core.Auth;
using Chat.Core.Code;
using Chat.Core.Hashing;
using Chat.Core.Smtp;
using Chat.Core.Token;
using Chat.Database.Model;
using Chat.Database.Repository.User;

namespace Chat.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly ISmtpService _smtpService;
        private readonly ICodeService _code;

        public AuthService
        (
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher hasher,
            ITokenService tokenService,
            ISmtpService smtpService,
            ICodeService code
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hasher = hasher;
            _tokenService = tokenService;
            _smtpService = smtpService;
            _code = code;
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

            var mailCode = _code.GenerateRestoringCode();

            await _smtpService.SendEmailAsync(registerUserDto.Email, mailCode);
            
            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                DateofBirth = registerUserDto.DateOfBirth,
                Email = registerUserDto.Email,
                Password = _hasher.HashPassword(registerUserDto.Password),
                Active = false,
                Code = mailCode
            };
            
            
            await _userRepository.Create(user);
            
            

            result.ErrorType = ErrorType.Create;
            return result;
        }

        public async Task<ResultContainer<UserResponseDto>> Login(LoginUserDto loginUserDto)
        {
            var result = new ResultContainer<UserResponseDto>();

            var trueUser = _userRepository.GetOne(u => u.Nickname == loginUserDto.Nickname);
            
            if (!_hasher.VerifyHashedPassword(loginUserDto.Password, trueUser.Password))
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = _mapper.Map<UserModelDto>(trueUser);
            result = _mapper.Map<ResultContainer<UserResponseDto>>(trueUser);
            result.Data.Token = _tokenService.CreateToken(user);
            
            return result;
        }
    }
}