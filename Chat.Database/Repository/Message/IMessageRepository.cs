using System;
using System.Linq;
using System.Linq.Expressions;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Message
{
    public interface IMessageRepository 
    {
        public void CreateMessage(MessageModel item);

        IQueryable<MessageModel> FindMessageByCondition(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges);
    }
}