using System;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Message
{
    public interface IMessageRepository 
    {
        public void CreateMessage(MessageModel item);
    }
}