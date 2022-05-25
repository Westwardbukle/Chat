using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Message
{
    public class MessageRepository : BaseRepository<MessageModel>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}