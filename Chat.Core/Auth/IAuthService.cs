using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Result;

namespace Chat.Core.Auth
{
    public interface IAuthService
    {
        public Task<ResultContainer<UserResponseDto>> Registration(RegisterUserDto registerUserDto);
        public Task<ResultContainer<UserResponseDto>> Login(LoginUserDto loginUserDto);
    }
}