using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Friend;

namespace Chat.Core.Abstract
{
    public interface IFriendService
    {
        Task<FriendResponseDto> ConfirmFriendRequest(Guid id);
    }
}