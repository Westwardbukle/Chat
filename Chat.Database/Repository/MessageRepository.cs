using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.AbstractRepository;
using Chat.Database.Extensions;
using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository
{
    public class MessageRepository : BaseRepository<MessageModel>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task CreateMessage(MessageModel item)
            =>  await CreateAsync(item);

        public async Task<PagedList<MessageModel>> FindMessagesByCondition(
            Expression<Func<MessageModel, bool>> expression,
            bool trackChanges, MessagesParameters messagesParameters)
        {
            var messages = await FindByCondition(expression, trackChanges)
                .Search(messagesParameters.SearchTerm, m => m.Text)
                .Filter(messagesParameters)
                .Sort(messagesParameters.OrderBy, x => x.DateCreated)
                .ToListAsync();

            return PagedList<MessageModel>
                .ToPagedList(messages, messagesParameters.PageNumber, messagesParameters.PageSize);
        }
    }
}