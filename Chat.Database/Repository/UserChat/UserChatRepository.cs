using System;
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
    }
}