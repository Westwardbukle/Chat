using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Friend;
using Chat.Common.Dto.User;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IUserService
    {
        Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChatAsync(Guid chatId,
            UsersParameters usersParameters);

        Task UpdateUserAsync(string nickname, string newNick);

        Task<GetAllUsersDto> GetOneUserAsync(string nickName);

        Task<FriendResponseDto> SendFriendRequestAsync(Guid recipientId);
    }
}