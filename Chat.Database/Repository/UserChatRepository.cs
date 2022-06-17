using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Database.Repository
{
    public class UserChatRepository : BaseRepository<UserChatModel>, IUserChatRepository
    {
        public UserChatRepository(AppDbContext context) : base(context) {}


        public async Task<UserChatModel>  GetOneUserChat(Expression<Func<UserChatModel, bool>> expression)
            => await GetOne(expression);
        
        public async Task CreateUserChat(UserChatModel item)
            => await CreateAsync(item);

        public void DeleteUserChat(UserChatModel item)
            => Delete(item);

        /*public IQueryable<UserChatModel> FindUserChatByCondition(Expression<Func<UserChatModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);*/
    }
}