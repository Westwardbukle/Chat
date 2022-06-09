using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.User;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IUserService
    {
        Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChat(Guid chatId,
            UsersParameters usersParameters);

        Task UpdateUser(string nickname, string newNick);

        Task<GetAllUsersDto> GetOneUser(string nickName);
    }
}