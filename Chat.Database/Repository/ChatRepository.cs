using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Chat;
using Chat.Common.RequestFeatures;
using Chat.Database.AbstractRepository;
using Chat.Database.Extensions;
using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository
{
    public class ChatRepository : BaseRepository<ChatModel>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context) {}
        
        public async Task<ChatModel> GetPersonalChatAsync(Guid user1, Guid user2)
        {
            var chat = await AppDbContext.ChatModels
                .Include(x => x.UserChats)
                .FirstOrDefaultAsync(x => x.UserChats.All(y => y.UserId == user1 || y.UserId == user2)
                                          && x.Type == ChatType.Personal);
            return chat;
        }

        public async Task<ChatModel> GetChatAsync(Guid chatId)
            => await AppDbContext.ChatModels.FirstOrDefaultAsync(x => x.Id == chatId);

        public async Task CreateChatAsync(ChatModel item)
            => await CreateAsync(item);

        public void UpdateChat(ChatModel item)
            => Update(item);

        public async Task<PagedList<ChatModel>> GetAllChatsOfUserAsync(Guid userId, ChatsParameters chatsParameters)
        {
            var chats = await AppDbContext
                .ChatModels
                .Include(c => c.UserChats)
                .Where(u => u.UserChats.Any(y => y.UserId == userId))
                .Filter(chatsParameters)
                .Search(chatsParameters.SearchTerm, c => c.Name)
                .Sort(chatsParameters.OrderBy, c => c.DateCreated)
                .ToListAsync();
            return PagedList<ChatModel>
                .ToPagedList(chats, chatsParameters.PageNumber, chatsParameters.PageSize);
        }
    }
}