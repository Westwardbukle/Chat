using System.Linq;
using System.Linq.Dynamic.Core;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Extensions
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
        
        /*public static IQueryable<MessageModel> SortMessages(this IQueryable<MessageModel> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.DispatchTime);

            var orderQuery = BaseExtensions.CreateOrderQuery<UserModel>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.DispatchTime);

            return employees.OrderBy(orderQuery);
        }*/
        
        public static IQueryable<MessageModel> Search(this IQueryable<MessageModel> employees,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Text.ToLower().Contains(lowerCaseTerm));
        }
    }
}