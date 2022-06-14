using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Chat.Common.Base;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Extensions
{
    public static class BaseExtensions
    {
        public static IQueryable<TSource> Sort<TSource, TKey>(this IQueryable<TSource> items, string orderByQueryString, Expression<Func<TSource, TKey>> defaultOrderProperty) where TSource : BaseModel
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return items.OrderBy(defaultOrderProperty);

            var orderQuery = CreateOrderQuery<TSource>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return items.OrderBy(defaultOrderProperty);

            return items.OrderBy(orderQuery);
        }
        
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> items, FriendParameters friendParameters) where  TSource : BaseModel
        {
            if (friendParameters.MaxDate.HasValue)
            {
                items = items.Where(m => m.DateCreated <= friendParameters.MaxDate.Value);
            }

            if (friendParameters.MinDate.HasValue)
            {
                items = items.Where(m => m.DateCreated >= friendParameters.MinDate.Value);
            }
            return items;
        }
        
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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
    }
}