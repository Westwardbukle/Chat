using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IUserChatRepository
    {
        Task<UserChatModel> GetOneUserChat(Expression<Func<UserChatModel, bool>> expression);
        Task CreateUserChat(UserChatModel item);
        void DeleteUserChat(UserChatModel item);

        /*IQueryable<UserChatModel> FindUserChatByCondition(Expression<Func<UserChatModel, bool>> expression,
            bool trackChanges);*/
    }
}