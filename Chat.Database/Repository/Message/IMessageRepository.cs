﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Repository.Message
{
    public interface IMessageRepository 
    {
        public Task CreateMessage(MessageModel item);

        IQueryable<MessageModel> FindMessagesByCondition(Expression<Func<MessageModel, bool>> expression,
            bool trackChanges, MessagesFeatures messagesFeatures);
    }
}