using System;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.UserChat
{
    public interface IUserChatRepository
    {
        UserChatModel GetOneUserChat(Func<UserChatModel, bool> predicate);
        void CreateUserChat(UserChatModel item);
        void DeleteUserChat(UserChatModel item);
    }
}