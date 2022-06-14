using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Friend;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IFriendService
    {
        Task<FriendResponseDto> ConfirmFriendRequest(Guid unverifiedFriend, Guid userId);

        Task<FriendResponseDto> RejectFriendRequest(Guid userId, Guid requestId);

        Task<List<FriendResponseDto>> GetAllFriendsOfUser(Guid userId, FriendParameters friendParameters);

        //Task<List<FriendResponseDto>> GetAllFriendsRequest(FriendParameters friendParameters);
    }
}