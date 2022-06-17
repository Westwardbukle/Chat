using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Core.Abstract
{
    public interface IMessageService
    {
        Task<MessagesResponseDto> SendMessageAsync(Guid userId, Guid chatId, string text);

        
        Task<MessagesResponseDto> SendPersonalMessageAsync(Guid senderId, Guid recipientId, string text);

        Task<(List<MessagesResponseDto> Data, MetaData MetaData )> GetAllMessagesFromUserToUserAsync(Guid userId,
            Guid senderId, MessagesParameters messagesParameters);
    }
}