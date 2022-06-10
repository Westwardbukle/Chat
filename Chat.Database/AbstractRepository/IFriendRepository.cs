using System;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IFriendRepository
    {
        Task CreateFriendRequest(FriendModel friend);

        FriendModel GetRequest(Func<FriendModel, bool> predicate);

        void DeleteRequest(FriendModel friend);

        void UpdateRequest(FriendModel friend);
    }
}