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

        public async Task<PagedList<MessageModel>> FindMessagesByCondition(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges, MessagesFeatures messagesFeatures)
        {
            var messages = FindByCondition(expression, trackChanges);
            
            return PagedList<MessageModel>
                    .ToPagedList(messages, messagesFeatures.PageNumber, messagesFeatures.PageSize);
        } 

        public MessageModel GetOneMessage(Func<MessageModel, bool> predicate)
            => GetOne(predicate);
    }
}