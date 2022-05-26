using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Chat;
using Chat.Common.Dto.Chat;
using Chat.Common.Result;

namespace Chat.Core.Chat
{
    public interface IChatService
    {
        Task<ResultContainer<ChatResponseDto>> CreatePersonalChat(Guid user1, Guid user2);

        Task<ResultContainer<ChatResponseDto>> CreateCommonChat(CreateCommonChatDto commonChatDto);
    }
}