using System.Threading.Tasks;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Code;
using Chat.Database.Repository.Message;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;

namespace Chat.Database.Repository.Manager
{
    public interface IRepositoryManager
    {
        IChatRepository Chat { get; }

        IUserRepository User { get; }

        ICodeRepository Code { get; }

        IMessageRepository Message { get; }

        IUserChatRepository UserChat { get; }

        Task SaveAsync();
    }
}