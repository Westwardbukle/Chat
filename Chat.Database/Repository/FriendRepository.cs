using System;
using System.Threading.Tasks;
using Chat.Database.AbstractRepository;
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
    }
}