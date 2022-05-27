using System.Collections.Generic;
using Chat.Common.Chat;

namespace Chat.Common.Dto.Chat
{
    public class ChatResponseDto
    {
        public List<AllChatsDto> Chats { get; set; }
    }
}