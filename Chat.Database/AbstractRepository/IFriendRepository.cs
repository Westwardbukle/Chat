﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IFriendRepository
    {
        Task CreateFriendRequestAsync(FriendModel friend);

        Task<FriendModel> GetRequestAsync(Expression<Func<FriendModel, bool>> predicate);

        void DeleteRequest(FriendModel friend);

        void UpdateRequest(FriendModel friend);

        Task<PagedList<FriendModel>> GetAllFriendsAsync(Guid userId, bool trackChanges,
            FriendParameters friendParameters);

        /*Task<PagedList<FriendModel>> GelAllRequest(Guid userId, bool trackChanges, FriendParameters friendParameters);*/
    }
}