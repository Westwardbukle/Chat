using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Message
{
    public class MessageRepository : BaseRepository<MessageModel>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
            
        }

        public void CreateMessage(MessageModel item)
            => Create(item);

        public IQueryable<MessageModel> FindMessageByCondition(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);
    }
}