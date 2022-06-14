using System.Linq;
using System.Linq.Dynamic.Core;
using Chat.Common.Base;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.Extensions
{
    public static class FriendRepositoryExtensions
    {
        public static IQueryable<FriendModel> SearchActiveFriend(this IQueryable<FriendModel> employees,
            bool? searchTerm)
        {
            return searchTerm is null ? employees : employees.Where(e => e.Confirmed == searchTerm);
        }
    }
}