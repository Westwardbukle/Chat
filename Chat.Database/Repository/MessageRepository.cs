using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;
using Chat.Database.Repository.Message;

namespace Chat.Database.Repository
{
    public class MessageRepository : BaseRepository<MessageModel>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }

        public Task CreateMessage(MessageModel item)
            => Create(item);

        public async Task<PagedList<MessageModel>> FindMessagesByCondition(
            Expression<Func<MessageModel, bool>> expression,
            bool trackChanges, MessagesParameters messagesParameters)
        {
            var messages =  FindByCondition(expression, trackChanges)
                .Search(messagesParameters.SearchTerm)
                .Filter(messagesParameters)
                .SortMessages(messagesParameters.OrderBy);

            return PagedList<MessageModel>
                .ToPagedList(messages, messagesParameters.PageNumber, messagesParameters.PageSize);
        }
    }
}