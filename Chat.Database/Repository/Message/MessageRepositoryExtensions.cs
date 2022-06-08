using System;
using System.Linq;
using System.Linq.Expressions;
using Chat.Common.Base;
using Chat.Common.Exceptions;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Repository.Message
{
    public static class MessageRepositoryExtensions
    {
        public static IQueryable<MessageModel> Filter(this IQueryable<MessageModel> messages,
            MessagesParameters messagesParameters)
        {
            if (messagesParameters.MaxDate.HasValue)
            {
                messages = messages.Where(m => m.DispatchTime <= messagesParameters.MaxDate.Value);
            }

            if (messagesParameters.MinDate.HasValue)
            {
                messages = messages.Where(m => m.DispatchTime >= messagesParameters.MinDate.Value);
            }

            if (messagesParameters.UserId.HasValue)
            {
                messages = messages.Where(m => m.UserId == messagesParameters.UserId.Value);
            }

            return messages;
        }
        
    }
    
    public static class RepositoryExtensions
    {
        public static IQueryable<T> Paginate<T, P>(this IQueryable<T> items,
            P parameters) where P : RequestParameters where T : BaseModel => items
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize);
    }
}