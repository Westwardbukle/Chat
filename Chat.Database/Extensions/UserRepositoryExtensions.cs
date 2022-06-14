using System.Linq;
using System.Linq.Dynamic.Core;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Extensions
{
    public static class UserRepositoryExtensions
    {
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
        
        public static IQueryable<UserModel> SortUsers(this IQueryable<UserModel> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Nickname);

            var orderQuery = BaseExtensions.CreateOrderQuery<UserModel>(orderByQueryString);

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