using System;
using System.Linq;
using System.Linq.Expressions;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IUserChatRepository
    {
        UserChatModel GetOneUserChat(Func<UserChatModel, bool> predicate);
        void CreateUserChat(UserChatModel item);
        void DeleteUserChat(UserChatModel item);

        IQueryable<UserChatModel> FindUserChatByCondition(Expression<Func<UserChatModel, bool>> expression,
            bool trackChanges);
    }
}