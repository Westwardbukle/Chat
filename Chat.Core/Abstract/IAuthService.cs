using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.Token;

namespace Chat.Core.Abstract
{
    public interface IAuthService
    {
        Task Registration(RegisterUserDto registerUserDto);
        Task<TokenModel> Login(LoginUserDto loginUserDto);
    }
}