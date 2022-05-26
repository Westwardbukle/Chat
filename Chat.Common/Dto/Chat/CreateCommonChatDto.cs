using System;
using System.Collections.Generic;
using Chat.Common.Chat;

namespace Chat.Common.Dto.Chat
{
    public class CreateCommonChatDto
    {
        public List<Guid> userIds { get; set; }

        public ChatType chatType { get; set; } = ChatType.Common;

        public Guid? adminId { get; set; } = null;
    }
}