using System;
using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Code;
using Chat.Common.Dto.Login;
using Chat.Common.Result;
using Chat.Common.User;

namespace Chat.Core.Auth
{
    public interface IAuthService
    {
        Task<ResultContainer<UserResponseDto>> Registration(RegisterUserDto registerUserDto);
        Task<ResultContainer<UserResponseDto>> Login(LoginUserDto loginUserDto);
        Task<ResultContainer<CodeResponseDto>> CodeСonfirmation(CodeDto codeDto);
    }
}