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


        public async Task<UserChatModel>  GetOneUserChatAsync(Expression<Func<UserChatModel, bool>> expression)
            => await GetOneAsync(expression);
        
        public async Task CreateUserChatAsync(UserChatModel item)
            => await CreateAsync(item);

        public void DeleteUserChat(UserChatModel item)
            => Delete(item);
    }
}