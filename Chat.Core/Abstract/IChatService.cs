using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IChatService
    {
        Task CreatePersonalChat(Guid user1, Guid user2);

        Task CreateCommonChat(CreateCommonChatDto commonChatDto);

        Task InviteUserToCommonChat(Guid chatId, InviteUserCommonChatDto inviteUserCommonChatDto);

        Task<List<ChatResponseDto>> GetAllCommonChatsOfUser(Guid userid, ChatsParameters chatsParameters);

        Task UpdateChat(Guid id, string name);

        Task RemoveUserInChat(Guid remoteUserId, Guid chatId);
    }
}