using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.UserChat
{
    public class UserChatRepository : BaseRepository<UserChatModel>, IUserChatRepository
    {
        public UserChatRepository(AppDbContext context) : base(context){}
    }
}