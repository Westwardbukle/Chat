using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Chat
{
    public interface IChatService
    {
        Task CreatePersonalChat(Guid user1, Guid user2);

        Task CreateCommonChat(CreateCommonChatDto commonChatDto);

        Task InviteUserToCommonChat(Guid chatId, InviteUserCommonChatDto inviteUserCommonChatDto);

        Task<List<ChatResponseDto>> GetAllCommonChats();

        Task UpdateChat(Guid id, string name);

        Task RemoveUserInChat(Guid userId, Guid chatId);
    }
}