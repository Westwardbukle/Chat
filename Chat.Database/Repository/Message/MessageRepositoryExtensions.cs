using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
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
        
        public static IQueryable<MessageModel> SortMessages(this IQueryable<MessageModel> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.DispatchTime);

            var orderQuery = CreateOrderQuery<UserModel>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.DispatchTime);

            return employees.OrderBy(orderQuery);
        }
        
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            
            var propertyInfos = typeof(UserModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
            var orderQueryBuilder = new StringBuilder();
        
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                
                if (objectProperty == null)
                    continue;
                
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            return orderQuery;
        }
        public static IQueryable<MessageModel> Search(this IQueryable<MessageModel> employees,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Text.ToLower().Contains(lowerCaseTerm));
        }
    }
    
    /*public static class RepositoryExtensions
    {
        public static IQueryable<T> Paginate<T, P>(this IQueryable<T> items,
            P parameters) where P : RequestParameters where T : BaseModel => items
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize);
    }*/
    
    
    
    
}