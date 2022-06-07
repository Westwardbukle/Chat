using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Message
{
    public class MessageRepository : BaseRepository<MessageModel>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
            
        }

        public Task CreateMessage(MessageModel item)
            => Create(item);

        public IQueryable<MessageModel> FindMessagesByCondition(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges, MessagesFeatures messagesFeatures)
            => FindByCondition(expression, trackChanges)
                .Skip((messagesFeatures.PageNumber - 1) * messagesFeatures.PageSize)
                .Take(messagesFeatures.PageSize);

        public MessageModel GetOneMessage(Func<MessageModel, bool> predicate)
            => GetOne(predicate);
    }
}