using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.Token;
using Chat.Common.Dto.User;

namespace Chat.Core.Abstract
{
    public interface IAuthService
    {
        Task<UserRegisterResponseDto> Registration(RegisterUserDto registerUserDto);
        Task<TokenModel> Login(LoginUserDto loginUserDto);
    }
}