using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.User
{
    public class UserRepository: BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}