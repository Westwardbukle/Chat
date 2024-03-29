﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Friend;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IFriendService
    {
        Task<FriendResponseDto> ConfirmFriendRequestAsync(Guid unverifiedFriend, Guid userId);

        Task<FriendResponseDto> RejectFriendRequestAsync(Guid userId, Guid requestId);

        Task<(List<FriendResponseDto> Data, MetaData MetaData )> GetAllFriendsOfUserAsync(Guid userId,
            FriendParameters friendParameters);

        //Task<List<FriendResponseDto>> GetAllFriendsRequest(FriendParameters friendParameters);
    }
}