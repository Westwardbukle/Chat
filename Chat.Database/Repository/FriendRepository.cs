using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.AbstractRepository;
using Chat.Database.Extentions;
using Chat.Database.Model;

namespace Chat.Database.Repository
{
    public class FriendRepository : BaseRepository<FriendModel>, IFriendRepository
    {
        public FriendRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task CreateFriendRequest(FriendModel friend)
            => await Create(friend);


        public FriendModel GetRequest(Func<FriendModel, bool> predicate)
            => GetOne(predicate);

        public void DeleteRequest(FriendModel friend)
            => Delete(friend);

        public void UpdateRequest(FriendModel friend)
            => Update(friend);

        public async Task<PagedList<FriendModel>> GetAllFriends(Guid userId, bool trackChanges,
            FriendParameters friendParameters)
        {
            var friends = GetAllObjects(trackChanges)
                .Where(f => f.FriendId == userId)
                .Filter(friendParameters)
                .Search(friendParameters.ActiveTerm)
                .SortFriends(friendParameters.OrderBy);

            return PagedList<FriendModel>.ToPagedList(friends, friendParameters.PageNumber, friendParameters.PageSize);
        }

        public async Task<PagedList<FriendModel>> GelAllRequest(Guid userId, bool trackChanges , FriendParameters friendParameters)
        {
            var friends = GetAllObjects(trackChanges)
                .Where(f => f.Confirmed == false && f.FriendId == userId)
                .Filter(friendParameters)
                .Search(friendParameters.ActiveTerm)
                .SortFriends(friendParameters.OrderBy);
            
            return PagedList<FriendModel>.ToPagedList(friends, friendParameters.PageNumber, friendParameters.PageSize);
        }
    }
}