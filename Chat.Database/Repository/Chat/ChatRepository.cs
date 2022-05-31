using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.Chat;
using Chat.Common.Exceptions;
using Chat.Common.UsersRole;
using Chat.Database.Model;
using Chat.Database.Repository.Base;
using Chat.Database.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository.Chat
{
    public class ChatRepository : BaseRepository<ChatModel>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context) {}


        public ChatModel GetPersonalChat(Guid user1, Guid user2)
        {
            var chat = AppDbContext.ChatModels.Include(x => x.UserChats)
                .FirstOrDefault(x => x.UserChats.All(y => y.UserId == user1 || y.UserId == user2) && x.Type == ChatType.Personal);
            
            return chat;
        }

        public async Task<IEnumerable<ChatModel>> GetAllChats(bool trackChanges)
            => await GetAllObjects(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();

        public IQueryable<ChatModel> FindChatByCondition(Expression<Func<ChatModel, bool>> expression,
            bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public ChatModel GetChat(Func<ChatModel, bool> predicate)
            => GetOne(predicate);

        public void CreateChat(ChatModel item)
            => Create(item);

        public void UpdateChat(ChatModel item)
            => Update(item);

        public Task<ChatModel> GetChatById(Guid id)
            => GetById(id);
    }
}