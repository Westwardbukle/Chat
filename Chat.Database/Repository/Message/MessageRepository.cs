using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;
using Chat.Database.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository.Message
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
            var messages = FindByCondition(expression, trackChanges)
                .Filter(messagesParameters)
                .SortMessages(messagesParameters.OrderBy);

            return PagedList<MessageModel>
                .ToPagedList(messages, messagesParameters.PageNumber, messagesParameters.PageSize);
        }
        
        /*public async Task<PagedList<MessageModel>> GetMessages(bool trackChanges, MessagesParameters messagesParameters)
        {
            var messages = await GetAllObjects(trackChanges)
                .Filter(messagesParameters)
                .SortMessages(messagesParameters.OrderBy)
                .Paginate(messagesParameters)
                .ToListAsync();

            return PagedList<MessageModel>
                .ToPagedList(messages, messagesParameters.PageNumber, messagesParameters.PageSize);
        }*/

        public MessageModel GetOneMessage(Func<MessageModel, bool> predicate)
            => GetOne(predicate);
    }
}