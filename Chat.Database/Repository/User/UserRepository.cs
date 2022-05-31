using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.Model;
using Chat.Database.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository.User
{
    public class UserRepository: BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers(bool trackChanges) 
            => await GetAllObjects(trackChanges)
                .OrderBy(c => c.Nickname)
                .ToListAsync();

        public UserModel GetUser(Func<UserModel, bool> predicate)
            => GetOne(predicate);

        public void CreateUser(UserModel item)
            => Create(item);

        public void UpdateUser(UserModel item)
            => Update(item);

        public Task<UserModel> GetUserById(Guid id)
            => GetById(id);

        public IQueryable<UserModel> FindUserByCondition(Expression<Func<UserModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);
        
        /*public IQueryable<UserModel> GetAllUsersInChat(Guid chatId)
        {
            var users = AppDbContext.UserModels.Include(u => u.UserChatModel)
                .
        }*/
    }
}