using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Extentions
{
    public static class FriendRepositoryExtentions
    {
        public static IQueryable<FriendModel> Filter(this IQueryable<FriendModel> friends, FriendParameters friendParameters)
        {
            if (friendParameters.MaxDate.HasValue)
            {
                friends = friends.Where(m => m.DateCreated <= friendParameters.MaxDate.Value);
            }

            if (friendParameters.MinDate.HasValue)
            {
                friends = friends.Where(m => m.DateCreated >= friendParameters.MinDate.Value);
            }

            if (friendParameters.UserId.HasValue)
            {
                friends = friends.Where(m => m.UserId == friendParameters.UserId.Value);
            }

            return friends;
        }
        
        public static IQueryable<FriendModel> SortFriends(this IQueryable<FriendModel> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.DateCreated);

            var orderQuery = CreateOrderQuery<UserModel>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.DateCreated);

            return employees.OrderBy(orderQuery);
        }
        
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            
            var propertyInfos = typeof(FriendModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
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
        
        public static IQueryable<FriendModel> Search(this IQueryable<FriendModel> employees,
            bool? searchTerm)
        {
            return searchTerm is null ? employees : employees.Where(e => e.Confirmed ==searchTerm);
        }
    }
}