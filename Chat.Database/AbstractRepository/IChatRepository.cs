using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface IChatRepository 
    {
        /*Task<IEnumerable<ChatModel>>GetAllChats(bool trackChanges);*/

        Task<ChatModel> GetChatAsync(Guid chatId);

        Task CreateChatAsync(ChatModel item);

        void UpdateChat(ChatModel item);
        
        /*Task<ChatModel> GetChatById(Guid id);*/

        /*IQueryable<ChatModel> FindChatByCondition(Expression<Func<ChatModel, bool>> expression,
            bool trackChanges);*/

        Task<ChatModel> GetPersonalChatAsync(Guid user1, Guid user2);

        Task<PagedList<ChatModel>> GetAllChatsOfUserAsync(Guid userId, ChatsParameters chatsParameters);
    }
}