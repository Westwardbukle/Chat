using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IMessageRepository 
    {
        public Task CreateMessageAsync(MessageModel item);

        Task<PagedList<MessageModel>> FindMessagesByConditionAsync(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges, MessagesParameters messagesParameters);
    }
}