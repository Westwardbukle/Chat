using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Chat
{
    public interface IChatService
    {
        Task<ActionResult> CreatePersonalChat(Guid user1, Guid user2);

        Task<ActionResult> CreateCommonChat(CreateCommonChatDto commonChatDto);

        Task<ActionResult> InviteUserToCommonChat(Guid chatId, InviteUserCommonChatDto inviteUserCommonChatDto);

        Task<ActionResult> GetAllChats();

        Task<ActionResult> UpdateChat(Guid id, string name);

        Task<ActionResult> RemoveUserInChat(Guid userId, Guid chatId);
    }
}