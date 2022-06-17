using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.AbstractRepository;
using Chat.Database.Extensions;
using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository
{
    public class UserRepository: BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<PagedList<UserModel>> GetAllUsersInChatAsync(Guid chatId, UsersParameters usersParameters)
        {
            var users = await AppDbContext
                .UserModels
                .Include(u => u.UserChatModel)
                .Where(u => u.UserChatModel.Any(y => y.ChatId == chatId))
                .Filter(usersParameters)
                .Sort(usersParameters.OrderBy, x => x.Nickname)
                .Search(usersParameters.SearchTerm, u => u.Nickname)
                .ToListAsync();

            return PagedList<UserModel>
                .ToPagedList(users, usersParameters.PageNumber, usersParameters.PageSize);
        }

        public async Task<List<Guid>>  GetAllUsersIdsInChatForNotify(Guid chatId)
        {
            var users = await AppDbContext
                .UserModels
                .Include(u => u.UserChatModel)
                .Where(u => u.UserChatModel.Any(y => y.ChatId == chatId))
                .Select(u => u.Id)
                .ToListAsync();

            return users;
        }

        public async Task<UserModel>  GetUserAsync(Expression <Func<UserModel, bool>> predicate)
            => await GetOneAsync(predicate);

        public async Task CreateUserAsync(UserModel item)
            => await CreateAsync(item);

        public  void UpdateUser(UserModel item)
            =>  Update(item);
    }
}