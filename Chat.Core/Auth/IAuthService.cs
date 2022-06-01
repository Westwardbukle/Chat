using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Auth
{
    public interface IAuthService
    {
        Task Registration(RegisterUserDto registerUserDto);
        Task<string> Login(LoginUserDto loginUserDto);
        Task<IEnumerable<GetAllUsersDto>> GetAllUsersInChat(Guid chatId);
        Task UpdateUser(string nickname, string newNick);
    }
}