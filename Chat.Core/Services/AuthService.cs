using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Code;
using Chat.Common.Dto;
using Chat.Common.Dto.Code;
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
using Chat.Database.Repository.Code;
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
        private readonly ICodeRepository _codeRepository;

        public AuthService
        (
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher hasher,
            ITokenService tokenService,
            ISmtpService smtpService,
            ICodeService code,
            ICodeRepository codeRepository
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hasher = hasher;
            _tokenService = tokenService;
            _smtpService = smtpService;
            _code = code;
            _codeRepository = codeRepository;
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
                DateTimeActivation = null,
            };


            var code = new CodeModel()
            {
                Id = id,
                Code = mailCode,
                CodePurpose = CodePurpose.ConfirmEmail,
                DateCreated = DateTime.Now,
                DateExpiration = DateTime.Now.AddHours(2),
                UserModelId = user.Id
            };


            await _userRepository.Create(user);
            await _codeRepository.Create(code);

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

        public async Task<ResultContainer<CodeResponseDto>> CodeСonfirmation(CodeDto codeDto)
        {
            var result = new ResultContainer<CodeResponseDto>();
            var code = _codeRepository.GetOne(c => c.UserModelId == _tokenService.GetCurrentUserId());

            if (code is null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            if (code.CodePurpose != CodePurpose.ConfirmEmail && code.DateExpiration > DateTime.Now)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = _userRepository.GetOne(u => u.Id == code.Id); 
            user.Active = true;
            user.DateTimeActivation = DateTime.Now;

            await _userRepository.Update(user);
            
            await _codeRepository.Delete(code.Id);
            
            result.ErrorType = ErrorType.Create;

            return result;
        }
    }
}