using Chat.Database.Model;
using Chat.Database.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository.Chat
{
    public class ChatRepository : BaseRepository<ChatModel>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context){}
    }
}