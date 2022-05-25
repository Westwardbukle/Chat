#nullable enable
using System;
using Chat.Common.Base;
using Chat.Common.Chat;

namespace Chat.Database.Model
{
    public class ChatModel : BaseModel
    {

        public string? Name { get; set; }
        
        public UserChatModel UserChat { get; set; }
        
        public MessageModel Messages { get; set; }
        
        public ChatType Type { get; set; }
    }
}