using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Result;
using Chat.Common.User;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Auth
{
    public interface IAuthService
    {
        Task<ActionResult> Registration(RegisterUserDto registerUserDto);
        Task<ActionResult> Login(LoginUserDto loginUserDto);
        Task<ActionResult> GetAllUsers();
        Task<ActionResult> UpdateUser(string nickname, string newNick);
    }
}