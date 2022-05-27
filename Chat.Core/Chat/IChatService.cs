using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.Result;

namespace Chat.Core.Chat
{
    public interface IChatService
    {
        Task<ResultContainer<ChatResponseDto>> CreatePersonalChat(Guid user1, Guid user2);

        Task<ResultContainer<ChatResponseDto>> CreateCommonChat(CreateCommonChatDto commonChatDto);

        Task<ResultContainer<ChatResponseDto>> InviteUserToCommonChat(Guid chatId,
            InviteUserCommonChatDto inviteUserCommonChatDto);

        Task<ResultContainer<ChatResponseDto>> GetAllChats();

        Task<ResultContainer<ChatResponseDto>> UpdateChat(Guid id, string name);

        Task<ResultContainer<ChatResponseDto>> RemoveUserInChat(Guid userId, Guid chatId);
    }
}