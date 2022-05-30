using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Chat
{
    public interface IChatRepository 
    {
        Task<IEnumerable<ChatModel>>GetAllChats(bool trackChanges);

        ChatModel GetChat(Func<ChatModel, bool> predicate); 
        void CreateChat(ChatModel item);

        void UpdateChat(ChatModel item);
    }
}