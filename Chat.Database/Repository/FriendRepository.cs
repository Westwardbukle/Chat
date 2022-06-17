using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.AbstractRepository;
using Chat.Database.Extensions;
using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository
{
    public class FriendRepository : BaseRepository<FriendModel>, IFriendRepository
    {
        public FriendRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task CreateFriendRequestAsync(FriendModel friend)
            => await CreateAsync(friend);


        public async Task<FriendModel>  GetRequestAsync(Expression<Func<FriendModel, bool>> predicate)
            => await GetOneAsync(predicate);

        public void DeleteRequest(FriendModel friend)
            => Delete(friend);

        public void UpdateRequest(FriendModel friend)
            => Update(friend);

        public async Task<PagedList<FriendModel>> GetAllFriendsAsync(Guid userId, bool trackChanges,
            FriendParameters friendParameters)
        {
            if (friendParameters.ActiveTerm.HasValue)
            {
                var friends = await AppDbContext.FriendModels
                    .Where(f => f.FriendId == userId && f.Confirmed == friendParameters.ActiveTerm)
                    .Filter(friendParameters)
                    .Sort(friendParameters.OrderBy, f => f.DateCreated)
                    .ToListAsync();

                return PagedList<FriendModel>.ToPagedList(friends, friendParameters.PageNumber,
                    friendParameters.PageSize);
            }

            var friends1 = await AppDbContext.FriendModels
                .Where(f => f.FriendId == userId)
                .Filter(friendParameters)
                .Sort(friendParameters.OrderBy, f => f.DateCreated)
                .ToListAsync();

            return PagedList<FriendModel>.ToPagedList(friends1, friendParameters.PageNumber, friendParameters.PageSize);
        }
    }
}