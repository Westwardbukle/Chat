using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.Token;
using Chat.Common.Dto.User;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IAuthService
    {
        Task Registration(RegisterUserDto registerUserDto);
        Task<TokenModel> Login(LoginUserDto loginUserDto);
        Task<IEnumerable<GetAllUsersDto>> GetAllUsersInChat(Guid chatId, UsersParameters usersParameters);
        Task UpdateUser(string nickname, string newNick);
    }
}