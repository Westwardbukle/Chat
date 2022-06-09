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
        Task CreatePersonalChat(Guid user1, Guid user2);

        Task<ChatResponseDto> CreateCommonChat(CreateCommonChatDto commonChatDto);

        Task<List<UserChatResponseDto>> InviteUserToCommonChat(Guid chatId, InviteUserCommonChatDto inviteUserCommonChatDto);

        Task<(List<ChatResponseDto> Data, MetaData MetaData)> GetAllCommonChatsOfUser(Guid userId,
            ChatsParameters chatsParameters);

        Task UpdateChat(Guid id, string name);

        Task RemoveUserInChat(Guid remoteUserId, Guid chatId);
        
        Task<(List<MessagesResponseDto> Data, MetaData MetaData)> GetAllMessageInCommonChat(Guid chatId,
            MessagesParameters messagesParameters);

        Task<(List<GetAllUsersDto> Data, MetaData MetaData)> GetAllUsersInChat(Guid chatId,
            UsersParameters usersParameters);
    }
}