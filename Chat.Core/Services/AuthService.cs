using System;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.Token;
using Chat.Common.Dto.User;
using Chat.Common.Exceptions;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

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

        public async Task<UserRegisterResponseDto> RegistrationAsync(RegisterUserDto registerUserDto)
        {
            var id = Guid.NewGuid();

            if ( await _repositoryManager.User.GetUserAsync(u => u.Nickname == registerUserDto.Nickname) is not null)
            {
                throw new UserExistException();
            }

            var user = new UserModel
            {
                Id = id,
                DateCreated = DateTime.Now,
                Nickname = registerUserDto.Nickname,
                DateOfBirth = registerUserDto.DateOfBirth,
                Email = registerUserDto.Email,
                Password = _hasher.HashPassword(registerUserDto.Password),
                Active = false,
                DateTimeActivation = null,
            };

            await _repositoryManager.User.CreateUserAsync(user);
            await _repositoryManager.SaveAsync();

            var returnUser = _mapper.Map<UserRegisterResponseDto>(user);

            return returnUser;
        }


        public async Task<TokenModel> LoginAsync(LoginUserDto loginUserDto)
        {
            var trueUser = await _repositoryManager.User.GetUserAsync(u => u.Nickname == loginUserDto.Nickname);

            if (trueUser is null)
            {
                throw new UserNotFoundException();
            }

            if (!_hasher.VerifyHashedPassword(loginUserDto.Password, trueUser.Password))
            {
                throw new PasswordIncorrectException();
            }

            var user = _mapper.Map<UserModelDto>(trueUser);

            var token = _tokenService.CreateToken(user);

            return token;
        }
    }
}