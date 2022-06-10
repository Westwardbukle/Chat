using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Repository.User
{
    public interface IUserRepository
    {
        Task<PagedList<UserModel>> GetAllUsersInChat(Guid chatId, UsersParameters usersParameters);
        
        IQueryable<UserModel> GetAllUsersIdsInChatForNotify(Guid chatId);
        
        UserModel GetUser(Func<UserModel, bool> predicate);

        void CreateUser(UserModel item);

        void UpdateUser(UserModel item);

        /*Task<UserModel> GetUserById(Guid id);

        IQueryable<UserModel> FindUserByCondition(Expression<Func<UserModel, bool>> expression,
            bool trackChanges);*/
    }
}