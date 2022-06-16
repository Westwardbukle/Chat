using System;
using System.Linq;
using System.Linq.Expressions;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Database.Repository
{
    public class UserChatRepository : BaseRepository<UserChatModel>, IUserChatRepository
    {
        public UserChatRepository(AppDbContext context) : base(context) {}


        public UserChatModel GetOneUserChat(Func<UserChatModel, bool> predicate)
            => GetOne(predicate);

        public void CreateUserChat(UserChatModel item)
            => CreateAsync(item);

        public void DeleteUserChat(UserChatModel item)
            => Delete(item);

        public IQueryable<UserChatModel> FindUserChatByCondition(Expression<Func<UserChatModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);
    }
}