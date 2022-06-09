using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Chat.Database.Model;
using System.Linq.Dynamic.Core;
using Chat.Common.RequestFeatures;

namespace Chat.Database.Repository.User
{
    public static class UserRepositoryExtentions
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            
            var propertyInfos = typeof(UserModel).GetProperties(BindingFlags.Public |
                                                               BindingFlags.Instance);
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
        
        public static IQueryable<UserModel> Filter(this IQueryable<UserModel> messages,
            UsersParameters usersParameters)
        {
            if (usersParameters.MaxDate.HasValue)
            {
                messages = messages.Where(m => m.DateOfBirth <= usersParameters.MaxDate.Value);
            }

            if (usersParameters.MinDate.HasValue)
            {
                messages = messages.Where(m => m.DateOfBirth >= usersParameters.MinDate.Value);
            }
            
            return messages;
        }
        
        public static IQueryable<UserModel> Sort(this IQueryable<UserModel> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Nickname);

            var orderQuery = CreateOrderQuery<UserModel>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Nickname);

            return employees.OrderBy(orderQuery);
        }
        public static IQueryable<UserModel> Search(this IQueryable<UserModel> employees,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Nickname.ToLower().Contains(lowerCaseTerm));
        }
    }
}