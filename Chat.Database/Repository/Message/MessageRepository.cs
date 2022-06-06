using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public IQueryable<MessageModel> FindMessagesByCondition(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public MessageModel GetOneMessage(Func<MessageModel, bool> predicate)
            => GetOne(predicate);
    }
}