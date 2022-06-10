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
        Task<IEnumerable<ChatModel>>GetAllChats(bool trackChanges);

        ChatModel GetChat(Func<ChatModel, bool> predicate); 
        
        void CreateChat(ChatModel item);

        void UpdateChat(ChatModel item);
        
        Task<ChatModel> GetChatById(Guid id);

        IQueryable<ChatModel> FindChatByCondition(Expression<Func<ChatModel, bool>> expression,
            bool trackChanges);

        Task<ChatModel> GetPersonalChat(Guid user1, Guid user2);

        Task<PagedList<ChatModel>> GetAllChatsOfUser(Guid userId, ChatsParameters chatsParameters);
    }
}