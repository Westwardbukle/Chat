using System.Collections.Generic;
using Chat.Common.Base;
using Chat.Common.Chat;

namespace Chat.Common.Dto.Chat
{
    public class ChatResponseDto
    {
        public string? Name { get; set; }
        
        public ChatType Type { get; set; }
    }
}