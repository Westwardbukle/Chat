using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IUserChatRepository
    {
        Task<UserChatModel> GetOneUserChatAsync(Expression<Func<UserChatModel, bool>> expression);
        Task CreateUserChatAsync(UserChatModel item);
        void DeleteUserChat(UserChatModel item);

        /*IQueryable<UserChatModel> FindUserChatByCondition(Expression<Func<UserChatModel, bool>> expression,
            bool trackChanges);*/
    }
}