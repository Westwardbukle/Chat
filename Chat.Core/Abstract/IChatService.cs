using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Dto.User;
using Chat.Common.Dto.UserChat;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface IChatService
    {
        Task CreatePersonalChatAsync(Guid user1, Guid user2);

        Task<ChatResponseDto> CreateCommonChatAsync(CreateCommonChatDto commonChatDto);

        Task<List<UserChatResponseDto>> InviteUserToCommonChatAsync(Guid chatId, InviteUserCommonChatDto inviteUserCommonChatDto);

        Task<(List<ChatResponseDto> Data, MetaData MetaData)> GetAllCommonChatsOfUserAsync(Guid userId,
            ChatsParameters chatsParameters);

        Task UpdateChatAsync(Guid id, string name);

        Task RemoveUserInChatAsync(Guid remoteUserId, Guid chatId);
        
        Task<(List<MessagesResponseDto> Data, MetaData MetaData)> GetAllMessageInCommonChatAsync(Guid chatId,
            MessagesParameters messagesParameters);

        Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChatAsync(Guid chatId,
            UsersParameters usersParameters);
    }
}