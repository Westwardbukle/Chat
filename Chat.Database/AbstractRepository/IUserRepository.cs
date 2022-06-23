using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IUserRepository
    {
        Task<PagedList<UserModel>> GetAllUsersInChatAsync(Guid chatId, UsersParameters usersParameters);

        Task<IQueryable<UserModel>> GetAllUsersbyCondition(Expression<Func<UserModel, bool>> expression,
            bool trackChanges);

        Task<List<Guid>> GetAllUsersIdsInChatForNotify(Guid chatId);
        
        Task<UserModel>  GetUserAsync(Expression <Func<UserModel, bool>> predicate);

        Task CreateUserAsync(UserModel item);

        Task CreateUserRangeAsync(List<UserModel> users);

        void UpdateUser(UserModel item);
        
    }
}