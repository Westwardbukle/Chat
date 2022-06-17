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
        Task<PagedList<UserModel>> GetAllUsersInChat(Guid chatId, UsersParameters usersParameters);

        Task<List<Guid>> GetAllUsersIdsInChatForNotify(Guid chatId);
        
        Task<UserModel>  GetUser(Expression <Func<UserModel, bool>> predicate);

        Task CreateUser(UserModel item);

        void UpdateUser(UserModel item);
        
    }
}