using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IUserRepository
    {
        Task<PagedList<UserModel>> GetAllUsersInChat(Guid chatId, UsersParameters usersParameters);
        
        IQueryable<UserModel> GetAllUsersIdsInChatForNotify(Guid chatId);
        
        UserModel GetUser(Func<UserModel, bool> predicate);

        Task CreateUser(UserModel item);

        void UpdateUser(UserModel item);
        
    }
}