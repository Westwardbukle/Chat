using System;
using System.Linq;
using System.Linq.Expressions;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.UserChat
{
    public class UserChatRepository : BaseRepository<UserChatModel>, IUserChatRepository
    {
        public UserChatRepository(AppDbContext context) : base(context) {}


        public UserChatModel GetOneUserChat(Func<UserChatModel, bool> predicate)
            => GetOne(predicate);

        public void CreateUserChat(UserChatModel item)
            => Create(item);

        public void DeleteUserChat(UserChatModel item)
            => Delete(item);

        public IQueryable<UserChatModel> FindUserChatByCondition(Expression<Func<UserChatModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);
    }
}